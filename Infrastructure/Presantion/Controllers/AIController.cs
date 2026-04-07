using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServicesAbstractions;
using Shared.DTOs.AI;
using System.Security.Claims;

namespace Presantion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AIController(IAIService _aiService, IAiBrainService _aiBrainService) : ControllerBase
    {
        [HttpPost("ask-lesson")]
        public async Task<IActionResult> AskAboutLesson([FromBody] AIRequestDto request)
        {
            var response = await _aiService.AskAboutLessonAsync(request);
            return Ok(response);
        }

        [HttpPost("generate-quiz")]
        public async Task<IActionResult> GenerateQuiz([FromBody] QuizRequestDto request)
        {
            var response = await _aiService.GenerateQuizAsync(request);
            return Ok(response);
        }

        [Authorize]
        [HttpGet("recommendations")]
        public async Task<IActionResult> GetAiRecommendations()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var result = await _aiBrainService.GetCourseRecommendationsAsync(userId);
            return Ok(result);
        }

        [Authorize]
        [HttpPost("generate-path")]
        public async Task<IActionResult> GenerateLearningPath([FromBody] GeneratePathRequestDto request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var learningPath = await _aiBrainService.GenerateLearningPathAsync(userId, request.CurrentLevel, request.TargetGoal);

            return Ok(new
            {
                Message = "تم إنشاء مسار التعلم الخاص بك بنجاح! 🚀",
                Path = learningPath
            });
        }
    }
}