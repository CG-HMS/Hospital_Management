using Hms.API.DTOs.Patient;

namespace Hms.API.Services.Interfaces;

public interface IPatientService
{
    Task<IEnumerable<PatientResponseDto>> GetAllPatientsAsync();

    Task<PatientResponseDto?> GetPatientByIdAsync(int ssn);

    Task<PatientResponseDto> CreatePatientAsync(CreatePatientDto dto);

    Task<bool> UpdatePatientAsync(int ssn, UpdatePatientDto dto);

    Task<bool> DeletePatientAsync(int ssn);
}