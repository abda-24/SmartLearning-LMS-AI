using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using ServicesAbstractions;
using Shared.DTOs.Fawaterak_Payment;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Infrastructure.Services
{
    public class EInvoiceResponseModel
    {
        [JsonPropertyName("status")]
        public string status { get; set; } = string.Empty;

        [JsonPropertyName("data")]
        public FawaterakDataModel data { get; set; } = new();
    }

    public class FawaterakDataModel
    {
        [JsonPropertyName("invoice_id")]
        public int invoice_id { get; set; }

        [JsonPropertyName("url")]
        public string url { get; set; } = string.Empty;
    }

    public class FawaterakPaymentService : IFawaterakPaymentService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;
        private readonly string _vendorKey;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly string _baseUrl = "https://app.fawaterak.com/api/v2/";

        public FawaterakPaymentService(
            HttpClient httpClient,
            IConfiguration config,
            IUnitOfWork unitOfWork,
            UserManager<ApplicationUser> userManager)
        {
            _httpClient = httpClient;
            _config = config;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _vendorKey = _config["Fawaterak:VendorKey"] ?? throw new ArgumentNullException("VendorKey missing");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _vendorKey);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<(string? PaymentUrl, int? InvoiceId, string? ErrorMessage)> CreateInvoiceAsync(string userId, int courseId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                var course = await _unitOfWork.Repository<Course, int>().GetByIdAsync(courseId);

                if (course == null || user == null) return (null, null, "User or Course not found");

                var request = new EInvoiceRequestModel
                {
                    cartTotal = course.Price.ToString(),
                    currency = "EGP",
                    customer = new CustomerModel
                    {
                        first_name = user.FirstName ?? "Student",
                        last_name = user.LastName ?? "",
                        email = user.Email ?? "",
                        phone = user.PhoneNumber ?? "01000000000"
                    },
                    cartItems = new List<CartItemModel> { new() { name = course.Title, price = course.Price.ToString(), quantity = "1" } }
                };

                var response = await _httpClient.PostAsync($"{_baseUrl}createInvoiceLink",
                    new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json"));

                if (!response.IsSuccessStatusCode) return (null, null, "Payment Gateway Connection Failed");

                var responseString = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var result = JsonSerializer.Deserialize<EInvoiceResponseModel>(responseString, options);

                if (result == null || result.status != "success") return (null, null, "Payment Gateway Rejected Request");

                var transaction = new PaymentTransaction
                {
                    FawaterakInvoiceId = result.data.invoice_id,
                    StudentId = userId,
                    CourseId = courseId,
                    Amount = course.Price,
                    Status = "Pending"
                };

                await _unitOfWork.Repository<PaymentTransaction, int>().AddAsync(transaction);
                await _unitOfWork.CompleteAsync();

                return (result.data.url, result.data.invoice_id, null);
            }
            catch (Exception ex) { return (null, null, ex.Message); }
        }

        public async Task<bool> ProcessWebhookAsync(WebHookModel response)
        {
            var transaction = (await _unitOfWork.Repository<PaymentTransaction, int>().GetAllAsync())
                .FirstOrDefault(t => t.FawaterakInvoiceId == response.invoice_id);

            if (transaction == null || transaction.Status == "Paid") return false;

            if (response.invoice_status.Equals("paid", StringComparison.OrdinalIgnoreCase))
            {
                transaction.Status = "Paid";
                await _unitOfWork.Repository<Enrollment, int>().AddAsync(new Enrollment
                {
                    StudentId = transaction.StudentId,
                    CourseId = transaction.CourseId
                });
            }
            else if (response.invoice_status.Equals("failed", StringComparison.OrdinalIgnoreCase))
            {
                transaction.Status = "Failed";
            }

            _unitOfWork.Repository<PaymentTransaction, int>().Update(transaction);
            await _unitOfWork.CompleteAsync();
            return true;
        }
    }
}