using Hms.API.DTOs.Patient;

namespace Hms.API.Services.Interfaces;

public interface IPatientService
{
    Task<IEnumerable<PatientResponseDto>> GetAllPatientsAsync();

    Task<PatientResponseDto?> GetPatientByIdAsync(int ssn);

    Task<PatientResponseDto> CreatePatientAsync(PatientRequestDto dto);

    Task<bool> UpdatePatientAsync(int ssn, PatientRequestDto dto);

    Task<bool> DeletePatientAsync(int ssn);
}