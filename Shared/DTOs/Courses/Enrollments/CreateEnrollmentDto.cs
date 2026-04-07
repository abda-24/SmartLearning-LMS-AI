using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Courses.Enrollments
{
    public class CreateEnrollmentDto
    {
        [Required(ErrorMessage = "StudentId is required")]
        public string StudentId { get; set; } = string.Empty;

        [Required(ErrorMessage = "CourseId is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Invalid CourseId")]
        public int CourseId { get; set; }
    }
}