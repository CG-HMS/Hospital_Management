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
    }
