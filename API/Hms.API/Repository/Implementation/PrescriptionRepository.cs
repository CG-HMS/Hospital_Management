using Hms.API.Data;
using Hms.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Hms.API.Repository;

public class PrescriptionRepository : IPrescriptionRepository
{
    private readonly MyAppDbContext _context;

    public PrescriptionRepository(MyAppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Prescribe>> GetAllAsync()
    {
        return await _context.Prescribes
            .Include(p => p.PhysicianNavigation)
            .Include(p => p.PatientNavigation)
            .Include(p => p.MedicationNavigation)
            .Include(p => p.AppointmentNavigation)
            .ToListAsync();
    }

    public async Task<Prescribe?> GetByIdAsync(int physician, int patient, int medication)
    {
        return await _context.Prescribes
            .Include(p => p.PhysicianNavigation)
            .Include(p => p.PatientNavigation)
            .Include(p => p.MedicationNavigation)
            .Include(p => p.AppointmentNavigation)
            .FirstOrDefaultAsync(p => p.Physician == physician && p.Patient == patient && p.Medication == medication);
    }

    public async Task<List<Prescribe>> GetByPhysicianAsync(int physicianId)
    {
        return await _context.Prescribes
            .Include(p => p.PhysicianNavigation)
            .Include(p => p.PatientNavigation)
            .Include(p => p.MedicationNavigation)
            .Where(p => p.Physician == physicianId)
            .ToListAsync();
    }

    public async Task<List<Prescribe>> GetByPatientAsync(int patientId)
    {
        return await _context.Prescribes
            .Include(p => p.PhysicianNavigation)
            .Include(p => p.PatientNavigation)
            .Include(p => p.MedicationNavigation)
            .Where(p => p.Patient == patientId)
            .ToListAsync();
    }

    public async Task<List<Prescribe>> GetByMedicationAsync(int medicationId)
    {
        return await _context.Prescribes
            .Include(p => p.PhysicianNavigation)
            .Include(p => p.PatientNavigation)
            .Include(p => p.MedicationNavigation)
            .Where(p => p.Medication == medicationId)
            .ToListAsync();
    }

    public async Task<List<Prescribe>> GetByAppointmentAsync(int appointmentId)
    {
        return await _context.Prescribes
            .Include(p => p.PhysicianNavigation)
            .Include(p => p.PatientNavigation)
            .Include(p => p.MedicationNavigation)
            .Where(p => p.Appointment == appointmentId)
            .ToListAsync();
    }

    public async Task<Prescribe> CreateAsync(Prescribe prescription)
    {
        _context.Prescribes.Add(prescription);
        await _context.SaveChangesAsync();
        return prescription;
    }

    public async Task<Prescribe> UpdateAsync(Prescribe prescription)
    {
        _context.Prescribes.Update(prescription);
        await _context.SaveChangesAsync();
        return prescription;
    }

    public async Task<bool> DeleteAsync(int physician, int patient, int medication)
    {
        var prescription = await _context.Prescribes
            .FirstOrDefaultAsync(p => p.Physician == physician && p.Patient == patient && p.Medication == medication);

        if (prescription == null)
            return false;

        _context.Prescribes.Remove(prescription);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistsAsync(int physician, int patient, int medication)
    {
        return await _context.Prescribes
            .AnyAsync(p => p.Physician == physician && p.Patient == patient && p.Medication == medication);
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}
