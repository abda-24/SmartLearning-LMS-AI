using Shared.DTOs.Courses.Enrollments;
using Shared.DTOs.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesAbstractions
{
    public interface IEnrollmentService
    {
        Task<PaginatedResultDto<EnrollmentDto>> GetPaginatedCourseEnrollmentsAsync(int courseId, int pageNumber, int pageSize);
        Task<EnrollmentDto> EnrollStudentAsync(CreateEnrollmentDto createEnrollmentDto);
        Task<IEnumerable<EnrollmentDto>> GetStudentEnrollmentsAsync(string studentId);
        Task<IEnumerable<EnrollmentDto>> GetCourseEnrollmentsAsync(int courseId);
        Task UnenrollStudentAsync(int id);
        
    }
}
