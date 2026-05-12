using Hms.API.Data;
using Hms.API.Models;
using Hms.API.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hms.API.Repository;

public class PatientRepository : IPatientRepository
{
    private readonly MyAppDbContext _context;

    public PatientRepository(MyAppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Patient>> GetAllAsync()
    {
        return await _context.Patients
            .Include(p => p.PcpNavigation)
            .ToListAsync();
    }
    public async Task<Patient?> GetByIdAsync(int ssn)
    {
        return await _context.Patients
            .Include(p => p.PcpNavigation)
            .FirstOrDefaultAsync(p => p.Ssn == ssn);
    }

    public async Task AddAsync(Patient patient)
    {
        await _context.Patients.AddAsync(patient);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Patient patient)
    {
        _context.Patients.Update(patient);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Patient patient)
    {
        _context.Patients.Remove(patient);
        await _context.SaveChangesAsync();
    }
    public async Task<bool> ExistsAsync(int ssn)
    {
        return await _context.Patients.AnyAsync(p => p.Ssn == ssn);
    }
}