using AutoMapper;
using Hms.API.DTOs;
using Hms.API.Models;

namespace Hms.API.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // ── Room ────────────────────────────────────────────────────────────
            CreateMap<Room, RoomDto>();

            // CreateRoomDto → Room  (Unavailable always starts as false)
            CreateMap<CreateRoomDto, Room>()
                .ForMember(dest => dest.Unavailable, opt => opt.MapFrom(_ => false));

            // UpdateRoomDto → Room  (only the three editable fields)
            CreateMap<UpdateRoomDto, Room>()
                .ForMember(dest => dest.RoomNumber, opt => opt.Ignore())
                .ForMember(dest => dest.Unavailable, opt => opt.Ignore())
                .ForMember(dest => dest.Block, opt => opt.Ignore())
                .ForMember(dest => dest.Stays, opt => opt.Ignore());

            // ── User ────────────────────────────────────────────────────────────
            CreateMap<User, UserDto>();

            // CreateUserDto → User
            // NOTE: PasswordHash is intentionally NOT mapped here —
            //       BCrypt hashing is done manually in AuthService.
            CreateMap<CreateUserDto, User>()
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());
        }
    }
}
