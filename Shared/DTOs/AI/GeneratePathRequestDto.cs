using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.AI
{
    public class GeneratePathRequestDto
    {
        [Required(ErrorMessage = "Current level is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Level description must be between 2 and 50 characters")]
        public string CurrentLevel { get; set; } = string.Empty;

        [Required(ErrorMessage = "Target goal is required")]
        [StringLength(200, MinimumLength = 5, ErrorMessage = "Goal description must be between 5 and 200 characters")]
        public string TargetGoal { get; set; } = string.Empty;
    }
}