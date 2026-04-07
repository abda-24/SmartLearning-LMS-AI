using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.Dashboard
{
    public class DashboardSummaryDto
    {
        public int TotalStudents { get; set; }
        public int TotalCourses { get; set; }
        public decimal TotalRevenue { get; set; }
        public int TotalEnrollments { get; set; }

        public List<MonthlySalesDto> MonthlySales { get; set; } = new();

        public List<TopCourseDto> TopCourses { get; set; } = new();
    }
}
