using AutoMapper;
using Domain.Custom_Exceptions;
using Domain.Entities;
using Domain.Interfaces;
using ServicesAbstractions;
using Shared.DTOs.Courses.Enrollments;
using Shared.DTOs.Pagination;

namespace Services.CourseSer
{
    public class EnrollmentService(IUnitOfWork unitOfWork, IMapper mapper) : IEnrollmentService
    {
        public async Task<EnrollmentDto> EnrollStudentAsync(CreateEnrollmentDto createEnrollmentDto)
        {
            var enrollment = mapper.Map<Enrollment>(createEnrollmentDto);
            enrollment.EnrolledAt = DateTime.UtcNow;

            await unitOfWork.Repository<Enrollment, int>().AddAsync(enrollment);
            await unitOfWork.CompleteAsync();

            return mapper.Map<EnrollmentDto>(enrollment);
        }

        public async Task<IEnumerable<EnrollmentDto>> GetCourseEnrollmentsAsync(int courseId)
        {
            var courseEnrollments = await unitOfWork.Repository<Enrollment, int>()
                .FindAsync(e => e.CourseId == courseId);

            return mapper.Map<IEnumerable<EnrollmentDto>>(courseEnrollments);
        }

        public async Task<PaginatedResultDto<EnrollmentDto>> GetPaginatedCourseEnrollmentsAsync(int courseId, int pageNumber, int pageSize)
        {
            var result = await unitOfWork.Repository<Enrollment, int>()
                .GetPaginatedAsync(e => e.CourseId == courseId, pageNumber, pageSize);

            return new PaginatedResultDto<EnrollmentDto>
            {
                Items = mapper.Map<List<EnrollmentDto>>(result.Items),
                Metadata = new PaginationMetaDataDto
                {
                    CurrentPage = result.Metadata.CurrentPage,
                    TotalPages = result.Metadata.TotalPages,
                    PageSize = result.Metadata.PageSize,
                    TotalCount = result.Metadata.TotalCount
                }
            };
        }

        public async Task<IEnumerable<EnrollmentDto>> GetStudentEnrollmentsAsync(string studentId)
        {
            var studentEnrollments = await unitOfWork.Repository<Enrollment, int>()
                .FindAsync(e => e.StudentId == studentId);

            return mapper.Map<IEnumerable<EnrollmentDto>>(studentEnrollments);
        }

        public async Task UnenrollStudentAsync(int id)
        {
            var enrollment = await unitOfWork.Repository<Enrollment, int>().GetByIdAsync(id);

            if (enrollment == null)
                throw new NotFoundException($"Enrollment ID {id} not found");

            unitOfWork.Repository<Enrollment, int>().Delete(enrollment);
            await unitOfWork.CompleteAsync();
        }
    }
}