using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServicesAbstractions;
using Shared.DTOs.Courses.Progress;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProgressController(IProgressService _progressService) : ControllerBase
    {
        [HttpPost("complete-lesson")]
        public async Task<IActionResult> MarkLessonAsCompleted([FromBody] MarkLessonCompletedDto dto)
        {
            var success = await _progressService.MarkLessonAsCompletedAsync(dto);

            if (!success)
                return BadRequest(new { Message = "Lesson already completed or invalid request" });

            return Ok(new { Message = "Lesson marked as completed successfully" });
        }

        [HttpGet("student/{studentId}/course/{courseId}")]
        public async Task<IActionResult> GetCourseProgress(string studentId, int courseId)
        {
            var progress = await _progressService.GetStudentCourseProgressAsync(studentId, courseId);
            return Ok(progress);
        }
    }
}