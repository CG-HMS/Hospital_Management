using Hms.API.DTOs;

namespace Hms.API.Services
{
    public interface IProcedureService
    {
        Task<IEnumerable<ProcedureDto>> GetAllProcedures();
        Task<ProcedureDto?> GetProcedureByCode(int code);

        Task<ProcedureDto> AddProcedure(ProcedureWriteDto dto);

        Task<ProcedureDto?> UpdateProcedure(int code, ProcedureWriteDto dto);

        Task DeleteProcedure(int code);
        Task<IEnumerable<ProcedurePhysicianDto>> GetPhysiciansByProcedure(int code);
        Task<IEnumerable<StayDto>> GetStaysByProcedure(int code);
        Task<IEnumerable<ProcedureDto>> SearchProcedures(string name);
        Task<IEnumerable<ProcedureDto>> GetProceduresByCostRange(float min, float max);
    }

}
