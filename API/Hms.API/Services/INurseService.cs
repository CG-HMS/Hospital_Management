using Hms.API.DTOs;

namespace Hms.API.Services;

public interface INurseService
{
    Task<List<NurseDto>> GetAllAsync();
    Task<NurseDto?> GetByIdAsync(int id);
    Task<NurseDto> AddAsync(NurseDto dto);
    Task<bool> UpdateAsync(int id, NurseDto dto);
    Task<bool> DeleteAsync(int id);
}
