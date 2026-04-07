using Domain.Entities;
using Domain.Interfaces;
using ServicesAbstractions;
using Shared.DTOs.Wishlist;

namespace Services;

public class WishlistService : IWishlistService
{
    private readonly IUnitOfWork _unitOfWork;

    public WishlistService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<WishlistToggleResponseDto> ToggleWishlistAsync(string userId, int courseId)
    {
        var repo = _unitOfWork.Repository<Wishlist, int>();

        var wishlists = await repo.FindAsync(w => w.StudentId == userId && w.CourseId == courseId);
        var existingItem = wishlists.FirstOrDefault();

        if (existingItem != null)
        {
            repo.Delete(existingItem);
            await _unitOfWork.CompleteAsync();

            return new WishlistToggleResponseDto
            {
                IsAdded = false,
                Message = "Removed from wishlist"
            };
        }

        var newItem = new Wishlist
        {
            StudentId = userId,
            CourseId = courseId
        };

        await repo.AddAsync(newItem);
        await _unitOfWork.CompleteAsync();

        return new WishlistToggleResponseDto
        {
            IsAdded = true,
            Message = "Added to wishlist successfully"
        };
    }

    public async Task<List<WishlistCourseDto>> GetMyWishlistAsync(string userId)
    {
        var wishlists = await _unitOfWork.Repository<Wishlist, int>()
            .FindAsync(w => w.StudentId == userId);

        var courseIds = wishlists.Select(w => w.CourseId).ToList();

        var courses = await _unitOfWork.Repository<Course, int>()
            .FindAsync(c => courseIds.Contains(c.Id), c => c.Instructor);

        return wishlists.Select(w =>
        {
            var course = courses.FirstOrDefault(c => c.Id == w.CourseId);

            return new WishlistCourseDto
            {
                CourseId = w.CourseId,
                Title = course?.Title ?? string.Empty,
                ThumbnailUrl = course?.ThumbnailUrl,
                Price = course?.Price ?? 0,
                DiscountPrice = null,
                Rating = (double)(course?.Rating ?? 0),
                InstructorName = course?.Instructor?.User?.FirstName ?? "Instructor"
            };
        }).ToList();
    }
}