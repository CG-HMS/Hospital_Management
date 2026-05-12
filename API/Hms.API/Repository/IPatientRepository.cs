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
}