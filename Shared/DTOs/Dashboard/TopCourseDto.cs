using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.Dashboard
{
    public class TopCourseDto
    {
        public string CourseName { get; set; } = string.Empty;
        public int EnrolledStudents { get; set; }
    }
}
