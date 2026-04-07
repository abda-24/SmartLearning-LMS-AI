using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServicesAbstractions;
using Shared.DTOs.Dashboard;
using System.Net.Mime;
using System.Security.Claims;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "dashboard")]
    [Authorize]
    [Produces(MediaTypeNames.Application.Json)]
    public class DashboardController(IDashboardService _dashboardService) : ControllerBase
    {
        [Authorize(Roles = "Admin")]
        [HttpGet("stats")]
        [ProducesResponseType(typeof(DashboardSummaryDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetStats()
        {
            var stats = await _dashboardService.GetAdminDashboardStatsAsync();

            if (stats == null)
            {
                return NotFound(new { Message = "Dashboard data not found." });
            }

            return Ok(stats);
        }

        [Authorize(Roles = "Instructor")]
        [HttpGet("instructor")]
        public async Task<IActionResult> GetInstructorStats()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "User ID not found in token" });
            }

            var stats = await _dashboardService.GetInstructorDashboardAsync(userId);
            return Ok(stats);
        }

        [Authorize(Roles = "Student")]
        [HttpGet("student")]
        public async Task<IActionResult> GetStudentStats()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var stats = await _dashboardService.GetStudentDashboardAsync(userId);
            return Ok(stats);
        }
    }
}