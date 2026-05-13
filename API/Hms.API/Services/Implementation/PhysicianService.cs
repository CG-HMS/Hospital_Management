using System.Linq;
using Hms.API.Data;
using Hms.API.DTOs;
using Hms.API.Models;
using Hms.API.Repository;
using Hms.API.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Hms.API.Services
{
    public class PhysicianService : IPhysicianService
    {
        private readonly IPhysicianRepository _repository;

        public PhysicianService(IPhysicianRepository repository)
        {
            _repository = repository;
        }

        public async Task<PhysicianDto> AddPhysician(PhysicianWriteDto dto)
        {
            var physicians = await _repository.GetAll();

            int nextId = physicians.Any()
                ? physicians.Max(p => p.EmployeeId) + 1
                : 1;

            var physician = new Physician
            {
                Name = dto.Name,
                Position = dto.Position,
                Ssn = dto.Ssn ?? 0,
                EmployeeId = nextId
            };

            await _repository.Add(physician);
            await _repository.Save();

            return MapToDto(physician);
        }

        public async Task<bool> AssignDepartment(int physicianId, AssignDepartmentDto dto)
        {
            var physicianExists = await _repository.PhysicianExists(physicianId);
            var departmentExists = await _repository.DepartmentExists(dto.DepartmentId);

            if (!physicianExists || !departmentExists)
            {
                throw new BadRequestException("Invalid physician or department");
            }

            var alreadyExists = await _repository.AffiliationExists(physicianId, dto.DepartmentId);

            if (alreadyExists)
            {
                throw new ConflictException("Physician already affiliated with department");
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

        public async Task DeletePhysician(int id)
        {
            var physician = await _repository.GetById(id);

            if (physician == null)
            {
                throw new NotFoundException("Physician not found");
            }

            _repository.Delete(physician);
            await _repository.Save();

            //return true;
        }

        public async Task<IEnumerable<PhysicianDto>> GetAllPhysicians()
        {
            var physicians = await _repository.GetAll();
            return physicians.Select(MapToDto);
        }

        public async Task<IEnumerable<AppointDto>> GetAppointmentsByPhysician(int physicianId)
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
                throw new NotFoundException("Physician not found");
            }

            return MapToDto(physician);
        }

        public async Task<IEnumerable<ProcedureDto>> GetProceduresByPhysician(int physicianId)
        {
            return await _repository.GetProceduresByPhysician(physicianId);
        }
        private static PhysicianDto MapToDto(Physician p)
        {
            return new PhysicianDto
            {
                EmployeeId = p.EmployeeId,
                Name = p.Name,
                Position = p.Position
            };
        }

        public async Task<PhysicianDto> UpdatePhysician(int id, PhysicianWriteDto dto)
        {
            var physician = await _repository.GetById(id);

            if (physician == null)
            {
                throw new NotFoundException(
                    "Physician not found");
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
            await _repository.Save( );

            return MapToDto(physician);
        }
    }
}
