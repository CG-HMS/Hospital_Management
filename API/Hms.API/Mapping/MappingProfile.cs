using AutoMapper;
using Hms.API.DTOs;
using Hms.API.Models;

namespace Hms.API.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Nurse, NurseDto>().ReverseMap();
        CreateMap<Appointment, AppointmentDto>().ReverseMap();
    }
}
