using Hms.API.Data;
using Hms.API.DTOs;
using Hms.API.Models;
using Hms.API.Repository;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace Hms.API.Services
{
    public class PhysicianService : IPhysicianService
    {
        private readonly IPhysicianRepository _repository;
        private readonly IMapper _mapper;

        public PhysicianService(
            IPhysicianRepository repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<PhysicianDto> AddPhysician(CreatePhysicianDto dto)
        {
            var physicians = await _repository.GetAll();

            int nextId = physicians.Any()
                ? physicians.Max(p => p.EmployeeId) + 1
                : 1;

            var physician = _mapper.Map<Physician>(dto);

            physician.EmployeeId = nextId;

            await _repository.Add(physician);

            await _repository.Save();

            return _mapper.Map<PhysicianDto>(physician);
        }

        public async Task<bool> AssignDepartment(int physicianId, AssignDepartmentDto dto)
        {
            var physicianExists =
                await _repository.PhysicianExists(physicianId);

            var departmentExists =
                await _repository.DepartmentExists(dto.DepartmentId);

            if (!physicianExists || !departmentExists)
            {
                return false;
            }
            var alreadyExists = await _repository.AffiliationExists(
            physicianId,
            dto.DepartmentId);

            if (alreadyExists)
            {
                return false;
            }

            var affiliation = new AffiliatedWith
            {
                Physician = physicianId,
                Department = dto.DepartmentId
            };

            await _repository.AssignDepartment(affiliation);

            await _repository.Save();

            return true;
        }

        public async Task<bool> DeletePhysician(int id)
        {
            var physician = await _repository.GetById(id);

            if (physician == null)
            {
                return false;
            }

            _repository.Delete(physician);

            await _repository.Save();

            return true;
        }

        public async Task<IEnumerable<PhysicianDto>> GetAllPhysicians()
        {
            var physicians = await _repository.GetAll();

            return _mapper.Map<IEnumerable<PhysicianDto>>(physicians);
        }

        public async Task<IEnumerable<AppointmentDto>> GetAppointmentsByPhysician(int physicianId)
        {
            return await _repository.GetAppointmentsByPhysician(physicianId);
        }

        public async Task<IEnumerable<DepartmentDto>> GetDepartmentsByPhysician(int physicianId)
        {
            return await _repository.GetDepartmentsByPhysician(physicianId);
        }

        public async Task<IEnumerable<PatientDto>> GetPatientsByPhysician(int physicianId)
        {
            return await _repository.GetPatientsByPhysician(physicianId);
        }

        public async Task<PhysicianDto?> GetPhysicianById(int id)
        {
            var physician = await _repository.GetById(id);

            if (physician == null)
            {
                return null;
            }

            return _mapper.Map<PhysicianDto>(physician);
        }

        public async Task<IEnumerable<ProcedureDto>> GetProceduresByPhysician(int physicianId)
        {
            return await _repository.GetProceduresByPhysician(physicianId);
        }

        public async Task<PhysicianDto> UpdatePhysician(int id, UpdatePhysicianDto dto)
        {
            var physician = await _repository.GetById(id);

            if (physician == null)
            {
                return null;
            }
            if (dto.Name != null)
            {
                physician.Name = dto.Name;
            }

            if (dto.Position != null)
            {
                physician.Position = dto.Position;
            }

            if (dto.Ssn.HasValue)
            {
                physician.Ssn = dto.Ssn.Value;
            }

            //_mapper.Map(dto, physician);

            await _repository.Save();

            return _mapper.Map<PhysicianDto>(physician);
        }
    }
}
