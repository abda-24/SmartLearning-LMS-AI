using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Courses.Reviews
{
    public class CreateReviewDto
    {
        [Required(ErrorMessage = "StudentId is required")]
        public string StudentId { get; set; } = string.Empty;

        [Required(ErrorMessage = "CourseId is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Invalid CourseId")]
        public int CourseId { get; set; }

        [Required(ErrorMessage = "Rating is required")]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5 stars")]
        public int Rating { get; set; }

        [StringLength(500, ErrorMessage = "Comment cannot exceed 500 characters")]
        public string? Comment { get; set; }
    }
}