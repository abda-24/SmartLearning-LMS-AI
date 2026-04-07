using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Fawaterak_Payment
{
    public class CartItemModel
    {
        [Required(ErrorMessage = "Item name is required")]
        [StringLength(100, ErrorMessage = "Item name cannot exceed 100 characters")]
        public string name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Price is required")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Price must be a valid number (e.g., 10.50)")]
        public string price { get; set; } = string.Empty;

        [Required(ErrorMessage = "Quantity is required")]
        [RegularExpression(@"^[1-9]\d*$", ErrorMessage = "Quantity must be at least 1")]
        public string quantity { get; set; } = "1";
    }
}