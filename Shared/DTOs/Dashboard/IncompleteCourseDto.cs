using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.Dashboard
{
    public class IncompleteCourseDto
    {
        public int CourseId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string ThumbnailUrl { get; set; } = string.Empty;
        public double ProgressPercentage { get; set; }         public string LastLessonTitle { get; set; } = string.Empty;
    }
}
