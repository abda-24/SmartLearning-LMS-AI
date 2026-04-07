using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.Dashboard
{
    public class StudentDashboardDto
    {
        public int EnrolledCoursesCount { get; set; }
        public int CompletedCoursesCount { get; set; }
        public List<IncompleteCourseDto> InProgressCourses { get; set; } = new();
        public List<RecommendedCourseDto> RecommendedCourses { get; set; } = new();
    }
}
