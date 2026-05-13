using AutoMapper;
using Hms.API.DTOs;
using Hms.API.Models;
using Hms.API.Repository;

namespace Hms.API.Services;

public class StayService : IStayService
{
    private readonly IStayRepository _stayRepository;
    private readonly IMapper _mapper;

    public StayService(IStayRepository stayRepository, IMapper mapper)
    {
        _stayRepository = stayRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<StayDTO>> GetAllStaysAsync()
    {
        var stays = await _stayRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<StayDTO>>(stays);
    }

    public async Task<StayDetailDTO?> GetStayByIdAsync(int stayId)
    {
        var stay = await _stayRepository.GetByIdAsync(stayId);
        if (stay == null)
            return null;

        return _mapper.Map<StayDetailDTO>(stay);
    }

    public async Task<IEnumerable<StayDTO>> GetStaysByPatientAsync(int patientId)
    {
        var stays = await _stayRepository.GetByPatientAsync(patientId);
        return _mapper.Map<IEnumerable<StayDTO>>(stays);
    }

    public async Task<IEnumerable<StayDTO>> GetStaysByRoomAsync(int roomId)
    {
        var stays = await _stayRepository.GetByRoomAsync(roomId);
        return _mapper.Map<IEnumerable<StayDTO>>(stays);
    }

    public async Task<IEnumerable<StayDTO>> GetActiveStaysAsync()
    {
        var stays = await _stayRepository.GetActiveStaysAsync();
        return _mapper.Map<IEnumerable<StayDTO>>(stays);
    }

    public async Task<IEnumerable<StayDTO>> GetStaysByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        var stays = await _stayRepository.GetByDateRangeAsync(startDate, endDate);
        return _mapper.Map<IEnumerable<StayDTO>>(stays);
    }

    public async Task<StayDTO> CreateStayAsync(CreateStayDTO createStayDto)
    {
        var stay = _mapper.Map<Stay>(createStayDto);
        var createdStay = await _stayRepository.CreateAsync(stay);
        return _mapper.Map<StayDTO>(createdStay);
    }

    public async Task<StayDTO> UpdateStayAsync(int stayId, UpdateStayDTO updateStayDto)
    {
        var stay = await _stayRepository.GetByIdAsync(stayId);
        if (stay == null)
            throw new KeyNotFoundException($"Stay record not found for ID: {stayId}");

        _mapper.Map(updateStayDto, stay);
        var updatedStay = await _stayRepository.UpdateAsync(stay);
        return _mapper.Map<StayDTO>(updatedStay);
    }

    public async Task<bool> DeleteStayAsync(int stayId)
    {
        return await _stayRepository.DeleteAsync(stayId);
    }

    public async Task<bool> StayExistsAsync(int stayId)
    {
        return await _stayRepository.ExistsAsync(stayId);
    }
}
