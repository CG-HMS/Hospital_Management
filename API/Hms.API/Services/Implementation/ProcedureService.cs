using System.Linq;
using Hms.API.DTOs;
using Hms.API.Exceptions;
using Hms.API.Models;
using Hms.API.Repository;

namespace Hms.API.Services
{
    public class ProcedureService : IProcedureService
    {
        private readonly IProcedureRepository _repository;

        public ProcedureService(IProcedureRepository repository)
        {
            _repository = repository;
        }

        public async Task<ProcedureDto> AddProcedure(ProcedureWriteDto dto)
        {
            var procedures = await _repository.GetAll();

            int nextCode = procedures.Any()
                ? procedures.Max(p => p.Code) + 1
                : 1;

            var procedure = new Procedure
            {
                Name = dto.Name,
                Cost = (float)dto.Cost,
                Code = nextCode
            };

            await _repository.Add(procedure);
            await _repository.Save();

            return MapToDto(procedure);
        }

        public async Task DeleteProcedure(int code)
        {
            var procedure = await _repository.GetByCode(code);

            if (procedure == null)
            {
                throw new NotFoundException("Procedure not found");
            }

            _repository.Delete(procedure);
            await _repository.Save();

            
        }

        public async Task<IEnumerable<ProcedureDto>> GetAllProcedures()
        {
            var procedures = await _repository.GetAll();
            return procedures.Select(MapToDto);
        }

        public async Task<IEnumerable<ProcedurePhysicianDto>> GetPhysiciansByProcedure(int code)
        {
            return await _repository.GetPhysiciansByProcedure(code);
        }

        public async Task<ProcedureDto?> GetProcedureByCode(int code)
        {
            var procedure = await _repository.GetByCode(code);

            if (procedure == null)
            {
                throw new NotFoundException("Procedure not found");
            }

            return MapToDto(procedure);
        }

        public async Task<IEnumerable<ProcedureDto>> GetProceduresByCostRange(float min, float max)
        {
            var procedures = await _repository.GetProceduresByCostRange(min, max);
            return await _repository.GetProceduresByCostRange(min, max);
        }

        public async Task<IEnumerable<StayDto>> GetStaysByProcedure(int code)
        {
            return await _repository.GetStaysByProcedure(code);
        }

        public async Task<IEnumerable<ProcedureDto>> SearchProcedures(string name)
        {
            var procedures = await _repository.SearchProcedures(name);
            return await _repository.SearchProcedures(name);
        }

        public async Task<ProcedureDto?> UpdateProcedure(int code, ProcedureWriteDto dto)
        {
            var procedure = await _repository.GetByCode(code);

            if (procedure == null)
            {
                throw new NotFoundException("Procedure not found");
            }

            if (dto.Name != null)
            {
                procedure.Name = dto.Name;
            }

            if (dto.Cost.HasValue)
            {
                procedure.Cost = (float)dto.Cost.Value;
            }

            await _repository.Save();

            return MapToDto(procedure);
        }

        // Manual mapping helper
        private static ProcedureDto MapToDto(Procedure p)
        {
            return new ProcedureDto
            {
                Code = p.Code,
                Name = p.Name,
                Cost = (decimal)p.Cost
            };
        }
    }
}
