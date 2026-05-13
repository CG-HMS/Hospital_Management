using AutoMapper;
using Hms.API.DTOs;
using Hms.API.Models;

namespace Hms.API.Mapping;

public class StayMappingProfile : Profile
{
    public StayMappingProfile()
    {
        CreateMap<Stay, StayDTO>().ReverseMap();

        CreateMap<Stay, CreateStayDTO>().ReverseMap();

        CreateMap<Stay, UpdateStayDTO>().ReverseMap();

        CreateMap<Stay, StayDetailDTO>()
            .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => src.PatientNavigation!.Name))
            .ForMember(dest => dest.RoomNumber, opt => opt.MapFrom(src => src.RoomNavigation!.RoomNumber.ToString()))
            .ForMember(dest => dest.DaysOfStay, opt => opt.MapFrom(src => (int)(src.StayEnd - src.StayStart).TotalDays));
    }
}
