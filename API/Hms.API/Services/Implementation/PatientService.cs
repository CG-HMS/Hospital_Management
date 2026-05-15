using Hms.API.DTOs.Patient;
using Hms.API.Exceptions;
using Hms.API.Models;
using Hms.API.Repository.Interfaces;
using Hms.API.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace Hms.API.Services;

public class PatientService : IPatientService
{
    private readonly IPatientRepository _repository;
    private readonly ILogger<PatientService> _logger;

    public PatientService(IPatientRepository repository, ILogger<PatientService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<IEnumerable<PatientResponseDto>> GetAllPatientsAsync()
    {
        var patients = await _repository.GetAllAsync();

        return patients.Select(MapToDto);
    }

    public async Task<PatientResponseDto> GetPatientByIdAsync(int ssn)
    {
        var patient = await _repository.GetByIdAsync(ssn);

        if (patient == null)
        {
            throw new NotFoundException("Patient", ssn);
        }

        return MapToDto(patient);
    }

    public async Task<PatientResponseDto> CreatePatientAsync(PatientRequestDto dto)
    {
        try
        {
            var existingPatient = await _repository.GetByIdAsync(dto.Ssn);

            if (existingPatient != null)
            {
                throw new ConflictException(
                    $"Patient with SSN {dto.Ssn} already exists"
                );
            }

            var patient = new Patient
            {
                Ssn = dto.Ssn,
                Name = dto.Name,
                Address = dto.Address,
                Phone = dto.Phone,
                InsuranceId = dto.InsuranceId,
                Pcp = dto.Pcp
            };

            await _repository.AddAsync(patient);

            return MapToDto(patient);
        }
        catch (AppException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create patient.");
            throw new ValidationException("Failed to create patient.");
        }
    }

    public async Task UpdatePatientAsync(int ssn, PatientRequestDto dto)
    {
        try
        {
            var patient = await _repository.GetByIdAsync(ssn);

            if (patient == null)
            {
                throw new NotFoundException("Patient", ssn);
            }

            patient.Name = dto.Name;
            patient.Address = dto.Address;
            patient.Phone = dto.Phone;
            patient.InsuranceId = dto.InsuranceId;
            patient.Pcp = dto.Pcp;

            await _repository.UpdateAsync(patient);
        }
        catch (AppException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update patient {PatientId}.", ssn);
            throw new ValidationException("Failed to update patient.");
        }
    }

    public async Task DeletePatientAsync(int ssn)
    {
        try
        {
            var patient = await _repository.GetByIdAsync(ssn);

            if (patient == null)
            {
                throw new NotFoundException("Patient", ssn);
            }

            await _repository.DeleteAsync(patient);
        }
        catch (AppException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete patient {PatientId}.", ssn);
            throw new ValidationException("Failed to delete patient.");
        }
    }

    public async Task<IEnumerable<PatientAppointmentDto>> GetAppointmentsByPatientAsync(int ssn)
    {
        try
        {
            await EnsurePatientExists(ssn);
            return await _repository.GetAppointmentsByPatientAsync(ssn);
        }
        catch (AppException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get appointments for patient {PatientId}.", ssn);
            throw new ValidationException("Failed to get patient appointments.");
        }
    }

    public async Task<IEnumerable<PatientMedicationDto>> GetMedicationsByPatientAsync(int ssn)
    {
        try
        {
            await EnsurePatientExists(ssn);
            return await _repository.GetMedicationsByPatientAsync(ssn);
        }
        catch (AppException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get medications for patient {PatientId}.", ssn);
            throw new ValidationException("Failed to get patient medications.");
        }
    }

    public async Task<IEnumerable<PatientStayHistoryDto>> GetStayHistoryByPatientAsync(int ssn)
    {
        try
        {
            await EnsurePatientExists(ssn);
            return await _repository.GetStayHistoryByPatientAsync(ssn);
        }
        catch (AppException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get stays for patient {PatientId}.", ssn);
            throw new ValidationException("Failed to get patient stays.");
        }
    }

    public async Task<IEnumerable<PatientProcedureDto>> GetProceduresByPatientAsync(int ssn)
    {
        try
        {
            await EnsurePatientExists(ssn);
            return await _repository.GetProceduresByPatientAsync(ssn);
        }
        catch (AppException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get procedures for patient {PatientId}.", ssn);
            throw new ValidationException("Failed to get patient procedures.");
        }
    }

    public async Task<PatientDashboardDto> GetPatientDashboardAsync(int ssn)
    {
        try
        {
            await EnsurePatientExists(ssn);
            return await _repository.GetPatientDashboardAsync(ssn);
        }
        catch (AppException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get dashboard for patient {PatientId}.", ssn);
            throw new ValidationException("Failed to get patient dashboard.");
        }
    }

    private async Task EnsurePatientExists(int ssn)
    {
        if (!await _repository.ExistsAsync(ssn))
        {
            throw new NotFoundException("Patient", ssn);
        }
    }

    private static PatientResponseDto MapToDto(Patient patient)
    {
        return new PatientResponseDto
        {
            Ssn = patient.Ssn,
            Name = patient.Name,
            Address = patient.Address,
            Phone = patient.Phone,
            InsuranceId = patient.InsuranceId,
            Pcp = patient.Pcp
        };
    }
}