using AutoMapper;
using Hms.API.DTOs;
using Hms.API.Models;
using Hms.API.Repository;

namespace Hms.API.Services
{
    public class ProcedureService : IProcedureService
    {
        private readonly IProcedureRepository _repository;
        private readonly IMapper _mapper;

        public ProcedureService(
            IProcedureRepository repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ProcedureDto> AddProcedure(CreateProcedureDto dto)
        {
            var procedures =
        await _repository.GetAll();

            int nextCode = procedures.Any()
                ? procedures.Max(p => p.Code) + 1
                : 1;

            var procedure =
                _mapper.Map<Procedure>(dto);

            procedure.Code = nextCode;

            await _repository.Add(procedure);

            await _repository.Save();

            return _mapper.Map<ProcedureDto>(procedure);
        }

        public async Task<bool> DeleteProcedure(int code)
        {
            var procedure = await _repository.GetByCode(code);

            if (procedure == null)
            {
                return false;
            }

            _repository.Delete(procedure);

            await _repository.Save();

            return true;
        }

        public async Task<IEnumerable<ProcedureDto>> GetAllProcedures()
        {
            var procedures = await _repository.GetAll();

            return _mapper.Map<IEnumerable<ProcedureDto>>(procedures);
        }

        public async Task<IEnumerable<ProcedurePhysicianDto>> GetPhysiciansByProcedure(int code)
        {
            return await _repository
        .GetPhysiciansByProcedure(code);
        }

        public async Task<ProcedureDto?> GetProcedureByCode(int code)
        {
            var procedure = await _repository.GetByCode(code);

            if (procedure == null)
            {
                return null;
            }

            return _mapper.Map<ProcedureDto>(procedure);
        }

        public async Task<IEnumerable<ProcedureDto>> GetProceduresByCostRange(float min, float max)
        {
            var procedures = await _repository.GetProceduresByCostRange(min, max);

            return _mapper.Map<IEnumerable<ProcedureDto>>(procedures);
        }

        public async Task<IEnumerable<StayDto>> GetStaysByProcedure(int code)
        {
            return await _repository.GetStaysByProcedure(code);
        }

        public async Task<IEnumerable<ProcedureDto>> SearchProcedures(string name)
        {
            var procedures = await _repository.SearchProcedures(name);

            return _mapper.Map<IEnumerable<ProcedureDto>>(procedures);
        }

        public async Task<ProcedureDto?> UpdateProcedure(int code, UpdateProcedureDto dto)
        {
            var procedure = await _repository.GetByCode(code);

            if (procedure == null)
            {
                return null;
            }

            _mapper.Map(dto, procedure);

            await _repository.Save();

            return _mapper.Map<ProcedureDto>(procedure);
        }
    }
}
