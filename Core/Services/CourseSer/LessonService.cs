using AutoMapper;
using Domain.Custom_Exceptions;
using Domain.Entities;
using Domain.Interfaces;
using ServicesAbstractions;
using Shared.DTOs.Courses.Lessons;
using Shared.DTOs.Pagination;
using System; using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services
{
        public class LessonService(IUnitOfWork unitOfWork, IMapper mapper, ICacheService cacheService) : ILessonService
    {
        public async Task<PaginatedResultDto<LessonDto>> GetPaginatedLessonsAsync(int pageNumber, int pageSize)
        {
            var cacheKey = $"Lessons_Page_{pageNumber}_Size_{pageSize}";

                        var cachedLessons = await cacheService.GetCachedDataAsync<PaginatedResultDto<LessonDto>>(cacheKey);
            if (cachedLessons != null) return cachedLessons;

            var paginatedLessons = await unitOfWork.Repository<Lesson, int>().GetPaginatedAsync(pageNumber, pageSize);

            var response = new PaginatedResultDto<LessonDto>
            {
                Items = mapper.Map<List<LessonDto>>(paginatedLessons.Items),
                Metadata = new PaginationMetaDataDto
                {
                    CurrentPage = paginatedLessons.Metadata.CurrentPage,
                    TotalPages = paginatedLessons.Metadata.TotalPages,
                    PageSize = paginatedLessons.Metadata.PageSize,
                    TotalCount = paginatedLessons.Metadata.TotalCount
                }
            };

                        await cacheService.SetCachedDataAsync(cacheKey, response, TimeSpan.FromMinutes(30));

            return response;
        }

        public async Task<LessonDto> GetLessonByIdAsync(int id)
        {
            var cacheKey = $"Lesson_{id}";

                        var cachedLesson = await cacheService.GetCachedDataAsync<LessonDto>(cacheKey);
            if (cachedLesson != null) return cachedLesson;

            var lesson = await unitOfWork.Repository<Lesson, int>().GetByIdAsync(id);

            if (lesson == null)
                throw new NotFoundException($"Lesson with ID {id} was not found.");

            var response = mapper.Map<LessonDto>(lesson);

                        await cacheService.SetCachedDataAsync(cacheKey, response, TimeSpan.FromHours(1));

            return response;
        }

        public async Task<IEnumerable<LessonDto>> GetLessonsByModuleIdAsync(int moduleId)
        {
                        var cacheKey = $"Module_{moduleId}_Lessons";

            var cachedModuleLessons = await cacheService.GetCachedDataAsync<IEnumerable<LessonDto>>(cacheKey);
            if (cachedModuleLessons != null) return cachedModuleLessons;

            var moduleLessons = await unitOfWork.Repository<Lesson, int>()
                .FindAsync(l => l.ModuleId == moduleId);

            var orderedLessons = moduleLessons.OrderBy(l => l.Order);
            var response = mapper.Map<IEnumerable<LessonDto>>(orderedLessons);

                        await cacheService.SetCachedDataAsync(cacheKey, response, TimeSpan.FromHours(1));

            return response;
        }

        public async Task<LessonDto> CreateLessonAsync(CreateLessonDto createLessonDto)
        {
            var newLesson = mapper.Map<Lesson>(createLessonDto);

            await unitOfWork.Repository<Lesson, int>().AddAsync(newLesson);
            await unitOfWork.CompleteAsync();

                        await cacheService.RemoveCachedDataAsync($"Module_{newLesson.ModuleId}_Lessons");

            return mapper.Map<LessonDto>(newLesson);
        }

        public async Task UpdateLessonAsync(int id, CreateLessonDto updateLessonDto)
        {
            var lesson = await unitOfWork.Repository<Lesson, int>().GetByIdAsync(id);

            if (lesson == null)
                throw new NotFoundException($"Lesson with ID {id} was not found.");

            mapper.Map(updateLessonDto, lesson);
            unitOfWork.Repository<Lesson, int>().Update(lesson);
            await unitOfWork.CompleteAsync();

                        await cacheService.RemoveCachedDataAsync($"Lesson_{id}");
            await cacheService.RemoveCachedDataAsync($"Module_{lesson.ModuleId}_Lessons");
        }

        public async Task DeleteLessonAsync(int id)
        {
            var lesson = await unitOfWork.Repository<Lesson, int>().GetByIdAsync(id);

            if (lesson == null)
                throw new NotFoundException($"Lesson with ID {id} was not found.");

            unitOfWork.Repository<Lesson, int>().Delete(lesson);
            await unitOfWork.CompleteAsync();

                        await cacheService.RemoveCachedDataAsync($"Lesson_{id}");
            await cacheService.RemoveCachedDataAsync($"Module_{lesson.ModuleId}_Lessons");
        }
    }
}