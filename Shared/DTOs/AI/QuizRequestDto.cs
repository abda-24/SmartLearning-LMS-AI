using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.AI
{
    public class QuizRequestDto
    {
        [Required(ErrorMessage = "LessonId is required")]
        [Range(1, int.MaxValue)]
        public int LessonId { get; set; }

        [Range(1, 20, ErrorMessage = "Question count must be between 1 and 20")]
        public int QuestionCount { get; set; } = 10;

        public bool IncludeMCQ { get; set; } = true;
        public bool IncludeTrueFalse { get; set; } = true;
        public bool IncludeEssay { get; set; } = false;
    }
}