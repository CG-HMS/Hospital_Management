using Hms.API.DTOs;

namespace Hms.API.Services;

public interface INurseService
{
    Task<List<NurseDto>> GetAllAsync();
    Task<NurseDto?> GetByIdAsync(int id);
    Task<NurseDto> AddAsync(NurseCreateDto dto);
    Task UpdateAsync(int id, NurseUpdateDto dto);
    Task DeleteAsync(int id);
    Task<List<NurseOnCallDto>> GetOnCallScheduleAsync(int id, DateTime? fromDate, DateTime? toDate);
    Task<List<NurseTrainedProcedureDto>> GetTrainedProceduresAsync(int id);
}
