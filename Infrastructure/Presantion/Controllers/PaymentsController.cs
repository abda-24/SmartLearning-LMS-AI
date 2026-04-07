using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServicesAbstractions;
using Shared.DTOs.Fawaterak_Payment;
using System.Security.Claims;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController(IFawaterakPaymentService _paymentService) : ControllerBase
    {
        [HttpPost("create-invoice")]
        [Authorize]
        public async Task<IActionResult> CreateInvoice(int courseId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var (paymentUrl, invoiceId, errorMessage) = await _paymentService.CreateInvoiceAsync(userId, courseId);

            if (errorMessage != null) return BadRequest(new { Message = errorMessage });

            return Ok(new
            {
                Status = "Success",
                PaymentUrl = paymentUrl,
                RealInvoiceId = invoiceId
            });
        }

        [HttpPost("webhook")]
        [AllowAnonymous]
        public async Task<IActionResult> ReceivePaymentNotification([FromBody] WebHookModel response)
        {
            await _paymentService.ProcessWebhookAsync(response);
            return Ok();
        }
    }
}