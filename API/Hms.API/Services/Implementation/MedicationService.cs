using Hms.API.DTOs.Medication;
using Hms.API.Exceptions;
using Hms.API.Models;
using Hms.API.Repository.Interfaces;
using Hms.API.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace Hms.API.Services;

public class MedicationService : IMedicationService
{
    private readonly IMedicationRepository _repository;
    private readonly ILogger<MedicationService> _logger;

    public MedicationService(
        IMedicationRepository repository,
        ILogger<MedicationService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<IEnumerable<MedicationResponseDto>> GetAllMedicationsAsync()
    {
        var medications = await _repository.GetAllAsync();

        return medications.Select(MapToDto);
    }

    public async Task<MedicationResponseDto> GetMedicationByIdAsync(int code)
    {
        var medication = await _repository.GetByIdAsync(code);

        if (medication == null)
            throw new NotFoundException("Medication", code);

        return MapToDto(medication);
    }

    public async Task<MedicationResponseDto> CreateMedicationAsync(MedicationRequestDto dto)
    {
        try
        {
            var medication = new Medication
            {
                Code = dto.Code,
                Name = dto.Name,
                Brand = dto.Brand,
                Description = dto.Description
            };

            await _repository.AddAsync(medication);

            return MapToDto(medication);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create medication.");
            throw new ValidationException("Failed to create medication.");
        }
    }

    public async Task UpdateMedicationAsync(int code, MedicationRequestDto dto)
    {
        try
        {
            var medication = await _repository.GetByIdAsync(code);

            if (medication == null)
                throw new NotFoundException("Medication", code);

            medication.Name = dto.Name;
            medication.Brand = dto.Brand;
            medication.Description = dto.Description;

            await _repository.UpdateAsync(medication);
        }
        catch (AppException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update medication {Code}.", code);
            throw new ValidationException("Failed to update medication.");
        }
    }

    public async Task DeleteMedicationAsync(int code)
    {
        try
        {
            var medication = await _repository.GetByIdAsync(code);

            if (medication == null)
                throw new NotFoundException("Medication", code);

            await _repository.DeleteAsync(medication);
        }
        catch (AppException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete medication {Code}.", code);
            throw new ValidationException("Failed to delete medication.");
        }
    }

    public async Task<int> GetPrescriptionCountAsync(int code)
    {
        try
        {
            if (!await _repository.ExistsAsync(code))
            {
                throw new NotFoundException("Medication", code);
            }

            return await _repository.GetPrescriptionCountAsync(code);
        }
        catch (AppException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get prescription count for medication {Code}.", code);
            throw new ValidationException("Failed to get prescription count.");
        }
    }

    public async Task<IEnumerable<MedicationPatientDto>> GetMedicationPatientsAsync(int code)
    {
        try
        {
            if (!await _repository.ExistsAsync(code))
            {
                throw new NotFoundException("Medication", code);
            }

            return await _repository.GetMedicationPatientsAsync(code);
        }
        catch (AppException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get medication patients for {Code}.", code);
            throw new ValidationException("Failed to get medication patients.");
        }
    }

    public async Task<IEnumerable<MedicationUsageDto>> GetTopMedicationsAsync(int take)
    {
        try
        {
            var limit = take <= 0 ? 5 : take;
            return await _repository.GetTopMedicationsAsync(limit);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get top medications.");
            throw new ValidationException("Failed to get top medications.");
        }
    }

    private static MedicationResponseDto MapToDto(Medication medication)
    {
        return new MedicationResponseDto
        {
            Code = medication.Code,
            Name = medication.Name,
            Brand = medication.Brand,
            Description = medication.Description
        };
    }
}