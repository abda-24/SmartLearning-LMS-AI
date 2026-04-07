using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServicesAbstractions;
using Shared.DTOs.Courses.Reviews;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewsController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllReviews([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _reviewService.GetPaginatedReviewsAsync(pageNumber, pageSize);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddReview([FromBody] CreateReviewDto createReviewDto)
        {
            var result = await _reviewService.AddReviewAsync(createReviewDto);
            return CreatedAtAction(nameof(GetReviewById), new { id = result.Id }, new { message = "Review added successfully.", data = result });
        }

        [HttpGet("course/{courseId}")]
        public async Task<IActionResult> GetCourseReviews(int courseId)
        {
            var reviews = await _reviewService.GetCourseReviewsAsync(courseId);
            return Ok(reviews);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetReviewById(int id)
        {
            var review = await _reviewService.GetReviewByIdAsync(id);
            return Ok(review);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReview(int id, [FromBody] UpdateReviewDto updateReviewDto)
        {
            await _reviewService.UpdateReviewAsync(id, updateReviewDto);
            return Ok(new { message = "Review updated successfully." });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReview(int id)
        {
            await _reviewService.DeleteReviewAsync(id);
            return Ok(new { message = "Review deleted successfully." });
        }
    }
}