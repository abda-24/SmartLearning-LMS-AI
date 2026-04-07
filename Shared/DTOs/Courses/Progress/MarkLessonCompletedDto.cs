using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Courses.Progress
{
    public class MarkLessonCompletedDto
    {
        [Required(ErrorMessage = "StudentId is required")]
        public string StudentId { get; set; } = string.Empty;

        [Required(ErrorMessage = "LessonId is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Invalid LessonId")]
        public int LessonId { get; set; }
    }
}