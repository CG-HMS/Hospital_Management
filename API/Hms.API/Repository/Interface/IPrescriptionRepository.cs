using Hms.API.Models;

namespace Hms.API.Repository;

public interface IPrescriptionRepository
{
    Task<List<Prescribe>> GetAllAsync();
    Task<Prescribe?> GetByIdAsync(int physician, int patient, int medication);
    Task<List<Prescribe>> GetByPhysicianAsync(int physicianId);
    Task<List<Prescribe>> GetByPatientAsync(int patientId);
    Task<List<Prescribe>> GetByMedicationAsync(int medicationId);
    Task<List<Prescribe>> GetByAppointmentAsync(int appointmentId);
    Task<Prescribe> CreateAsync(Prescribe prescription);
    Task<Prescribe> UpdateAsync(Prescribe prescription);
    Task<bool> DeleteAsync(int physician, int patient, int medication);
    Task<bool> ExistsAsync(int physician, int patient, int medication);
    Task SaveAsync();
}
