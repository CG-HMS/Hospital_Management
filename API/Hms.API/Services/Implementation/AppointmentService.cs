using Hms.API.DTOs;
using Hms.API.Exceptions;
using Hms.API.Models;
using Hms.API.Repository;
using Microsoft.Extensions.Logging;

namespace Hms.API.Services;

public class AppointmentService : IAppointmentService
{
    private readonly IAppointmentRepository _repository;
    private readonly ILogger<AppointmentService> _logger;

    public AppointmentService(IAppointmentRepository repository, ILogger<AppointmentService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<List<AppointmentDto>> GetAllAsync()
    {
        var appointments = await _repository.GetAllAsync();
        return appointments.Select(MapToDto).ToList();
    }

    public async Task<AppointmentDto?> GetByIdAsync(int id)
    {
        var appointment = await _repository.GetByIdAsync(id);
        return appointment == null ? null : MapToDto(appointment);
    }

    public async Task<AppointmentDto> AddAsync(AppointmentCreateDto dto)
    {
        try
        {
            var appointment = new Appointment
            {
                Patient = dto.Patient,
                PrepNurse = dto.PrepNurse,
                Physician = dto.Physician,
                Starto = dto.Starto,
                Endo = dto.Endo,
                ExaminationRoom = dto.ExaminationRoom
            };

            await _repository.AddAsync(appointment);
            return MapToDto(appointment);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Could not create appointment.");
            throw new ValidationException($"Could not create appointment: {ex.Message}");
        }
    }

    public async Task UpdateAsync(int id, AppointmentUpdateDto dto)
    {
        try
        {
            var appointment = await _repository.GetByIdAsync(id);
            if (appointment == null)
            {
                throw new NotFoundException("Appointment not found.");
            }

            appointment.Patient = dto.Patient;
            appointment.PrepNurse = dto.PrepNurse;
            appointment.Physician = dto.Physician;
            appointment.Starto = dto.Starto;
            appointment.Endo = dto.Endo;
            appointment.ExaminationRoom = dto.ExaminationRoom;

            await _repository.UpdateAsync(appointment);
        }
        catch (NotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Could not update appointment {AppointmentId}.", id);
            throw new ValidationException($"Could not update appointment: {ex.Message}");
        }
    }

    public async Task DeleteAsync(int id)
    {
        try
        {
            var appointment = await _repository.GetByIdAsync(id);
            if (appointment == null)
            {
                throw new NotFoundException("Appointment not found.");
            }

            await _repository.DeleteAsync(appointment);
        }
        catch (NotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Could not delete appointment {AppointmentId}.", id);
            throw new ValidationException($"Could not delete appointment: {ex.Message}");
        }
    }

    public async Task<List<AppointmentFilterDto>> GetAppointmentsFilteredAsync(DateTime? fromDate, DateTime? toDate, int? physicianId, int? patientId)
    {
        try
        {
            return await _repository.GetAppointmentsFilteredAsync(fromDate, toDate, physicianId, patientId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get filtered appointments.");
            throw new ValidationException("Could not load appointments.");
        }
    }

    public async Task<List<AppointmentFilterDto>> GetTodayAppointmentsAsync(DateTime? date)
    {
        try
        {
            var day = date ?? DateTime.Today;
            var start = day.Date;
            var end = day.Date.AddDays(1).AddTicks(-1);

            return await _repository.GetTodayAppointmentsAsync(start, end);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get today's appointments.");
            throw new ValidationException("Could not load today's appointments.");
        }
    }

    public async Task<List<AppointmentGroupDto>> GetAppointmentsGroupedByPhysicianAsync()
    {
        try
        {
            return await _repository.GetAppointmentsGroupedByPhysicianAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get grouped appointments.");
            throw new ValidationException("Could not load grouped appointments.");
        }
    }

    private static AppointmentDto MapToDto(Appointment appointment)
    {
        return new AppointmentDto
        {
            AppointmentId = appointment.AppointmentId,
            Patient = appointment.Patient,
            PrepNurse = appointment.PrepNurse,
            Physician = appointment.Physician,
            Starto = appointment.Starto,
            Endo = appointment.Endo,
            ExaminationRoom = appointment.ExaminationRoom
        };
    }

}
