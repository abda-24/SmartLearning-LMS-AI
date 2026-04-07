using Shared.DTOs.AI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesAbstractions
{
    public interface IAIService
    {
        Task<AIResponseDto> AskAboutLessonAsync(AIRequestDto request);
        Task<QuizResponseDto> GenerateQuizAsync(QuizRequestDto request);
    }
}
