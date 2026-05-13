using AutoMapper;
using Hms.API.DTOs;
using Hms.API.Models;
using Hms.API.Repository;

namespace Hms.API.Services;

public class PrescriptionService : IPrescriptionService
{
    private readonly IPrescriptionRepository _prescriptionRepository;
    private readonly IMapper _mapper;

    public PrescriptionService(IPrescriptionRepository prescriptionRepository, IMapper mapper)
    {
        _prescriptionRepository = prescriptionRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<PrescriptionDTO>> GetAllPrescriptionsAsync()
    {
        var prescriptions = await _prescriptionRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<PrescriptionDTO>>(prescriptions);
    }

    public async Task<PrescriptionDetailDTO?> GetPrescriptionByIdAsync(int physician, int patient, int medication)
    {
        var prescription = await _prescriptionRepository.GetByIdAsync(physician, patient, medication);
        if (prescription == null)
            return null;

        var detail = _mapper.Map<PrescriptionDetailDTO>(prescription);
        detail.PhysicianName = prescription.PhysicianNavigation?.Name ?? "Unknown";
        detail.PatientName = prescription.PatientNavigation?.Name ?? "Unknown";
        detail.MedicationName = prescription.MedicationNavigation?.Name ?? "Unknown";
        return detail;
    }

    public async Task<IEnumerable<PrescriptionDTO>> GetPrescriptionsByPhysicianAsync(int physicianId)
    {
        var prescriptions = await _prescriptionRepository.GetByPhysicianAsync(physicianId);
        return _mapper.Map<IEnumerable<PrescriptionDTO>>(prescriptions);
    }

    public async Task<IEnumerable<PrescriptionDTO>> GetPrescriptionsByPatientAsync(int patientId)
    {
        var prescriptions = await _prescriptionRepository.GetByPatientAsync(patientId);
        return _mapper.Map<IEnumerable<PrescriptionDTO>>(prescriptions);
    }

    public async Task<IEnumerable<PrescriptionDTO>> GetPrescriptionsByMedicationAsync(int medicationId)
    {
        var prescriptions = await _prescriptionRepository.GetByMedicationAsync(medicationId);
        return _mapper.Map<IEnumerable<PrescriptionDTO>>(prescriptions);
    }

    public async Task<IEnumerable<PrescriptionDTO>> GetPrescriptionsByAppointmentAsync(int appointmentId)
    {
        var prescriptions = await _prescriptionRepository.GetByAppointmentAsync(appointmentId);
        return _mapper.Map<IEnumerable<PrescriptionDTO>>(prescriptions);
    }

    public async Task<IEnumerable<PrescriptionDTO>> GetPrescriptionsByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        var prescriptions = await _prescriptionRepository.GetAllAsync();
        var filtered = prescriptions.Where(p => p.Date >= startDate && p.Date <= endDate).ToList();
        return _mapper.Map<IEnumerable<PrescriptionDTO>>(filtered);
    }

    public async Task<IEnumerable<PrescriptionDTO>> GetRecentPrescriptionsAsync(int days)
    {
        var startDate = DateTime.Now.AddDays(-days);
        var prescriptions = await _prescriptionRepository.GetAllAsync();
        var filtered = prescriptions.Where(p => p.Date >= startDate).OrderByDescending(p => p.Date).ToList();
        return _mapper.Map<IEnumerable<PrescriptionDTO>>(filtered);
    }

    public async Task<PrescriptionDTO> CreatePrescriptionAsync(CreatePrescriptionDTO createPrescriptionDto)
    {
        var prescription = _mapper.Map<Prescribe>(createPrescriptionDto);
        var createdPrescription = await _prescriptionRepository.CreateAsync(prescription);
        return _mapper.Map<PrescriptionDTO>(createdPrescription);
    }

    public async Task<PrescriptionDTO> UpdatePrescriptionAsync(int physician, int patient, int medication, UpdatePrescriptionDTO updatePrescriptionDto)
    {
        var prescription = await _prescriptionRepository.GetByIdAsync(physician, patient, medication);
        if (prescription == null)
            throw new KeyNotFoundException($"Prescription record not found for Physician: {physician}, Patient: {patient}, Medication: {medication}");

        _mapper.Map(updatePrescriptionDto, prescription);
        var updatedPrescription = await _prescriptionRepository.UpdateAsync(prescription);
        return _mapper.Map<PrescriptionDTO>(updatedPrescription);
    }

    public async Task<bool> DeletePrescriptionAsync(int physician, int patient, int medication)
    {
        return await _prescriptionRepository.DeleteAsync(physician, patient, medication);
    }

    public async Task<bool> PrescriptionExistsAsync(int physician, int patient, int medication)
    {
        return await _prescriptionRepository.ExistsAsync(physician, patient, medication);
    }
}
