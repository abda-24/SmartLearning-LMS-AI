using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.AI
{
    public class QuizResponseDto
    {
        public List<QuestionDto> Questions { get; set; } = new();
    }
}
