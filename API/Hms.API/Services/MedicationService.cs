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

        public MedicationService(
            IMedicationRepository repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<MedicationResponseDto>> GetAllAsync()
        {
            var medications = await _repository.GetAllAsync();

            return _mapper.Map<IEnumerable<MedicationResponseDto>>(medications);
        }
        public async Task<MedicationResponseDto?> GetByIdAsync(int code)
        {
            var medication = await _repository.GetByIdAsync(code);

            if (medication == null)
                return null;

            return _mapper.Map<MedicationResponseDto>(medication);
        }

        public async Task<MedicationResponseDto> CreateAsync(CreateMedicationDto dto)
        {
            var medication = _mapper.Map<Medication>(dto);

            await _repository.AddAsync(medication);

            return _mapper.Map<MedicationResponseDto>(medication);
        }
        public async Task<bool> UpdateAsync(int code, UpdateMedicationDto dto)
        {
            var existingMedication = await _repository.GetByIdAsync(code);

            if (existingMedication == null)
                return false;

            _mapper.Map(dto, existingMedication);

            await _repository.UpdateAsync(existingMedication);

            return true;
        }

        public async Task<bool> DeleteAsync(int code)
        {
            var medication = await _repository.GetByIdAsync(code);

            if (medication == null)
                return false;

            await _repository.DeleteAsync(medication);

            return true;
        }
    }
