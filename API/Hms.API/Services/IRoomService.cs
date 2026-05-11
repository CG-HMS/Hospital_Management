using Hms.API.DTOs;

namespace Hms.API.Services
{
    public interface IRoomService
    {
        Task<IEnumerable<RoomDto>> GetAllAsync();
        Task<RoomDto> GetByIdAsync(int roomNumber);
        Task<IEnumerable<RoomDto>> GetAvailableAsync();
        Task<IEnumerable<RoomDto>> GetByBlockAsync(int floor, int code);
        Task<IEnumerable<RoomDto>> GetByTypeAsync(string type);
        Task<RoomDto> CreateAsync(CreateRoomDto dto);
        Task<RoomDto> UpdateAsync(int roomNumber, UpdateRoomDto dto);
        Task UpdateAvailabilityAsync(int roomNumber, bool unavailable);
        Task DeleteAsync(int roomNumber);
    }
}
