using Hms.API.DTOs;

namespace Hms.API.Services;

public interface IAppointmentService
{
    Task<List<AppointmentDto>> GetAllAsync();
    Task<AppointmentDto?> GetByIdAsync(int id);
    Task<AppointmentDto> AddAsync(AppointmentDto dto);
    Task<bool> UpdateAsync(int id, AppointmentDto dto);
    Task<bool> DeleteAsync(int id);
}
