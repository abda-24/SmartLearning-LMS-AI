using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServicesAbstractions;
using Shared.DTOs.Courses.Enrollments;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EnrollmentsController(IEnrollmentService _enrollmentService) : ControllerBase
    {
        [HttpPost("enroll")]
        public async Task<IActionResult> EnrollStudent([FromBody] CreateEnrollmentDto createEnrollmentDto)
        {
            var result = await _enrollmentService.EnrollStudentAsync(createEnrollmentDto);
            return Ok(new { Message = "Enrollment successful", Data = result });
        }

        [HttpGet("student/{studentId}")]
        public async Task<IActionResult> GetStudentEnrollments(string studentId)
        {
            var result = await _enrollmentService.GetStudentEnrollmentsAsync(studentId);
            return Ok(result);
        }

        [HttpGet("course/{courseId}")]
        public async Task<IActionResult> GetCourseEnrollments(int courseId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _enrollmentService.GetPaginatedCourseEnrollmentsAsync(courseId, pageNumber, pageSize);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> UnenrollStudent(int id)
        {
            await _enrollmentService.UnenrollStudentAsync(id);
            return Ok(new { Message = "Unenrollment successful" });
        }
    }
}