using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.AI
{
    public class PathStepDto
    {
        public int StepOrder { get; set; }
        public int CourseId { get; set; }
        public string CourseTitle { get; set; } = string.Empty;
        public string WhyThisCourse { get; set; } = string.Empty;
        public int EstimatedWeeks { get; set; }
    }
}
