using Hms.API.Data;
using Hms.API.DTOs;
using Hms.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Hms.API.Repository
{
    public class PhysicianRepository : IPhysicianRepository
    {
        private readonly MyAppDbContext _context;

        public PhysicianRepository(MyAppDbContext context)
        {
            _context = context;
        }
        public async Task Add(Physician physician)
        {
            await _context.Physicians.AddAsync(physician);
        }

        public void Delete(Physician physician)
        {
            _context.Physicians.Remove(physician);
        }

        public async Task<IEnumerable<Physician>> GetAll()
        {
            return await _context.Physicians.ToListAsync();
        }

        public async Task<Physician?> GetById(int id)
        {
            return await _context.Physicians.FirstOrDefaultAsync(p => p.EmployeeId == id);
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        public async Task AssignDepartment(AffiliatedWith affiliatedWith)
        {
            await _context.AffiliatedWiths.AddAsync(affiliatedWith);
        }

        public async Task<bool> DepartmentExists(int departmentId)
        {
            return await _context.Departments
                .AnyAsync(d => d.DepartmentId == departmentId);
        }

        public async Task<IEnumerable<AppointmentDto>> GetAppointmentsByPhysician(int physicianId)
        {
            return await
                (from a in _context.Appointments
                 join p in _context.Patients
                    on a.Patient equals p.Ssn

                 where a.Physician == physicianId

                 select new AppointmentDto
                 {
                     AppointmentId = a.AppointmentId,
                     PatientName = p.Name,
                     StartDateTime = a.Starto,
                     ExaminationRoom = a.ExaminationRoom
                 }).ToListAsync();
        }

        public async Task<IEnumerable<DepartmentDto>> GetDepartmentsByPhysician(int physicianId)
        {
            return await
                (from aw in _context.AffiliatedWiths
                 join d in _context.Departments
                    on aw.Department equals d.DepartmentId

                 where aw.Physician == physicianId

                 select new DepartmentDto
                 {
                     DepartmentId = d.DepartmentId,
                     Name = d.Name
                 }).ToListAsync();
        }

        public async Task<IEnumerable<PatientDto>> GetPatientsByPhysician(int physicianId)
        {
            return await _context.Patients
                .Where(p => p.Pcp == physicianId)

                .Select(p => new PatientDto
                {
                    Ssn = p.Ssn,
                    Name = p.Name,
                    Address = p.Address,
                    Phone = p.Phone
                }).ToListAsync();
        }

        public async Task<IEnumerable<ProcedureDto>> GetProceduresByPhysician(int physicianId) => await
                (from t in _context.TrainedIns
                 join p in _context.Procedures
                    on t.Treatment equals p.Code

                 where t.Physician == physicianId

                 select new ProcedureDto
                 {
                     Code = p.Code,
                     Name = p.Name,
                     Cost = (decimal)p.Cost
                 }).ToListAsync();

        public async Task<bool> PhysicianExists(int physicianId)
        {
            return await _context.Physicians
                .AnyAsync(p => p.EmployeeId == physicianId);
        }
    }
}
