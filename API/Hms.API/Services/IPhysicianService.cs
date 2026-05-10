using Hms.API.DTOs;
namespace Hms.API.Services
{
    public interface IPhysicianService
    {
        Task<IEnumerable<PhysicianDto>> GetAllPhysicians();
        Task<PhysicianDto?> GetPhysicianById(int id);
        Task<PhysicianDto> AddPhysician(CreatePhysicianDto dto);
        Task<PhysicianDto> UpdatePhysician(int id, UpdatePhysicianDto dto);
        Task<bool> DeletePhysician(int id);
        Task<IEnumerable<DepartmentDto>> GetDepartmentsByPhysician(int physicianId);
        Task<IEnumerable<ProcedureDto>> GetProceduresByPhysician(int physicianId);
        Task<bool> AssignDepartment(int physicianId, AssignDepartmentDto dto);
        Task<IEnumerable<AppointmentDto>> GetAppointmentsByPhysician(int physicianId);
        Task<IEnumerable<PatientDto>> GetPatientsByPhysician(int physicianId);

    }
}
