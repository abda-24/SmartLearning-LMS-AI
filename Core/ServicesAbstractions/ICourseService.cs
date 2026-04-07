using Shared.DTOs.Courses;
using Shared.DTOs.Pagination;

namespace ServicesAbstractions
{
    public interface ICourseService
    {
        Task<PaginatedResultDto<CourseResponseDto>> GetPaginatedCoursesAsync(int pageNumber, int pageSize);

        Task<IEnumerable<CourseResponseDto>> GetAllCoursesAsync();  
        Task<CourseResponseDto> GetCourseByIdAsync(int id);
        Task<CourseResponseDto> CreateCourseAsync(CreateCourseDto createCourseDto);
        Task UpdateCourseAsync (UpdateCourseDto updateCourseDto);
        Task DeleteCourseAsync(int id);
    }
}
