using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Fawaterak_Payment
{
    public class WebHookModel
    {
        [Required(ErrorMessage = "Invoice ID is required")]
        public int invoice_id { get; set; }

        [Required(ErrorMessage = "Invoice key is required")]
        public string invoice_key { get; set; } = string.Empty;

        [Required(ErrorMessage = "Invoice status is required")]
        public string invoice_status { get; set; } = string.Empty;
    }
}