using Hms.API.DTOs;
using Hms.API.Models;

namespace Hms.API.Repository
{
    public interface IPhysicianRepository
    {
        Task<IEnumerable<Physician>> GetAll();

        Task<Physician?> GetById(int id);

        Task Add(Physician physician);

        void Delete(Physician physician);

        Task Save();

        Task<IEnumerable<DepartmentDto>> GetDepartmentsByPhysician(int physicianId);

        Task<IEnumerable<ProcedureDto>> GetProceduresByPhysician(int physicianId);

        Task<IEnumerable<AppointmentDto>> GetAppointmentsByPhysician(int physicianId);

        Task<IEnumerable<PatientDto>> GetPatientsByPhysician(int physicianId);

        Task<bool> PhysicianExists(int physicianId);

        Task<bool> DepartmentExists(int departmentId);

        Task AssignDepartment(AffiliatedWith affiliatedWith);

        Task<bool> AffiliationExists(
    int physicianId,
    int departmentId);
    }
}
