using Hms.API.DTOs;
namespace Hms.API.Services
{
    public interface IPhysicianService
    {
        Task<IEnumerable<PhysicianDto>> GetAllPhysicians();
        Task<PhysicianDto?> GetPhysicianById(int id);
        Task<PhysicianDto> AddPhysician(PhysicianWriteDto dto);
        Task<PhysicianDto> UpdatePhysician(int id, PhysicianWriteDto dto);
        Task DeletePhysician(int id);
        Task<IEnumerable<DepartmentDto>> GetDepartmentsByPhysician(int physicianId);
        Task<IEnumerable<ProcedureDto>> GetProceduresByPhysician(int physicianId);
        Task<bool> AssignDepartment(int physicianId, AssignDepartmentDto dto);
        Task<IEnumerable<AppointDto>> GetAppointmentsByPhysician(int physicianId);
        Task<IEnumerable<PatientDto>> GetPatientsByPhysician(int physicianId);

        Task<DTOs.Physician.PhysicianAppointmentStatsDto> GetAppointmentStatsAsync(int physicianId);
        Task<IEnumerable<DTOs.Physician.PhysicianUpcomingAppointmentDto>> GetUpcomingAppointmentsAsync(int physicianId, DateTime? fromDate);
        Task<IEnumerable<DTOs.Physician.PhysicianTopDto>> GetTopPhysiciansByAppointmentsAsync(int take);

    }
}
