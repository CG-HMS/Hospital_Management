using AutoMapper;
using Hms.API.DTOs;
using Hms.API.Exceptions;
using Hms.API.Models;
using Hms.API.Repository;
using Microsoft.Extensions.Logging;

namespace Hms.API.Services;

public class StayService : IStayService
{
    private readonly IStayRepository _stayRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<StayService> _logger;

    public StayService(IStayRepository stayRepository, IMapper mapper, ILogger<StayService> logger)
    {
        _stayRepository = stayRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<StayDTO>> GetAllStaysAsync()
    {
        try
        {
            var stays = await _stayRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<StayDTO>>(stays);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get stays.");
            throw new ValidationException("Failed to get stays.");
        }
    }

    public async Task<StayDetailDTO?> GetStayByIdAsync(int stayId)
    {
        try
        {
            var stay = await _stayRepository.GetByIdAsync(stayId);
            if (stay == null)
            {
                return null;
            }

            return _mapper.Map<StayDetailDTO>(stay);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get stay {StayId}.", stayId);
            throw new ValidationException("Failed to get stay.");
        }
    }

    public async Task<IEnumerable<StayDTO>> GetStaysByPatientAsync(int patientId)
    {
        try
        {
            var stays = await _stayRepository.GetByPatientAsync(patientId);
            return _mapper.Map<IEnumerable<StayDTO>>(stays);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get stays for patient {PatientId}.", patientId);
            throw new ValidationException("Failed to get stays for patient.");
        }
    }

    public async Task<IEnumerable<StayDTO>> GetStaysByRoomAsync(int roomId)
    {
        try
        {
            var stays = await _stayRepository.GetByRoomAsync(roomId);
            return _mapper.Map<IEnumerable<StayDTO>>(stays);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get stays for room {RoomId}.", roomId);
            throw new ValidationException("Failed to get stays for room.");
        }
    }

    public async Task<IEnumerable<StayDTO>> GetActiveStaysAsync()
    {
        try
        {
            var stays = await _stayRepository.GetActiveStaysAsync();
            return _mapper.Map<IEnumerable<StayDTO>>(stays);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get active stays.");
            throw new ValidationException("Failed to get active stays.");
        }
    }

    public async Task<IEnumerable<StayDTO>> GetStaysByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        try
        {
            var stays = await _stayRepository.GetByDateRangeAsync(startDate, endDate);
            return _mapper.Map<IEnumerable<StayDTO>>(stays);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get stays by date range.");
            throw new ValidationException("Failed to get stays by date range.");
        }
    }

    public async Task<StayDTO> CreateStayAsync(CreateStayDTO createStayDto)
    {
        try
        {
            var stay = _mapper.Map<Stay>(createStayDto);
            var createdStay = await _stayRepository.CreateAsync(stay);
            return _mapper.Map<StayDTO>(createdStay);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create stay.");
            throw new ValidationException("Failed to create stay.");
        }
    }

    public async Task<StayDTO> UpdateStayAsync(int stayId, UpdateStayDTO updateStayDto)
    {
        try
        {
            var stay = await _stayRepository.GetByIdAsync(stayId);
            if (stay == null)
                throw new NotFoundException("Stay", stayId);

            _mapper.Map(updateStayDto, stay);
            var updatedStay = await _stayRepository.UpdateAsync(stay);
            return _mapper.Map<StayDTO>(updatedStay);
        }
        catch (AppException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update stay {StayId}.", stayId);
            throw new ValidationException("Failed to update stay.");
        }
    }

    public async Task<bool> DeleteStayAsync(int stayId)
    {
        try
        {
            return await _stayRepository.DeleteAsync(stayId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete stay {StayId}.", stayId);
            throw new ValidationException("Failed to delete stay.");
        }
    }

    public async Task<bool> StayExistsAsync(int stayId)
    {
        return await _stayRepository.ExistsAsync(stayId);
    }

    public async Task<StayCurrentRoomDto?> GetCurrentRoomAsync(int patientId)
    {
        try
        {
            var room = await _stayRepository.GetCurrentRoomAsync(patientId, DateTime.Now);
            if (room == null)
            {
                throw new NotFoundException("Active stay not found for patient.");
            }

            return room;
        }
        catch (AppException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get current room for patient {PatientId}.", patientId);
            throw new ValidationException("Failed to get current room.");
        }
    }
}
