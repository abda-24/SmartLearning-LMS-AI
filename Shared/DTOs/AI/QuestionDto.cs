using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.AI
{
    public class QuestionDto
    {
        public string QuestionType { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;         public List<string>? Options { get; set; }
        public string CorrectAnswer { get; set; } = string.Empty;
    }
}
