using Hms.API.DTOs;
using Hms.API.DTOs.Physician;
using Hms.API.Models;
using Hms.API.Repository;
using Hms.API.Exceptions;
using Microsoft.Extensions.Logging;

namespace Hms.API.Services
{
    public class PhysicianService : IPhysicianService
    {
        private readonly IPhysicianRepository _repository;
        private readonly ILogger<PhysicianService> _logger;

        public PhysicianService(IPhysicianRepository repository, ILogger<PhysicianService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<PhysicianDto> AddPhysician(PhysicianWriteDto dto)
        {
            try
            {
                var physicians = await _repository.GetAll();

                int nextId = physicians.Any()
                    ? physicians.Max(p => p.EmployeeId) + 1
                    : 1;

                var physician = new Physician
                {
                    Name = dto.Name ?? string.Empty,
                    Position = dto.Position ?? string.Empty,
                    Ssn = dto.Ssn ?? 0,
                    EmployeeId = nextId
                };

                await _repository.Add(physician);
                await _repository.Save();

                return MapToDto(physician);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to add physician.");
                throw new ValidationException("Failed to add physician.");
            }
        }

        public async Task<bool> AssignDepartment(int physicianId, AssignDepartmentDto dto)
        {
            try
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
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to assign department {DepartmentId} to physician {PhysicianId}.", dto.DepartmentId, physicianId);
                throw new ValidationException("Failed to assign department.");
            }
        }

        public async Task DeletePhysician(int id)
        {
            try
            {
                var physician = await _repository.GetById(id);

                if (physician == null)
                {
                    throw new NotFoundException("Physician not found");
                }

                _repository.Delete(physician);
                await _repository.Save();
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete physician {PhysicianId}.", id);
                throw new ValidationException("Failed to delete physician.");
            }
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
            try
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
                await _repository.Save();

                return MapToDto(physician);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update physician {PhysicianId}.", id);
                throw new ValidationException("Failed to update physician.");
            }
        }

        public async Task<PhysicianAppointmentStatsDto> GetAppointmentStatsAsync(int physicianId)
        {
            try
            {
                await EnsurePhysicianExists(physicianId);
                return await _repository.GetAppointmentStatsAsync(physicianId);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get appointment stats for physician {PhysicianId}.", physicianId);
                throw new ValidationException("Failed to get appointment stats.");
            }
        }

        public async Task<IEnumerable<PhysicianUpcomingAppointmentDto>> GetUpcomingAppointmentsAsync(int physicianId, DateTime? fromDate)
        {
            try
            {
                await EnsurePhysicianExists(physicianId);
                var startDate = fromDate ?? DateTime.UtcNow;
                return await _repository.GetUpcomingAppointmentsAsync(physicianId, startDate);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get upcoming appointments for physician {PhysicianId}.", physicianId);
                throw new ValidationException("Failed to get upcoming appointments.");
            }
        }

        public async Task<IEnumerable<PhysicianTopDto>> GetTopPhysiciansByAppointmentsAsync(int take)
        {
            try
            {
                var limit = take <= 0 ? 5 : take;
                return await _repository.GetTopPhysiciansByAppointmentsAsync(limit);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get top physicians.");
                throw new ValidationException("Failed to get top physicians.");
            }
        }

        private async Task EnsurePhysicianExists(int physicianId)
        {
            if (!await _repository.PhysicianExists(physicianId))
            {
                throw new NotFoundException("Physician not found");
            }
        }
    }
}
