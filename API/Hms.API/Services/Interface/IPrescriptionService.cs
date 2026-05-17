using Hms.API.DTOs;

namespace Hms.API.Services;

public interface IPrescriptionService
{
    Task<IEnumerable<PrescriptionDTO>> GetAllPrescriptionsAsync();
    Task<PrescriptionDetailDTO?> GetPrescriptionByIdAsync(int physician, int patient, int medication);
    Task<IEnumerable<PrescriptionDTO>> GetPrescriptionsByPhysicianAsync(int physicianId);
    Task<IEnumerable<PrescriptionDTO>> GetPrescriptionsByPatientAsync(int patientId);
    Task<IEnumerable<PrescriptionDTO>> GetPrescriptionsByMedicationAsync(int medicationId);
    Task<IEnumerable<PrescriptionDTO>> GetPrescriptionsByAppointmentAsync(int appointmentId);
    Task<IEnumerable<PrescriptionDTO>> GetPrescriptionsByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<IEnumerable<PrescriptionDTO>> GetRecentPrescriptionsAsync(int days);
    Task<PrescriptionDTO> CreatePrescriptionAsync(CreatePrescriptionDTO createPrescriptionDto);
    Task<PrescriptionDTO> UpdatePrescriptionAsync(int physician, int patient, int medication, UpdatePrescriptionDTO updatePrescriptionDto);
    Task<bool> DeletePrescriptionAsync(int physician, int patient, int medication);
    Task<bool> PrescriptionExistsAsync(int physician, int patient, int medication);
}
