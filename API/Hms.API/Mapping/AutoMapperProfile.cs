using AutoMapper;
using Hms.API.DTOs;
using Hms.API.Models;

namespace Hms.API.Mapping;

/// <summary>
/// Unified AutoMapper Profile for all entity-to-DTO mappings
/// </summary>
public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        ConfigureStayMappings();
        ConfigurePrescriptionMappings();
    }

    private void ConfigureStayMappings()
    {
        CreateMap<Stay, StayDTO>().ReverseMap();

        CreateMap<Stay, CreateStayDTO>().ReverseMap();

        CreateMap<Stay, UpdateStayDTO>().ReverseMap();

        CreateMap<Stay, StayDetailDTO>()
            .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => src.PatientNavigation!.Name))
            .ForMember(dest => dest.RoomNumber, opt => opt.MapFrom(src => src.RoomNavigation!.RoomNumber.ToString()))
            .ForMember(dest => dest.DaysOfStay, opt => opt.MapFrom(src => (int)(src.StayEnd - src.StayStart).TotalDays));
    }

    private void ConfigurePrescriptionMappings()
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