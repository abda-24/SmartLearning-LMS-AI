using Shared.DTOs.Courses.Moduels;

namespace ServicesAbstractions
{
    public interface IModuleService
    {
        Task<ModuleDto> CreateModuleAsync(CreateModuleDto createModuleDto);
        Task<ModuleDto> GetModuleByIdAsync(int id);
        Task<IEnumerable<ModuleDto>> GetModulesByCourseIdAsync(int courseId);
        Task UpdateModuleAsync(int id, CreateModuleDto updateModuleDto);
        Task DeleteModuleAsync(int id);

    }
}
