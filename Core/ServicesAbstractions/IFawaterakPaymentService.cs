using Shared.DTOs.Fawaterak_Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesAbstractions
{
    public interface IFawaterakPaymentService
    {
        Task<(string? PaymentUrl, int? InvoiceId, string? ErrorMessage)> 
            CreateInvoiceAsync(string userId, int courseId);

        Task<bool> ProcessWebhookAsync(WebHookModel response);
    }
}
