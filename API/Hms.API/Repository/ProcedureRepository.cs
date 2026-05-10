using Hms.API.Data;
using Hms.API.DTOs;
using Hms.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Hms.API.Repository
{
    public class ProcedureRepository : IProcedureRepository
    {
        private readonly MyAppDbContext _context;

        public ProcedureRepository(MyAppDbContext context)
        {
            _context = context;
        }

        public async Task Add(Procedure procedure)
        {
            await _context.Procedures.AddAsync(procedure);
        }

        public void Delete(Procedure procedure)
        {
            _context.Procedures.Remove(procedure);
        }

        public async Task<IEnumerable<Procedure>> GetAll()
        {
            return await _context.Procedures.ToListAsync();
        }

        public async Task<Procedure?> GetByCode(int code)
        {
            return await _context.Procedures
                .FirstOrDefaultAsync(p => p.Code == code);
        }

        public async Task<IEnumerable<ProcedurePhysicianDto>> GetPhysiciansByProcedure(int code)
        {
            return await
        (from t in _context.TrainedIns

         join p in _context.Physicians
            on t.Physician equals p.EmployeeId

         where t.Treatment == code

         select new ProcedurePhysicianDto
         {
             EmployeeId = p.EmployeeId,
             Name = p.Name,
             Position = p.Position
         }).ToListAsync();
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}
