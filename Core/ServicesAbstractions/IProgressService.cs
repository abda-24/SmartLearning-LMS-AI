using Shared.DTOs.Courses.Progress;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesAbstractions
{
    public interface IProgressService
    {
        Task<bool> MarkLessonAsCompletedAsync(MarkLessonCompletedDto dto);
        Task<CourseProgressDto> GetStudentCourseProgressAsync(string studentId, int courseId);
    }
}
