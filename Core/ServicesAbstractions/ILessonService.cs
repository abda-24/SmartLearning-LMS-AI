using Shared.DTOs.Courses.Lessons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesAbstractions
{
    public interface ILessonService
    {
        Task<LessonDto> CreateLessonAsync(CreateLessonDto createLessonDto);
        Task<LessonDto?> GetLessonByIdAsync(int id);
        Task<IEnumerable<LessonDto>> GetLessonsByModuleIdAsync(int moduleId);
        Task UpdateLessonAsync(int id, CreateLessonDto updateLessonDto);
        Task DeleteLessonAsync(int id);
    }
}
