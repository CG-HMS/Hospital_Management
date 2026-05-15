using Hms.API.Models;

namespace Hms.API.Repository.Interfaces;

    public interface IMedicationRepository
    {
        Task<IEnumerable<Medication>> GetAllAsync();

        Task<Medication?> GetByIdAsync(int code);

        Task AddAsync(Medication medication);

        Task UpdateAsync(Medication medication);

        Task DeleteAsync(Medication medication);

        Task<bool> ExistsAsync(int code);

        Task<List<DTOs.Medication.MedicationUsageDto>> GetTopMedicationsAsync(int take);
        Task<List<DTOs.Medication.MedicationPatientDto>> GetMedicationPatientsAsync(int code);
        Task<int> GetPrescriptionCountAsync(int code);
    }
