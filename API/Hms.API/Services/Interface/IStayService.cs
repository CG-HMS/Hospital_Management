using Hms.API.DTOs;

namespace Hms.API.Services;

public interface IStayService
{
    Task<IEnumerable<StayDTO>> GetAllStaysAsync();
    Task<StayDetailDTO?> GetStayByIdAsync(int stayId);
    Task<IEnumerable<StayDTO>> GetStaysByPatientAsync(int patientId);
    Task<IEnumerable<StayDTO>> GetStaysByRoomAsync(int roomId);
    Task<IEnumerable<StayDTO>> GetActiveStaysAsync();
    Task<IEnumerable<StayDTO>> GetStaysByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<StayDTO> CreateStayAsync(CreateStayDTO createStayDto);
    Task<StayDTO> UpdateStayAsync(int stayId, UpdateStayDTO updateStayDto);
    Task<bool> DeleteStayAsync(int stayId);
    Task<bool> StayExistsAsync(int stayId);
    Task<StayCurrentRoomDto?> GetCurrentRoomAsync(int patientId);
}
