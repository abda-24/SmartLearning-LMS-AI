using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.AI
{
    public class AIRequestDto
    {
        [Required(ErrorMessage = "LessonId is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Invalid LessonId")]
        public int LessonId { get; set; }

        [Required(ErrorMessage = "Question cannot be empty")]
        [StringLength(500, MinimumLength = 3, ErrorMessage = "Question must be between 3 and 500 characters")]
        public string UserQuestion { get; set; } = string.Empty;
    }
}