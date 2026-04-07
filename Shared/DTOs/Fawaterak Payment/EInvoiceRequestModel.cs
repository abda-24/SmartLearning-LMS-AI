using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Fawaterak_Payment
{
    public class EInvoiceRequestModel
    {
        [Required(ErrorMessage = "Cart total is required")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Total must be a valid number")]
        public string cartTotal { get; set; } = string.Empty;

        [Required]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "Currency must be 3 characters (e.g., EGP)")]
        public string currency { get; set; } = "EGP";

        [Required(ErrorMessage = "Customer information is required")]
        public CustomerModel customer { get; set; } = new CustomerModel();

        [Required(ErrorMessage = "At least one item must be in the cart")]
        [MinLength(1, ErrorMessage = "Cart must contain at least one item")]
        public List<CartItemModel> cartItems { get; set; } = new List<CartItemModel>();
    }
}