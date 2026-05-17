using Hms.API.DTOs;
using Hms.API.Exceptions;
using Hms.API.Models;
using Hms.API.Repository;
using Microsoft.Extensions.Logging;

namespace Hms.API.Services
{
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _repo;
        private readonly ILogger<RoomService> _logger;
        public RoomService(IRoomRepository repo, ILogger<RoomService> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public async Task<IEnumerable<RoomDto>> GetAllAsync()
            => (await _repo.GetAllAsync()).Select(ToDto);

        public async Task<RoomDto> GetByIdAsync(int roomNumber)
        {
            var room = await _repo.GetByIdAsync(roomNumber)
                ?? throw new NotFoundException("Room", roomNumber);
            return ToDto(room);
        }

        public async Task<IEnumerable<RoomDto>> GetAvailableAsync()
            => (await _repo.GetAvailableAsync()).Select(ToDto);

        public async Task<IEnumerable<RoomDto>> GetByBlockAsync(int floor, int code)
        {
            if (!await _repo.BlockExistsAsync(floor, code))
                throw new NotFoundException($"Block (Floor={floor}, Code={code}) was not found.");
            return (await _repo.GetByBlockAsync(floor, code)).Select(ToDto);
        }

        public async Task<IEnumerable<RoomDto>> GetByTypeAsync(string type)
            => (await _repo.GetByTypeAsync(type)).Select(ToDto);

        public async Task<RoomDto> CreateAsync(int roomNumber, RoomWriteDto dto)
        {
            if (await _repo.ExistsAsync(roomNumber))
                throw new ConflictException($"Room {roomNumber} already exists.");

            await _repo.EnsureBlockExistsAsync(dto.BlockFloor, dto.BlockCode);

            var room = new Room
            {
                RoomNumber = roomNumber,       
                RoomType = dto.RoomType,
                BlockFloor = dto.BlockFloor,
                BlockCode = dto.BlockCode,
                Unavailable = false 
            };

            return ToDto(await _repo.CreateAsync(room));
        }

        public async Task<RoomDto> UpdateAsync(int roomNumber, RoomWriteDto dto)
        {
            var room = await _repo.GetByIdAsync(roomNumber)
                ?? throw new NotFoundException("Room", roomNumber);

            if (!await _repo.BlockExistsAsync(dto.BlockFloor, dto.BlockCode))
                throw new NotFoundException($"Block (Floor={dto.BlockFloor}, Code={dto.BlockCode}) does not exist.");

            room.RoomType = dto.RoomType;
            room.BlockFloor = dto.BlockFloor;
            room.BlockCode = dto.BlockCode;

            await _repo.UpdateAsync(room);
            return ToDto(room);
        }

        public async Task UpdateAvailabilityAsync(int roomNumber, bool unavailable)
        {
            var room = await _repo.GetByIdAsync(roomNumber)
                ?? throw new NotFoundException("Room", roomNumber);

            room.Unavailable = unavailable;
            await _repo.UpdateAsync(room);
        }

        public async Task<IEnumerable<RoomPatientHistoryDto>> GetRoomPatientHistoryAsync(int roomNumber)
        {
            try
            {
                if (!await _repo.ExistsAsync(roomNumber))
                {
                    throw new NotFoundException("Room", roomNumber);
                }

                return await _repo.GetRoomPatientHistoryAsync(roomNumber);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get room history for room {RoomNumber}.", roomNumber);
                throw new ValidationException("Failed to get room history.");
            }
        }

        public async Task<IEnumerable<RoomUtilizationDto>> GetRoomUtilizationAsync()
        {
            try
            {
                return await _repo.GetRoomUtilizationAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get room utilization.");
                throw new ValidationException("Failed to get room utilization.");
            }
        }

        public async Task<IEnumerable<int>> GetOccupiedRoomsAsync()
        {
            try
            {
                return await _repo.GetOccupiedRoomsAsync(DateTime.Now);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get occupied rooms.");
                throw new ValidationException("Failed to get occupied rooms.");
            }
        }


        private static RoomDto ToDto(Room r) => new()
        {
            RoomNumber = r.RoomNumber,
            RoomType = r.RoomType,
            BlockFloor = r.BlockFloor,
            BlockCode = r.BlockCode,
            Unavailable = r.Unavailable
        };
    }
}