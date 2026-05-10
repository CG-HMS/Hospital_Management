using Hms.API.DTOs;
using Hms.API.Models;
using AutoMapper;

namespace Hms.API.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile() {

            CreateMap<Physician, PhysicianDto>();

            CreateMap<CreatePhysicianDto, Physician>();

            CreateMap<UpdatePhysicianDto, Physician>();

            CreateMap<Department, DepartmentDto>();

            CreateMap<Procedure, ProcedureDto>();

            CreateMap<Patient, PatientDto>();

            CreateMap<Procedure, ProcedureDto>();

            CreateMap<CreateProcedureDto, Procedure>();

            CreateMap<UpdateProcedureDto, Procedure>();
        }
    }
}
