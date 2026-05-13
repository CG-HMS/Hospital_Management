using Hms.API.DTOs;
using Hms.API.Exceptions;
using Hms.API.Models;
using Hms.API.Repository;

namespace Hms.API.Services;

public class AppointmentService : IAppointmentService
{
    private readonly IAppointmentRepository _repository;

    public AppointmentService(IAppointmentRepository repository)
    {
        _repository = repository;
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
            throw new ValidationException($"Could not delete appointment: {ex.Message}");
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
