using AutoMapper;
using Domain.Custom_Exceptions;
using Domain.Entities;
using Domain.Interfaces;
using ServicesAbstractions;
using Shared.DTOs.Courses.Moduels;
using Shared.DTOs.Pagination;

namespace Application.Services
{
    public class ModuleService(IUnitOfWork unitOfWork, IMapper mapper) : IModuleService
    {
        
            public async Task<PaginatedResultDto<ModuleDto>> GetPaginatedModulesAsync(int pageNumber, int pageSize)
            {
                var paginatedModules = await unitOfWork.Repository<Modules, int>().GetPaginatedAsync(pageNumber, pageSize);

                return new PaginatedResultDto<ModuleDto>
                {
                    Items = mapper.Map<List<ModuleDto>>(paginatedModules.Items),
                    Metadata = new PaginationMetaDataDto
                    {
                        CurrentPage = paginatedModules.Metadata.CurrentPage,
                        TotalPages = paginatedModules.Metadata.TotalPages,
                        PageSize = paginatedModules.Metadata.PageSize,
                        TotalCount = paginatedModules.Metadata.TotalCount
                    }
                };
            }

            public async Task<ModuleDto> CreateModuleAsync(CreateModuleDto createModuleDto)
        {
            var newModule = mapper.Map<Modules>(createModuleDto);

            await unitOfWork.Repository<Modules, int>().AddAsync(newModule);
            await unitOfWork.CompleteAsync();

            return mapper.Map<ModuleDto>(newModule);
        }

        public async Task<ModuleDto> GetModuleByIdAsync(int id)
        {
            var module = await unitOfWork.Repository<Modules, int>().GetByIdAsync(id);

            if (module == null)
                throw new NotFoundException($"Module with ID {id} was not found.");

            return mapper.Map<ModuleDto>(module);
        }

        public async Task<IEnumerable<ModuleDto>> GetModulesByCourseIdAsync(int courseId)
        {
            var courseModules = await unitOfWork.Repository<Modules, int>()
                .FindAsync(m => m.CourseId == courseId);

            var orderedModules = courseModules.OrderBy(m => m.Order);

            return mapper.Map<IEnumerable<ModuleDto>>(orderedModules);
        }

        public async Task UpdateModuleAsync(int id, CreateModuleDto updateModuleDto)
        {
            var module = await unitOfWork.Repository<Modules, int>().GetByIdAsync(id);

            if (module == null)
                throw new NotFoundException($"Module with ID {id} was not found.");

            mapper.Map(updateModuleDto, module);
            unitOfWork.Repository<Modules, int>().Update(module);
            await unitOfWork.CompleteAsync();
        }

        public async Task DeleteModuleAsync(int id)
        {
            var module = await unitOfWork.Repository<Modules, int>().GetByIdAsync(id);

            if (module == null)
                throw new NotFoundException($"Module with ID {id} was not found.");

            unitOfWork.Repository<Modules, int>().Delete(module);
            await unitOfWork.CompleteAsync();
        }
    }
}