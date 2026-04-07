using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using ServicesAbstractions;
using Shared.DTOs.Courses.Reviews;
using Shared.DTOs.Pagination;

namespace Application.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ReviewService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
            public async Task<PaginatedResultDto<ReviewDto>> GetPaginatedReviewsAsync(int pageNumber, int pageSize)
            {
                var result = await _unitOfWork.Repository<Review, int>().GetPaginatedAsync(pageNumber, pageSize);

                return new PaginatedResultDto<ReviewDto>
                {
                    Items = _mapper.Map<List<ReviewDto>>(result.Items),
                    Metadata = new PaginationMetaDataDto
                    {
                        CurrentPage = result.Metadata.CurrentPage,
                        TotalPages = result.Metadata.TotalPages,
                        PageSize = result.Metadata.PageSize,
                        TotalCount = result.Metadata.TotalCount
                    }
                };
            }

            public async Task<ReviewDto> AddReviewAsync(CreateReviewDto createReviewDto)
        {
            var review = _mapper.Map<Review>(createReviewDto);
            review.CreatedAt = DateTime.UtcNow;

            await _unitOfWork.Repository<Review, int>().AddAsync(review);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<ReviewDto>(review);
        }

        public async Task<IEnumerable<ReviewDto>> GetCourseReviewsAsync(int courseId)
        {
            var allReviews = await _unitOfWork.Repository<Review, int>().GetAllAsync();
            var courseReviews = allReviews.Where(r => r.CourseId == courseId);

            return _mapper.Map<IEnumerable<ReviewDto>>(courseReviews);
        }

        public async Task<ReviewDto?> GetReviewByIdAsync(int id)
        {
            var review = await _unitOfWork.Repository<Review, int>().GetByIdAsync(id);
            if (review == null) return null;

            return _mapper.Map<ReviewDto>(review);
        }

        public async Task<bool> UpdateReviewAsync(int id, UpdateReviewDto updateReviewDto)
        {
            var review = await _unitOfWork.Repository<Review, int>().GetByIdAsync(id);
            if (review == null) return false;

            review.Rating = updateReviewDto.Rating;
            review.Comment = updateReviewDto.Comment;

            _unitOfWork.Repository<Review, int>().Update(review);
            await _unitOfWork.CompleteAsync();

            return true;
        }

        public async Task<bool> DeleteReviewAsync(int id)
        {
            var review = await _unitOfWork.Repository<Review, int>().GetByIdAsync(id);
            if (review == null) return false;

            _unitOfWork.Repository<Review, int>().Delete(review);
            await _unitOfWork.CompleteAsync();

            return true;
        }
    }
}