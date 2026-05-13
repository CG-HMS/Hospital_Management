using Hms.API.DTOs;

namespace Hms.API.Services
{
    public interface IAuthService
    {
        Task<LoginResponseDto> LoginAsync(LoginRequestDto dto);
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<UserDto> GetUserByIdAsync(int userId);
        Task<UserDto> CreateUserAsync(CreateUserDto dto);
        Task ChangePasswordAsync(int userId, ChangePasswordDto dto);
        Task UpdateRoleAsync(int userId, string role);
        Task UpdateStatusAsync(int userId, bool isActive);
    }
}