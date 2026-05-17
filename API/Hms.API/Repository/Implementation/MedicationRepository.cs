using Hms.API.Repository.Interfaces;
using Hms.API.Models;
using Hms.API.Data;
using Microsoft.EntityFrameworkCore;
namespace Hms.API.Repository;

    public class MedicationRepository : IMedicationRepository
    {
        private readonly MyAppDbContext _context;

        public MedicationRepository(MyAppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Medication>> GetAllAsync()
        {
            return await _context.Medications.ToListAsync();
        }

        public async Task<Medication?> GetByIdAsync(int code)
        {
            return await _context.Medications
                .FirstOrDefaultAsync(m => m.Code == code);
        }
        public async Task AddAsync(Medication medication)
        {
            await _context.Medications.AddAsync(medication);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Medication medication)
        {
            _context.Medications.Update(medication);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Medication medication)
        {
            _context.Medications.Remove(medication);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int code)
        {
            return await _context.Medications
                .AnyAsync(m => m.Code == code);
        }

        public async Task<List<DTOs.Medication.MedicationUsageDto>> GetTopMedicationsAsync(int take)
        {
            return await _context.Prescribes
                .GroupBy(p => p.Medication)
                .OrderByDescending(g => g.Count())
                .Take(take)
                .Join(_context.Medications,
                    g => g.Key,
                    m => m.Code,
                    (g, m) => new DTOs.Medication.MedicationUsageDto
                    {
                        MedicationCode = m.Code,
                        MedicationName = m.Name,
                        PrescriptionCount = g.Count()
                    })
                .ToListAsync();
        }

        public async Task<List<DTOs.Medication.MedicationPatientDto>> GetMedicationPatientsAsync(int code)
        {
            return await _context.Prescribes
                .Where(p => p.Medication == code)
                .Include(p => p.PatientNavigation)
                .Include(p => p.PhysicianNavigation)
                .OrderByDescending(p => p.Date)
                .Select(p => new DTOs.Medication.MedicationPatientDto
                {
                    PatientId = p.Patient,
                    PatientName = p.PatientNavigation.Name,
                    Dose = p.Dose,
                    Date = p.Date,
                    PhysicianId = p.Physician,
                    PhysicianName = p.PhysicianNavigation.Name
                })
                .ToListAsync();
        }

        public async Task<int> GetPrescriptionCountAsync(int code)
        {
            return await _context.Prescribes.CountAsync(p => p.Medication == code);
        }
    }
