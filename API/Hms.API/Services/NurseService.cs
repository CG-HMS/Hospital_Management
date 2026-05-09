using AutoMapper;
using Hms.API.DTOs;
using Hms.API.Models;
using Hms.API.Repository;

namespace Hms.API.Services;

public class NurseService : INurseService
{
    private readonly INurseRepository _repository;
    private readonly IMapper _mapper;

    public NurseService(INurseRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<List<NurseDto>> GetAllAsync()
    {
        var nurses = await _repository.GetAllAsync();
        return _mapper.Map<List<NurseDto>>(nurses);
    }

    public async Task<NurseDto?> GetByIdAsync(int id)
    {
        var nurse = await _repository.GetByIdAsync(id);
        return nurse == null ? null : _mapper.Map<NurseDto>(nurse);
    }

    public async Task<NurseDto> AddAsync(NurseDto dto)
    {
        var nurse = _mapper.Map<Nurse>(dto);
        await _repository.AddAsync(nurse);
        return _mapper.Map<NurseDto>(nurse);
    }

    public async Task<bool> UpdateAsync(int id, NurseDto dto)
    {
        var nurse = await _repository.GetByIdAsync(id);
        if (nurse == null)
        {
            return false;
        }

        _mapper.Map(dto, nurse);

        await _repository.UpdateAsync(nurse);
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var nurse = await _repository.GetByIdAsync(id);
        if (nurse == null)
        {
            return false;
        }

        await _repository.DeleteAsync(nurse);
        return true;
    }

}
