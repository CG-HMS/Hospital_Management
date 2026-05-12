using Hms.API.DTOs.Medication;

namespace Hms.API.Services.Interfaces;

public interface IMedicationService
{
    Task<IEnumerable<MedicationResponseDto>> GetAllMedicationsAsync();

    Task<MedicationResponseDto?> GetMedicationByIdAsync(int code);

    Task<MedicationResponseDto> CreateMedicationAsync(MedicationRequestDto dto);

    Task<bool> UpdateMedicationAsync(int code, MedicationRequestDto dto);

    Task<bool> DeleteMedicationAsync(int code);
}