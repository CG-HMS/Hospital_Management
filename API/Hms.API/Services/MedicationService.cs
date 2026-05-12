using AutoMapper;
using Hms.API.DTOs.Medication;
using Hms.API.Models;
using Hms.API.Repository.Interfaces;
using Hms.API.Services.Interfaces;

namespace Hms.API.Services;

public class MedicationService : IMedicationService
{
    private readonly IMedicationRepository _repository;
    private readonly IMapper _mapper;

    public MedicationService(IMedicationRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<MedicationResponseDto>> GetAllMedicationsAsync()
    {
        var medications = await _repository.GetAllAsync();

        return _mapper.Map<IEnumerable<MedicationResponseDto>>(medications);
    }
    public async Task<MedicationResponseDto?> GetMedicationByIdAsync(int code)
    {
        var medication = await _repository.GetByIdAsync(code);

        if (medication == null)
            return null;

        return _mapper.Map<MedicationResponseDto>(medication);
    }

    public async Task<MedicationResponseDto> CreateMedicationAsync(MedicationRequestDto dto)
    {
        var medication = _mapper.Map<Medication>(dto);

        await _repository.AddAsync(medication);

        return _mapper.Map<MedicationResponseDto>(medication);
    }

    public async Task<bool> UpdateMedicationAsync(int code, MedicationRequestDto dto)
    {
        var medication = await _repository.GetByIdAsync(code);

        if (medication == null)
            return false;

        _mapper.Map(dto, medication);

        await _repository.UpdateAsync(medication);
         return true;
    }

    public async Task<bool> DeleteMedicationAsync(int code)
    {
        var medication = await _repository.GetByIdAsync(code);

        if (medication == null)
            return false;

        await _repository.DeleteAsync(medication);

        return true;
    }
}