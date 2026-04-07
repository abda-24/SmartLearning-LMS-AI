using Domain.Entities;
using Domain.Interfaces;
using ServicesAbstractions;
using Shared.DTOs.Courses.Progress;

namespace Application.Services
{
    public class ProgressService(IUnitOfWork _unitOfWork) : IProgressService
    {
        public async Task<bool> MarkLessonAsCompletedAsync(MarkLessonCompletedDto dto)
        {
            var existingProgress = await _unitOfWork.Repository<LessonProgress, int>()
                .FindAsync(p => p.StudentId == dto.StudentId && p.LessonId == dto.LessonId);

            if (existingProgress.Any()) return false;

            var progress = new LessonProgress
            {
                StudentId = dto.StudentId,
                LessonId = dto.LessonId,
                CompletedAt = DateTime.UtcNow
            };

            await _unitOfWork.Repository<LessonProgress, int>().AddAsync(progress);
            await _unitOfWork.CompleteAsync();

            return true;
        }

        public async Task<CourseProgressDto> GetStudentCourseProgressAsync(string studentId, int courseId)
        {
            var courseModules = await _unitOfWork.Repository<Domain.Entities.Modules, int>()
                .FindAsync(m => m.CourseId == courseId);

            var moduleIds = courseModules.Select(m => m.Id).ToList();

            var courseLessons = await _unitOfWork.Repository<Lesson, int>()
                .FindAsync(l => moduleIds.Contains(l.ModuleId));

            var lessonIds = courseLessons.Select(l => l.Id).ToList();
            var totalLessons = lessonIds.Count;

            var studentProgress = await _unitOfWork.Repository<LessonProgress, int>()
                .FindAsync(p => p.StudentId == studentId && lessonIds.Contains(p.LessonId));

            var completedLessons = studentProgress.Count();

            double percentage = totalLessons == 0 ? 0 : Math.Round((double)completedLessons / totalLessons * 100, 2);

            return new CourseProgressDto
            {
                CourseId = courseId,
                TotalLessons = totalLessons,
                CompletedLessons = completedLessons,
                ProgressPercentage = percentage
            };
        }
    }
}