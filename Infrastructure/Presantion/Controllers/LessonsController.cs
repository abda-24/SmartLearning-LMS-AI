using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServicesAbstractions;
using Shared.DTOs.Courses.Lessons;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class LessonsController : ControllerBase
    {
        private readonly ILessonService _lessonService;

        public LessonsController(ILessonService lessonService)
        {
            _lessonService = lessonService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateLesson([FromBody] CreateLessonDto createLessonDto)
        {
            var result = await _lessonService.CreateLessonAsync(createLessonDto);
            return CreatedAtAction(nameof(GetLessonById), new { id = result.Id }, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetLessonById(int id)
        {
            var lesson = await _lessonService.GetLessonByIdAsync(id);
            return Ok(lesson);
        }

        [HttpGet("module/{moduleId}")]
        public async Task<IActionResult> GetLessonsByModuleId(int moduleId)
        {
            var lessons = await _lessonService.GetLessonsByModuleIdAsync(moduleId);
            return Ok(lessons);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLesson(int id, [FromBody] CreateLessonDto updateLessonDto)
        {
            await _lessonService.UpdateLessonAsync(id, updateLessonDto);
            return Ok(new { message = "Lesson updated successfully." });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLesson(int id)
        {
            await _lessonService.DeleteLessonAsync(id);
            return Ok(new { message = "Lesson deleted successfully." });
        }
    }
}