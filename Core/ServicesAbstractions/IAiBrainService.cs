using Shared.DTOs.AI;
using Shared.DTOs.Courses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesAbstractions
{
    public interface IAiBrainService
    {
        Task<List<CourseRecommendationDto>> GetCourseRecommendationsAsync(string userId);
        Task<LearningPathDto?> GenerateLearningPathAsync(string userId, string currentLevel, string targetGoal);
    }
}
