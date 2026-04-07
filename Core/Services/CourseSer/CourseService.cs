using AutoMapper;
using Domain.Custom_Exceptions;
using Domain.Entities;
using Domain.Interfaces;
using ServicesAbstractions;
using Shared.DTOs.Courses;
using Shared.DTOs.Pagination;

namespace Application.Services.CourseSer;

public class CourseService(IUnitOfWork unitOfWork, IMapper mapper, ICacheService cacheService) : ICourseService
{
    public async Task<PaginatedResultDto<CourseResponseDto>> GetPaginatedCoursesAsync(int pageNumber, int pageSize)
    {
        var cacheKey = $"Courses_Page_{pageNumber}_Size_{pageSize}";

        var cachedData = await cacheService.GetCachedDataAsync<PaginatedResultDto<CourseResponseDto>>(cacheKey);
        if (cachedData != null)
            return cachedData;

        var paginatedCourses = await unitOfWork.Repository<Course, int>().GetPaginatedAsync(pageNumber, pageSize, c => c.Instructor, c => c.Instructor.User, c => c.Modules);

        var response = new PaginatedResultDto<CourseResponseDto>
        {
            Items = mapper.Map<List<CourseResponseDto>>(paginatedCourses.Items),
            Metadata = new PaginationMetaDataDto
            {
                CurrentPage = paginatedCourses.Metadata.CurrentPage,
                TotalPages = paginatedCourses.Metadata.TotalPages,
                PageSize = paginatedCourses.Metadata.PageSize,
                TotalCount = paginatedCourses.Metadata.TotalCount
            }
        };

        await cacheService.SetCachedDataAsync(cacheKey, response, TimeSpan.FromMinutes(10));

        return response;
    }

    public async Task<IEnumerable<CourseResponseDto>> GetAllCoursesAsync()
    {
        var cacheKey = "All_Courses";

        var cachedCourses = await cacheService.GetCachedDataAsync<IEnumerable<CourseResponseDto>>(cacheKey);
        if (cachedCourses != null) return cachedCourses;

        var courses = await unitOfWork.Repository<Course, int>()
            .FindAsync(c => true, c => c.Instructor, c => c.Instructor.User, c => c.Modules);

        var response = mapper.Map<IEnumerable<CourseResponseDto>>(courses);

        await cacheService.SetCachedDataAsync(cacheKey, response, TimeSpan.FromMinutes(10));

        return response;
    }

    public async Task<CourseResponseDto> GetCourseByIdAsync(int id)
    {
        var cacheKey = $"Course_{id}";

        var cachedCourse = await cacheService.GetCachedDataAsync<CourseResponseDto>(cacheKey);
        if (cachedCourse != null) return cachedCourse;

        var courses = await unitOfWork.Repository<Course, int>()
            .FindAsync(c => c.Id == id, c => c.Instructor, c => c.Instructor.User, c => c.Modules);

        var course = courses.FirstOrDefault();

        if (course == null)
            throw new NotFoundException($"Course with ID {id} was not found.");

        var response = mapper.Map<CourseResponseDto>(course);

        await cacheService.SetCachedDataAsync(cacheKey, response, TimeSpan.FromMinutes(30));

        return response;
    }

    public async Task<CourseResponseDto> CreateCourseAsync(CreateCourseDto createCourseDto)
    {
        var newCourse = mapper.Map<Course>(createCourseDto);
        newCourse.CreatedAt = DateTime.UtcNow;

        await unitOfWork.Repository<Course, int>().AddAsync(newCourse);
        await unitOfWork.CompleteAsync();

        var createdCourse = await unitOfWork.Repository<Course, int>()
            .FindAsync(c => c.Id == newCourse.Id, c => c.Instructor, c => c.Instructor.User);

        await cacheService.RemoveCachedDataAsync("All_Courses");

        return mapper.Map<CourseResponseDto>(createdCourse.FirstOrDefault());
    }

    public async Task UpdateCourseAsync(UpdateCourseDto updateCourseDto)
    {
        var existingCourse = await unitOfWork.Repository<Course, int>().GetByIdAsync(updateCourseDto.Id);

        if (existingCourse == null)
            throw new NotFoundException($"Course with ID {updateCourseDto.Id} was not found.");

        mapper.Map(updateCourseDto, existingCourse);
        existingCourse.LastModifiedAt = DateTime.UtcNow;

        unitOfWork.Repository<Course, int>().Update(existingCourse);
        await unitOfWork.CompleteAsync();

        await cacheService.RemoveCachedDataAsync($"Course_{updateCourseDto.Id}");
        await cacheService.RemoveCachedDataAsync("All_Courses");
    }

    public async Task DeleteCourseAsync(int id)
    {
        var existingCourse = await unitOfWork.Repository<Course, int>().GetByIdAsync(id);

        if (existingCourse == null)
            throw new NotFoundException($"Course with ID {id} was not found.");

        unitOfWork.Repository<Course, int>().Delete(existingCourse);
        await unitOfWork.CompleteAsync();

        await cacheService.RemoveCachedDataAsync($"Course_{id}");
        await cacheService.RemoveCachedDataAsync("All_Courses");
    }
}