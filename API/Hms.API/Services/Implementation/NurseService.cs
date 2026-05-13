using Hms.API.DTOs;
using Hms.API.Exceptions;
using Hms.API.Models;
using Hms.API.Repository;

namespace Hms.API.Services;

public class NurseService : INurseService
{
    private readonly INurseRepository _repository;

    public NurseService(INurseRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<NurseDto>> GetAllAsync()
    {
        var nurses = await _repository.GetAllAsync();
        return nurses.Select(MapToDto).ToList();
    }

    public async Task<NurseDto?> GetByIdAsync(int id)
    {
        var nurse = await _repository.GetByIdAsync(id);
        return nurse == null ? null : MapToDto(nurse);
    }

    public async Task<NurseDto> AddAsync(NurseCreateDto dto)
    {
        try
        {
            var nurse = new Nurse
            {
                Name = dto.Name,
                Position = dto.Position,
                Registered = dto.Registered,
                Ssn = dto.Ssn
            };

            await _repository.AddAsync(nurse);
            return MapToDto(nurse);
        }
        catch (Exception ex)
        {
            throw new ValidationException($"Could not create nurse: {ex.Message}");
        }
    }

    public async Task UpdateAsync(int id, NurseUpdateDto dto)
    {
        try
        {
            var nurse = await _repository.GetByIdAsync(id);
            if (nurse == null)
            {
                throw new NotFoundException("Nurse not found.");
            }

            nurse.Name = dto.Name;
            nurse.Position = dto.Position;
            nurse.Registered = dto.Registered;

            await _repository.UpdateAsync(nurse);
        }
        catch (NotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new ValidationException($"Could not update nurse: {ex.Message}");
        }
    }

    public async Task DeleteAsync(int id)
    {
        try
        {
            var nurse = await _repository.GetByIdAsync(id);
            if (nurse == null)
            {
                throw new NotFoundException("Nurse not found.");
            }

            await _repository.DeleteAsync(nurse);
        }
        catch (NotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new ValidationException($"Could not delete nurse: {ex.Message}");
        }
    }

    private static NurseDto MapToDto(Nurse nurse)
    {
        return new NurseDto
        {
            EmployeeId = nurse.EmployeeId,
            Name = nurse.Name,
            Position = nurse.Position,
            Registered = nurse.Registered,
            Ssn = nurse.Ssn
        };
    }

}
