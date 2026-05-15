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

    public async Task<List<DTOs.AppointmentFilterDto>> GetAppointmentsFilteredAsync(DateTime? fromDate, DateTime? toDate, int? physicianId, int? patientId)
    {
        var query = _context.Appointments
            .Include(a => a.PatientNavigation)
            .Include(a => a.PhysicianNavigation)
            .AsQueryable();

        if (fromDate.HasValue)
        {
            query = query.Where(a => a.Starto >= fromDate.Value);
        }

        if (toDate.HasValue)
        {
            query = query.Where(a => a.Starto <= toDate.Value);
        }

        if (physicianId.HasValue)
        {
            query = query.Where(a => a.Physician == physicianId.Value);
        }

        if (patientId.HasValue)
        {
            query = query.Where(a => a.Patient == patientId.Value);
        }

        return await query
            .OrderByDescending(a => a.Starto)
            .Select(a => new DTOs.AppointmentFilterDto
            {
                AppointmentId = a.AppointmentId,
                PatientId = a.Patient,
                PatientName = a.PatientNavigation.Name,
                PhysicianId = a.Physician,
                PhysicianName = a.PhysicianNavigation.Name,
                Starto = a.Starto,
                Endo = a.Endo,
                ExaminationRoom = a.ExaminationRoom
            })
            .ToListAsync();
    }

    public async Task<List<DTOs.AppointmentFilterDto>> GetTodayAppointmentsAsync(DateTime todayStart, DateTime todayEnd)
    {
        return await _context.Appointments
            .Where(a => a.Starto >= todayStart && a.Starto <= todayEnd)
            .Include(a => a.PatientNavigation)
            .Include(a => a.PhysicianNavigation)
            .OrderBy(a => a.Starto)
            .Select(a => new DTOs.AppointmentFilterDto
            {
                AppointmentId = a.AppointmentId,
                PatientId = a.Patient,
                PatientName = a.PatientNavigation.Name,
                PhysicianId = a.Physician,
                PhysicianName = a.PhysicianNavigation.Name,
                Starto = a.Starto,
                Endo = a.Endo,
                ExaminationRoom = a.ExaminationRoom
            })
            .ToListAsync();
    }

    public async Task<List<DTOs.AppointmentGroupDto>> GetAppointmentsGroupedByPhysicianAsync()
    {
        return await _context.Appointments
            .GroupBy(a => a.Physician)
            .Join(_context.Physicians,
                group => group.Key,
                physician => physician.EmployeeId,
                (group, physician) => new DTOs.AppointmentGroupDto
                {
                    PhysicianId = physician.EmployeeId,
                    PhysicianName = physician.Name,
                    AppointmentCount = group.Count()
                })
            .OrderByDescending(g => g.AppointmentCount)
            .ToListAsync();
    }
}
