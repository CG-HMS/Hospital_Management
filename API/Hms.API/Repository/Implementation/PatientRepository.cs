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

    public async Task<IEnumerable<DTOs.Patient.PatientAppointmentDto>> GetAppointmentsByPatientAsync(int ssn)
    {
        return await _context.Appointments
            .Where(a => a.Patient == ssn)
            .Include(a => a.PhysicianNavigation)
            .Select(a => new DTOs.Patient.PatientAppointmentDto
            {
                AppointmentId = a.AppointmentId,
                Starto = a.Starto,
                Endo = a.Endo,
                ExaminationRoom = a.ExaminationRoom,
                PhysicianId = a.Physician,
                PhysicianName = a.PhysicianNavigation.Name
            })
            .ToListAsync();
    }

    public async Task<IEnumerable<DTOs.Patient.PatientMedicationDto>> GetMedicationsByPatientAsync(int ssn)
    {
        return await _context.Prescribes
            .Where(p => p.Patient == ssn)
            .Include(p => p.MedicationNavigation)
            .Include(p => p.PhysicianNavigation)
            .Select(p => new DTOs.Patient.PatientMedicationDto
            {
                MedicationCode = p.Medication,
                MedicationName = p.MedicationNavigation.Name,
                Dose = p.Dose,
                Date = p.Date,
                PhysicianId = p.Physician,
                PhysicianName = p.PhysicianNavigation.Name
            })
            .ToListAsync();
    }

    public async Task<IEnumerable<DTOs.Patient.PatientStayHistoryDto>> GetStayHistoryByPatientAsync(int ssn)
    {
        return await _context.Stays
            .Where(s => s.Patient == ssn)
            .Include(s => s.RoomNavigation)
            .Select(s => new DTOs.Patient.PatientStayHistoryDto
            {
                StayId = s.StayId,
                StayStart = s.StayStart,
                StayEnd = s.StayEnd,
                RoomNumber = s.RoomNavigation.RoomNumber,
                RoomType = s.RoomNavigation.RoomType,
                BlockFloor = s.RoomNavigation.BlockFloor,
                BlockCode = s.RoomNavigation.BlockCode
            })
            .ToListAsync();
    }

    public async Task<IEnumerable<DTOs.Patient.PatientProcedureDto>> GetProceduresByPatientAsync(int ssn)
    {
        return await _context.Undergoes
            .Where(u => u.Patient == ssn)
            .Include(u => u.ProceduresNavigation)
            .Include(u => u.PhysicianNavigation)
            .Include(u => u.AssistingNurseNavigation)
            .Select(u => new DTOs.Patient.PatientProcedureDto
            {
                ProcedureCode = u.Procedures,
                ProcedureName = u.ProceduresNavigation.Name,
                DateUndergoes = u.DateUndergoes,
                PhysicianId = u.Physician,
                PhysicianName = u.PhysicianNavigation.Name,
                AssistingNurseId = u.AssistingNurse,
                AssistingNurseName = u.AssistingNurseNavigation != null ? u.AssistingNurseNavigation.Name : null
            })
            .ToListAsync();
    }

    public async Task<DTOs.Patient.PatientDashboardDto> GetPatientDashboardAsync(int ssn)
    {
        var appointmentQuery = _context.Appointments.Where(a => a.Patient == ssn);
        var medicationQuery = _context.Prescribes.Where(p => p.Patient == ssn);
        var stayQuery = _context.Stays.Where(s => s.Patient == ssn);
        var procedureQuery = _context.Undergoes.Where(u => u.Patient == ssn);

        return new DTOs.Patient.PatientDashboardDto
        {
            AppointmentCount = await appointmentQuery.CountAsync(),
            MedicationCount = await medicationQuery.CountAsync(),
            StayCount = await stayQuery.CountAsync(),
            ProcedureCount = await procedureQuery.CountAsync(),
            LastAppointmentDate = await appointmentQuery
                .OrderByDescending(a => a.Starto)
                .Select(a => (DateTime?)a.Starto)
                .FirstOrDefaultAsync()
        };
    }
}