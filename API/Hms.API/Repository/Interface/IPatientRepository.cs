using Hms.API.Models;

namespace Hms.API.Repository.Interfaces;

public interface IPatientRepository
{
    Task<IEnumerable<Patient>> GetAllAsync();

    Task<Patient?> GetByIdAsync(int ssn);

    Task AddAsync(Patient patient);

    Task UpdateAsync(Patient patient);

    Task DeleteAsync(Patient patient);

    Task<bool> ExistsAsync(int ssn);

    Task<IEnumerable<DTOs.Patient.PatientAppointmentDto>> GetAppointmentsByPatientAsync(int ssn);
    Task<IEnumerable<DTOs.Patient.PatientMedicationDto>> GetMedicationsByPatientAsync(int ssn);
    Task<IEnumerable<DTOs.Patient.PatientStayHistoryDto>> GetStayHistoryByPatientAsync(int ssn);
    Task<IEnumerable<DTOs.Patient.PatientProcedureDto>> GetProceduresByPatientAsync(int ssn);
    Task<DTOs.Patient.PatientDashboardDto> GetPatientDashboardAsync(int ssn);
}