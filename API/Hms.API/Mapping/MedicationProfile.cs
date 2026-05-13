using AutoMapper;
using Hms.API.DTOs.Medication;
using Hms.API.Models;

namespace Hms.API.Mapping;

public class MedicationProfile : Profile
{
    public MedicationProfile()
    {
        CreateMap<MedicationRequestDto, Medication>();

        CreateMap<Medication, MedicationResponseDto>();
    }
}