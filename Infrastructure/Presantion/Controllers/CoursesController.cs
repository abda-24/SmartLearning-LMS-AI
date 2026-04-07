using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presantion.Controllers; using ServicesAbstractions;
using Shared.DTOs.Courses;
using Shared.DTOs.Pagination;
using System.Security.Claims;

namespace LearningPlatform.Api.Controllers
{
    [Authorize]
    public class CoursesController(ICourseService courseService) : BaseController
    {
        [HttpGet]
        public async Task<ActionResult<PaginatedResultDto<CourseResponseDto>>> GetCourses(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await courseService.GetPaginatedCoursesAsync(pageNumber, pageSize);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CourseResponseDto>> GetById(int id)
        {
            var course = await courseService.GetCourseByIdAsync(id);
            return Ok(course);
        }

        [HttpPost]
        public async Task<ActionResult<CourseResponseDto>> Create(CreateCourseDto createCourseDto)
        {
            var createdCourse = await courseService.CreateCourseAsync(createCourseDto);
            return CreatedAtAction(nameof(GetById), new { id = createdCourse.Id }, createdCourse);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateCourseDto updateCourseDto)
        {
            if (id != updateCourseDto.Id)
                return BadRequest(new { message = "ID mismatch." });

            await courseService.UpdateCourseAsync(updateCourseDto);
            return Ok(new { message = "Course updated successfully." });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await courseService.DeleteCourseAsync(id);
            return Ok(new { message = "Course deleted successfully." });
        }
     
    }
}