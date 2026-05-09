using AutoMapper;
using Hms.API.DTOs;
using Hms.API.Models;
using Hms.API.Repository;

namespace Hms.API.Services;

public class AppointmentService : IAppointmentService
{
    private readonly IAppointmentRepository _repository;
    private readonly IMapper _mapper;

    public AppointmentService(IAppointmentRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<List<AppointmentDto>> GetAllAsync()
    {
        var appointments = await _repository.GetAllAsync();
        return _mapper.Map<List<AppointmentDto>>(appointments);
    }

    public async Task<AppointmentDto?> GetByIdAsync(int id)
    {
        var appointment = await _repository.GetByIdAsync(id);
        return appointment == null ? null : _mapper.Map<AppointmentDto>(appointment);
    }

    public async Task<AppointmentDto> AddAsync(AppointmentDto dto)
    {
        var appointment = _mapper.Map<Appointment>(dto);
        await _repository.AddAsync(appointment);
        return _mapper.Map<AppointmentDto>(appointment);
    }

    public async Task<bool> UpdateAsync(int id, AppointmentDto dto)
    {
        var appointment = await _repository.GetByIdAsync(id);
        if (appointment == null)
        {
            return false;
        }

        _mapper.Map(dto, appointment);

        await _repository.UpdateAsync(appointment);
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var appointment = await _repository.GetByIdAsync(id);
        if (appointment == null)
        {
            return false;
        }

        await _repository.DeleteAsync(appointment);
        return true;
    }

}
