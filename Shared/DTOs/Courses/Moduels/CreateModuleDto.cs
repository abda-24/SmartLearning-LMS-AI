using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Courses.Moduels
{
    public class CreateModuleDto
    {
        [Required(ErrorMessage = "Module name is required")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Module name must be between 3 and 100 characters")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Module order is required")]
        [Range(1, 100, ErrorMessage = "Order must be between 1 and 100")]
        public int Order { get; set; }

        [Required(ErrorMessage = "CourseId is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Invalid CourseId")]
        public int CourseId { get; set; }
    }
}