using Hms.API.Models;

namespace Hms.API.Repository;

public interface INurseRepository
{
    Task<List<Nurse>> GetAllAsync();
    Task<Nurse?> GetByIdAsync(int id);
    Task AddAsync(Nurse nurse);
    Task UpdateAsync(Nurse nurse);
    Task DeleteAsync(Nurse nurse);

    Task<List<DTOs.NurseOnCallDto>> GetOnCallScheduleAsync(int nurseId, DateTime? fromDate, DateTime? toDate);
    Task<List<DTOs.NurseTrainedProcedureDto>> GetTrainedProceduresAsync(int nurseId);
}
