using Hms.API.DTOs.Medication;

namespace Hms.API.Services.Interfaces;

    public interface IMedicationService
    {
        Task<IEnumerable<MedicationResponseDto>> GetAllAsync();

        Task<MedicationResponseDto?> GetByIdAsync(int code);

        Task<MedicationResponseDto> CreateAsync(CreateMedicationDto dto);

        Task<bool> UpdateAsync(int code, UpdateMedicationDto dto);

        Task<bool> DeleteAsync(int code);
    }
