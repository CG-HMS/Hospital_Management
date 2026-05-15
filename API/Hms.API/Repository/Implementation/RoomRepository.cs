using Hms.API.Data;
using Hms.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Hms.API.Repository
{
    public class RoomRepository : IRoomRepository
    {
        private readonly MyAppDbContext _db;
        public RoomRepository(MyAppDbContext db) => _db = db;

        public async Task<IEnumerable<Room>> GetAllAsync()
            => await _db.Rooms.Include(r => r.Block)
                .OrderBy(r => r.RoomNumber).ToListAsync();

        public Task<Room?> GetByIdAsync(int roomNumber)
            => _db.Rooms.Include(r => r.Block)
                .FirstOrDefaultAsync(r => r.RoomNumber == roomNumber);

        public async Task<IEnumerable<Room>> GetAvailableAsync()
            => await _db.Rooms.Include(r => r.Block)
                .Where(r => !r.Unavailable)
                .OrderBy(r => r.RoomNumber).ToListAsync();

        public async Task<IEnumerable<Room>> GetByBlockAsync(int floor, int code)
            => await _db.Rooms
                .Where(r => r.BlockFloor == floor && r.BlockCode == code)
                .OrderBy(r => r.RoomNumber).ToListAsync();

        public async Task<IEnumerable<Room>> GetByTypeAsync(string type)
            => await _db.Rooms.Include(r => r.Block)
                .Where(r => r.RoomType.ToLower() == type.ToLower())
                .ToListAsync();

        public async Task<Room> CreateAsync(Room room)
        {
            _db.Rooms.Add(room);
            await _db.SaveChangesAsync();
            return room;
        }

        public async Task UpdateAsync(Room room)
        {
            _db.Rooms.Update(room);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int roomNumber)
        {
            var room = await _db.Rooms.FindAsync(roomNumber);
            if (room is not null)
            {
                _db.Rooms.Remove(room);
                await _db.SaveChangesAsync();
            }
        }

        public Task<bool> ExistsAsync(int roomNumber)
            => _db.Rooms.AnyAsync(r => r.RoomNumber == roomNumber);

        public Task<bool> BlockExistsAsync(int floor, int code)
            => _db.Blocks.AnyAsync(b => b.BlockFloor == floor && b.BlockCode == code);

        public async Task EnsureBlockExistsAsync(int floor, int code)
        {
            if (!await BlockExistsAsync(floor, code))
            {
                _db.Blocks.Add(new Block { BlockFloor = floor, BlockCode = code });
                await _db.SaveChangesAsync();
            }
        }

        public async Task<List<DTOs.RoomPatientHistoryDto>> GetRoomPatientHistoryAsync(int roomNumber)
        {
            return await _db.Stays
                .Where(s => s.Room == roomNumber)
                .Include(s => s.PatientNavigation)
                .OrderByDescending(s => s.StayStart)
                .Select(s => new DTOs.RoomPatientHistoryDto
                {
                    StayId = s.StayId,
                    PatientId = s.Patient,
                    PatientName = s.PatientNavigation.Name,
                    StayStart = s.StayStart,
                    StayEnd = s.StayEnd
                })
                .ToListAsync();
        }

        public async Task<List<DTOs.RoomUtilizationDto>> GetRoomUtilizationAsync()
        {
            return await _db.Stays
                .GroupBy(s => s.Room)
                .Select(g => new DTOs.RoomUtilizationDto
                {
                    RoomNumber = g.Key,
                    TotalStays = g.Count(),
                    LastStayEnd = g.Max(x => x.StayEnd)
                })
                .OrderByDescending(x => x.TotalStays)
                .ToListAsync();
        }

        public async Task<List<int>> GetOccupiedRoomsAsync(DateTime now)
        {
            return await _db.Stays
                .Where(s => s.StayStart <= now && s.StayEnd >= now)
                .Select(s => s.Room)
                .Distinct()
                .ToListAsync();
        }
    }
}
