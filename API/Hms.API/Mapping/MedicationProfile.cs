using AutoMapper;
using Hms.API.Models;
using Hms.API.DTOs.Medication;

namespace Hms.API.Mapping;

public class MedicationProfile : Profile
{
    public MedicationProfile()
    {
        CreateMap<Medication, MedicationResponseDto>();

        CreateMap<CreateMedicationDto, Medication>();

        CreateMap<UpdateMedicationDto, Medication>();
    }
}