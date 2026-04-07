using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.AI
{
    public class LearningPathDto
    {
        public string Title { get; set; } = string.Empty;
        public string Overview { get; set; } = string.Empty;
        public int EstimatedTotalWeeks { get; set; }
        public List<PathStepDto> Steps { get; set; } = new List<PathStepDto>();
    }
}
 