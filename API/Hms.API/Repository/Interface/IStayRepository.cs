using Hms.API.Models;

namespace Hms.API.Repository;

public interface IStayRepository
{
    Task<List<Stay>> GetAllAsync();
    Task<Stay?> GetByIdAsync(int stayId);
    Task<List<Stay>> GetByPatientAsync(int patientId);
    Task<List<Stay>> GetByRoomAsync(int roomId);
    Task<List<Stay>> GetActiveStaysAsync();
    Task<List<Stay>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<Stay> CreateAsync(Stay stay);
    Task<Stay> UpdateAsync(Stay stay);
    Task<bool> DeleteAsync(int stayId);
    Task<bool> ExistsAsync(int stayId);
    Task SaveAsync();
    Task<DTOs.StayCurrentRoomDto?> GetCurrentRoomAsync(int patientId, DateTime now);
}
