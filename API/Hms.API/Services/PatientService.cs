using Hms.API.DTOs.Patient;
using AutoMapper;
using Hms.API.Models;
using Hms.API.Repository.Interfaces;
using Hms.API.Services.Interfaces;

namespace Hms.API.Services;

public class PatientService : IPatientService
{
    private readonly IPatientRepository _repository;
    private readonly IMapper _mapper;

    public PatientService(IPatientRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<PatientResponseDto>> GetAllPatientsAsync()
    {
        var patients = await _repository.GetAllAsync();

        return _mapper.Map<IEnumerable<PatientResponseDto>>(patients);
    }
    public async Task<PatientResponseDto?> GetPatientByIdAsync(int ssn)
    {
        var patient = await _repository.GetByIdAsync(ssn);

        if (patient == null)
            return null;

        return _mapper.Map<PatientResponseDto>(patient);
    }

    public async Task<PatientResponseDto> CreatePatientAsync(PatientRequestDto dto)
    {
        var patient = _mapper.Map<Patient>(dto);

        await _repository.AddAsync(patient);

        return _mapper.Map<PatientResponseDto>(patient);
    }

    public async Task<bool> UpdatePatientAsync(int ssn, PatientRequestDto dto)
    {
        var patient = await _repository.GetByIdAsync(ssn);

        if (patient == null)
            return false;
             _mapper.Map(dto, patient);

        await _repository.UpdateAsync(patient);

        return true;
    }

    public async Task<bool> DeletePatientAsync(int ssn)
    {
        var patient = await _repository.GetByIdAsync(ssn);

        if (patient == null)
            return false;

        await _repository.DeleteAsync(patient);

        return true;
    }
}