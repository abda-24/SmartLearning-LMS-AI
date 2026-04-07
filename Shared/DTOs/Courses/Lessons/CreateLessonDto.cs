using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Courses.Lessons
{
    public class CreateLessonDto
    {
        [Required(ErrorMessage = "Lesson title is required")]
        [StringLength(150, MinimumLength = 3, ErrorMessage = "Title must be between 3 and 150 characters")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Lesson description is required")]
        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
        public string Description { get; set; } = string.Empty;

        [Url(ErrorMessage = "Invalid Video URL format")]
        public string? VideoUrl { get; set; }

        [Required(ErrorMessage = "Lesson order is required")]
        [Range(1, 500, ErrorMessage = "Order must be between 1 and 500")]
        public int Order { get; set; }

        [Required(ErrorMessage = "ModuleId is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Invalid ModuleId")]
        public int ModuleId { get; set; }
    }
}