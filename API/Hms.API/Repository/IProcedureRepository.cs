using Hms.API.DTOs;
using Hms.API.Models;
namespace Hms.API.Repository
{
    public interface IProcedureRepository
    {
        Task<IEnumerable<Procedure>> GetAll();
        Task<Procedure?> GetByCode(int code);

        Task Add(Procedure procedure);

        void Delete(Procedure procedure);

        Task Save();

        Task<IEnumerable<ProcedurePhysicianDto>> GetPhysiciansByProcedure(int code);
    }
}
