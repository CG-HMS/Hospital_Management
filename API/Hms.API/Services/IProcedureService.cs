using Hms.API.DTOs;

namespace Hms.API.Services
{
    public interface IProcedureService
    {
        Task<IEnumerable<ProcedureDto>> GetAllProcedures();
        Task<ProcedureDto?> GetProcedureByCode(int code);

        Task<ProcedureDto> AddProcedure(CreateProcedureDto dto);

        Task<ProcedureDto?> UpdateProcedure(
            int code,
            UpdateProcedureDto dto);

        Task<bool> DeleteProcedure(int code);
        Task<IEnumerable<ProcedurePhysicianDto>>
            GetPhysiciansByProcedure(int code);
    }
}
