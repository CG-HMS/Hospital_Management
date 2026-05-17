using Hms.API.Models;

namespace Hms.API.Repository
{
    public interface IRoomRepository
    {
        Task<IEnumerable<Room>> GetAllAsync();
        Task<Room?> GetByIdAsync(int roomNumber);
        Task<IEnumerable<Room>> GetAvailableAsync();
        Task<IEnumerable<Room>> GetByBlockAsync(int floor, int code);
        Task<IEnumerable<Room>> GetByTypeAsync(string type);
        Task<Room> CreateAsync(Room room);
        Task UpdateAsync(Room room);
        Task DeleteAsync(int roomNumber);
        Task<bool> ExistsAsync(int roomNumber);
        Task<bool> BlockExistsAsync(int floor, int code);
        Task EnsureBlockExistsAsync(int floor, int code);
        Task<List<DTOs.RoomPatientHistoryDto>> GetRoomPatientHistoryAsync(int roomNumber);
        Task<List<DTOs.RoomUtilizationDto>> GetRoomUtilizationAsync();
        Task<List<int>> GetOccupiedRoomsAsync(DateTime now);
    }
}
