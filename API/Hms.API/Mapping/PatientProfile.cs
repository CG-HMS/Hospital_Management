using AutoMapper;
using Hms.API.DTOs.Patient;
using Hms.API.Models;

namespace Hms.API.Mapping;

public class PatientProfile : Profile
{
    public PatientProfile()
    {
        CreateMap<PatientRequestDto, Patient>();

        CreateMap<Patient, PatientResponseDto>();
    }
}