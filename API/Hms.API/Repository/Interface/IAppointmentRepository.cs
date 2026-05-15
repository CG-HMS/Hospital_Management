using Hms.API.Models;

namespace Hms.API.Repository;

public interface IAppointmentRepository
{
    Task<List<Appointment>> GetAllAsync();
   
    Task<Appointment?> GetByIdAsync(int id);
    Task AddAsync(Appointment appointment);
    Task UpdateAsync(Appointment appointment);
    Task DeleteAsync(Appointment appointment);

    Task<List<DTOs.AppointmentFilterDto>> GetAppointmentsFilteredAsync(DateTime? fromDate, DateTime? toDate, int? physicianId, int? patientId);
    Task<List<DTOs.AppointmentFilterDto>> GetTodayAppointmentsAsync(DateTime todayStart, DateTime todayEnd);
    Task<List<DTOs.AppointmentGroupDto>> GetAppointmentsGroupedByPhysicianAsync();
}
