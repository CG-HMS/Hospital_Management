using AutoMapper;
using Hms.API.DTOs;
using Hms.API.Models;

namespace Hms.API.Mapping;

public class PrescriptionMappingProfile : Profile
{
    public PrescriptionMappingProfile()
    {
        CreateMap<Prescribe, PrescriptionDTO>().ReverseMap();

        CreateMap<Prescribe, CreatePrescriptionDTO>().ReverseMap();

        CreateMap<Prescribe, UpdatePrescriptionDTO>().ReverseMap();

        CreateMap<Prescribe, PrescriptionDetailDTO>()
            .ForMember(dest => dest.PhysicianName, opt => opt.MapFrom(src => src.PhysicianNavigation!.Name))
            .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => src.PatientNavigation!.Name))
            .ForMember(dest => dest.MedicationName, opt => opt.MapFrom(src => src.MedicationNavigation!.Name));
    }
}
