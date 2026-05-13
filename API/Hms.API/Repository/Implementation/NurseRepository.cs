using Hms.API.Data;
using Hms.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Hms.API.Repository;

public class NurseRepository : INurseRepository
{
    private readonly MyAppDbContext _context;

    public NurseRepository(MyAppDbContext context)
    {
        _context = context;
    }

    public Task<List<Nurse>> GetAllAsync()
    {
        return _context.Nurses.AsNoTracking().ToListAsync();
    }

    public Task<Nurse?> GetByIdAsync(int id)
    {
        return _context.Nurses.FirstOrDefaultAsync(n => n.EmployeeId == id);
    }

    public async Task AddAsync(Nurse nurse)
    {
        _context.Nurses.Add(nurse);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Nurse nurse)
    {
        _context.Nurses.Update(nurse);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Nurse nurse)
    {
        _context.Nurses.Remove(nurse);
        await _context.SaveChangesAsync();
    }
}
