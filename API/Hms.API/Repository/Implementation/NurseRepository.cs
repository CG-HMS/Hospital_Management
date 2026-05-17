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

    public async Task<List<DTOs.NurseOnCallDto>> GetOnCallScheduleAsync(int nurseId, DateTime? fromDate, DateTime? toDate)
    {
        var query = _context.OnCalls.Where(o => o.Nurse == nurseId);

        if (fromDate.HasValue)
        {
            query = query.Where(o => o.OnCallStart >= fromDate.Value);
        }

        if (toDate.HasValue)
        {
            query = query.Where(o => o.OnCallStart <= toDate.Value);
        }

        return await query
            .OrderBy(o => o.OnCallStart)
            .Select(o => new DTOs.NurseOnCallDto
            {
                BlockFloor = o.BlockFloor,
                BlockCode = o.BlockCode,
                OnCallStart = o.OnCallStart,
                OnCallEnd = o.OnCallEnd
            })
            .ToListAsync();
    }

    public async Task<List<DTOs.NurseTrainedProcedureDto>> GetTrainedProceduresAsync(int nurseId)
    {
        return await _context.Undergoes
            .Where(u => u.AssistingNurse == nurseId)
            .Include(u => u.ProceduresNavigation)
            .GroupBy(u => new { u.Procedures, u.ProceduresNavigation.Name })
            .Select(g => new DTOs.NurseTrainedProcedureDto
            {
                ProcedureCode = g.Key.Procedures,
                ProcedureName = g.Key.Name,
                CertificationDate = g.Max(x => x.DateUndergoes),
                CertificationExpires = g.Max(x => x.DateUndergoes)
            })
            .ToListAsync();
    }
}
