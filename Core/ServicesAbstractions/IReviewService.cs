using Shared.DTOs.Courses.Reviews;
using Shared.DTOs.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesAbstractions
{
    public interface IReviewService
    {
        Task<PaginatedResultDto<ReviewDto>> GetPaginatedReviewsAsync(int pageNumber, int pageSize);
        Task<ReviewDto> AddReviewAsync(CreateReviewDto createReviewDto);
        Task<IEnumerable<ReviewDto>> GetCourseReviewsAsync(int courseId);
        Task<ReviewDto?> GetReviewByIdAsync(int id);
        Task<bool> UpdateReviewAsync(int id, UpdateReviewDto updateReviewDto);
        Task<bool> DeleteReviewAsync(int id);

    }
}
