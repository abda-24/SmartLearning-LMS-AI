using AutoMapper;
using Domain.Entities;
using Shared.DTOs.Courses;
using Shared.DTOs.Courses.Categories;
using Shared.DTOs.Courses.Enrollments;
using Shared.DTOs.Courses.Lessons;
using Shared.DTOs.Courses.Moduels;
using Shared.DTOs.Courses.Reviews;

public class CourseProfile : Profile
{
    public CourseProfile()
    {
        
        CreateMap<Course, CourseResponseDto>()

            .ForMember(dest => dest.InstructorName,
                opt => opt.MapFrom(src =>
                    (src.Instructor != null && src.Instructor.User != null)
                    ? $"{src.Instructor.User.FirstName} {src.Instructor.User.LastName}"
                    : "No Instructor"))

            .ForMember(dest => dest.ModulesCount,
                opt => opt.MapFrom(src => src.Modules != null ? src.Modules.Count : 0));

        CreateMap<CreateCourseDto, Course>()
            .ForMember(dest => dest.InstructorId,
                opt => opt.MapFrom(src => src.InstructorId));
                CreateMap<UpdateCourseDto, Course>();

                CreateMap<Modules, ModuleDto>().ReverseMap();
        CreateMap<CreateModuleDto, Modules>();

        CreateMap<Lesson, LessonDto>().ReverseMap();
        CreateMap<CreateLessonDto, Lesson>();

        CreateMap<Category, CategoryDto>().ReverseMap();
        CreateMap<CreateCategoryDto, Category>();

        CreateMap<Enrollment, EnrollmentDto>().ReverseMap();
        CreateMap<CreateEnrollmentDto, Enrollment>();

        CreateMap<Review, ReviewDto>().ReverseMap();
        CreateMap<CreateReviewDto, Review>();
    }
}