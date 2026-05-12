using AutoMapper;

using Hms.API.DTOs.Patient;
using Hms.API.Exceptions;
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

    public async Task<PatientResponseDto> GetPatientByIdAsync(int ssn)
    {
        var patient = await _repository.GetByIdAsync(ssn);

        if (patient == null)
            throw new NotFoundException("Patient", ssn);

        return _mapper.Map<PatientResponseDto>(patient);
    }

    public async Task<PatientResponseDto> CreatePatientAsync(PatientRequestDto dto)
    {
        var existingPatient = await _repository.GetByIdAsync(dto.Ssn);

        if (existingPatient != null)
            throw new ConflictException(
                $"Patient with SSN {dto.Ssn} already exists"
            );

        var patient = _mapper.Map<Patient>(dto);

        await _repository.AddAsync(patient);

        return _mapper.Map<PatientResponseDto>(patient);
    }

    public async Task UpdatePatientAsync(int ssn, PatientRequestDto dto)
    {
        var patient = await _repository.GetByIdAsync(ssn);

        if (patient == null)
            throw new NotFoundException("Patient", ssn);

        _mapper.Map(dto, patient);

        await _repository.UpdateAsync(patient);
    }

    public async Task DeletePatientAsync(int ssn)
    {
        var patient = await _repository.GetByIdAsync(ssn);

        if (patient == null)
            throw new NotFoundException("Patient", ssn);

        await _repository.DeleteAsync(patient);
    }
}