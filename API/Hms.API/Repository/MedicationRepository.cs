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
    }
