using Hms.API.DTOs;

namespace Hms.API.Services;

public interface INurseService
{
    Task<List<NurseDto>> GetAllAsync();
    Task<NurseDto?> GetByIdAsync(int id);
    Task<NurseDto> AddAsync(NurseCreateDto dto);
    Task UpdateAsync(int id, NurseUpdateDto dto);
    Task DeleteAsync(int id);
}
