using Hms.API.Data;
using Hms.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Hms.API.Repository;

public class AppointmentRepository : IAppointmentRepository
{
    private readonly MyAppDbContext _context;

    public AppointmentRepository(MyAppDbContext context)
    {
        _context = context;
    }

    public Task<List<Appointment>> GetAllAsync()
    {
        return _context.Appointments.AsNoTracking().ToListAsync();
    }

    public Task<Appointment?> GetByIdAsync(int id)
    {
        return _context.Appointments.FirstOrDefaultAsync(a => a.AppointmentId == id);
    }

    public async Task AddAsync(Appointment appointment)
    {
        _context.Appointments.Add(appointment);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Appointment appointment)
    {
        _context.Appointments.Update(appointment);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Appointment appointment)
    {
        _context.Appointments.Remove(appointment);
        await _context.SaveChangesAsync();
    }
}
