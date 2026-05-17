using Hms.API.DTOs.Patient;

namespace Hms.API.Services.Interfaces;

public interface IPatientService
{
    Task<IEnumerable<PatientResponseDto>> GetAllPatientsAsync();

    Task<PatientResponseDto> GetPatientByIdAsync(int ssn);

    Task<PatientResponseDto> CreatePatientAsync(PatientRequestDto dto);

    Task UpdatePatientAsync(int ssn, PatientRequestDto dto);

    Task DeletePatientAsync(int ssn);

    Task<IEnumerable<PatientAppointmentDto>> GetAppointmentsByPatientAsync(int ssn);
    Task<IEnumerable<PatientMedicationDto>> GetMedicationsByPatientAsync(int ssn);
    Task<IEnumerable<PatientStayHistoryDto>> GetStayHistoryByPatientAsync(int ssn);
    Task<IEnumerable<PatientProcedureDto>> GetProceduresByPatientAsync(int ssn);
    Task<PatientDashboardDto> GetPatientDashboardAsync(int ssn);
}