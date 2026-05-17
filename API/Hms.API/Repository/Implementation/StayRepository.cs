using Hms.API.Data;
using Hms.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Hms.API.Repository;

public class StayRepository : IStayRepository
{
    private readonly MyAppDbContext _context;

    public StayRepository(MyAppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Stay>> GetAllAsync()
    {
        return await _context.Stays
            .Include(s => s.PatientNavigation)
            .Include(s => s.RoomNavigation)
            .ToListAsync();
    }

    public async Task<Stay?> GetByIdAsync(int stayId)
    {
        return await _context.Stays
            .Include(s => s.PatientNavigation)
            .Include(s => s.RoomNavigation)
            .FirstOrDefaultAsync(s => s.StayId == stayId);
    }

    public async Task<List<Stay>> GetByPatientAsync(int patientId)
    {
        return await _context.Stays
            .Include(s => s.PatientNavigation)
            .Include(s => s.RoomNavigation)
            .Where(s => s.Patient == patientId)
            .ToListAsync();
    }

    public async Task<List<Stay>> GetByRoomAsync(int roomId)
    {
        return await _context.Stays
            .Include(s => s.PatientNavigation)
            .Include(s => s.RoomNavigation)
            .Where(s => s.Room == roomId)
            .ToListAsync();
    }

    public async Task<List<Stay>> GetActiveStaysAsync()
    {
        var now = DateTime.Now;
        return await _context.Stays
            .Include(s => s.PatientNavigation)
            .Include(s => s.RoomNavigation)
            .Where(s => s.StayStart <= now && s.StayEnd >= now)
            .ToListAsync();
    }

    public async Task<List<Stay>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _context.Stays
            .Include(s => s.PatientNavigation)
            .Include(s => s.RoomNavigation)
            .Where(s => (s.StayStart >= startDate && s.StayStart <= endDate) ||
                        (s.StayEnd >= startDate && s.StayEnd <= endDate))
            .ToListAsync();
    }

    public async Task<Stay> CreateAsync(Stay stay)
    {
        _context.Stays.Add(stay);
        await _context.SaveChangesAsync();
        return stay;
    }

    public async Task<Stay> UpdateAsync(Stay stay)
    {
        _context.Stays.Update(stay);
        await _context.SaveChangesAsync();
        return stay;
    }

    public async Task<bool> DeleteAsync(int stayId)
    {
        var stay = await _context.Stays.FirstOrDefaultAsync(s => s.StayId == stayId);
        if (stay == null)
            return false;

        _context.Stays.Remove(stay);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistsAsync(int stayId)
    {
        return await _context.Stays.AnyAsync(s => s.StayId == stayId);
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<DTOs.StayCurrentRoomDto?> GetCurrentRoomAsync(int patientId, DateTime now)
    {
        return await _context.Stays
            .Include(s => s.RoomNavigation)
            .Where(s => s.Patient == patientId && s.StayStart <= now && s.StayEnd >= now)
            .OrderByDescending(s => s.StayStart)
            .Select(s => new DTOs.StayCurrentRoomDto
            {
                StayId = s.StayId,
                RoomNumber = s.RoomNavigation.RoomNumber,
                RoomType = s.RoomNavigation.RoomType,
                StayStart = s.StayStart
            })
            .FirstOrDefaultAsync();
    }
}
