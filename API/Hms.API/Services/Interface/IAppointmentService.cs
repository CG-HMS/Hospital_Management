using Hms.API.DTOs;

namespace Hms.API.Services;

public interface IAppointmentService
{
    Task<List<AppointmentDto>> GetAllAsync();
    Task<AppointmentDto?> GetByIdAsync(int id);
    Task<AppointmentDto> AddAsync(AppointmentCreateDto dto);
    Task UpdateAsync(int id, AppointmentUpdateDto dto);
    Task DeleteAsync(int id);
    Task<List<AppointmentFilterDto>> GetAppointmentsFilteredAsync(DateTime? fromDate, DateTime? toDate, int? physicianId, int? patientId);
    Task<List<AppointmentFilterDto>> GetTodayAppointmentsAsync(DateTime? date);
    Task<List<AppointmentGroupDto>> GetAppointmentsGroupedByPhysicianAsync();
}
