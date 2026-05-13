using Hms.API.DTOs;
using Hms.API.Exceptions;
using Hms.API.Models;
using Hms.API.Repository;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Hms.API.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _config;
        private static readonly string[] AllowedRoles = ["admin", "physician", "nurse", "patient"];

        public AuthService(IAuthRepository repo, IConfiguration config)
        {
            _repo = repo;
            _config = config;
        }

        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto dto)
        {
            var user = await _repo.GetByEmailAsync(dto.Email)
                ?? throw new UnauthorizedException("Invalid email or password.");

            if (!user.IsActive)
                throw new ForbiddenException("Your account has been deactivated. Contact admin.");

            if (string.IsNullOrWhiteSpace(dto.Password) || string.IsNullOrWhiteSpace(user.PasswordHash))
                throw new UnauthorizedException("Invalid email or password.");

            var hashed = PasswordHasher.ComputeSha256(dto.Password);
            if (!string.Equals(hashed, user.PasswordHash, StringComparison.Ordinal))
                throw new UnauthorizedException("Invalid email or password.");

            return BuildTokenResponse(user);
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
            => (await _repo.GetAllAsync()).Select(ToDto);

        public async Task<UserDto> GetUserByIdAsync(int userId)
        {
            var user = await _repo.GetByIdAsync(userId)
                ?? throw new NotFoundException("User", userId);
            return ToDto(user);
        }

        public async Task<UserDto> CreateUserAsync(CreateUserDto dto)
        {
            if (await _repo.EmailExistsAsync(dto.Email))
                throw new ConflictException($"Email '{dto.Email}' is already registered.");

            if (await _repo.UsernameExistsAsync(dto.Username))
                throw new ConflictException($"Username '{dto.Username}' is already taken.");

            var user = new User
            {
                Username = dto.Username.ToLower().Trim(),
                Email = dto.Email.ToLower().Trim(),
                PasswordHash = PasswordHasher.ComputeSha256(dto.Password),
                Role = dto.Role.ToLower(),
                RefId = dto.RefId,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            return ToDto(await _repo.CreateAsync(user));
        }

        public async Task ChangePasswordAsync(int userId, ChangePasswordDto dto)
        {
            var user = await _repo.GetByIdAsync(userId)
                ?? throw new NotFoundException("User", userId);

            if (!string.Equals(PasswordHasher.ComputeSha256(dto.CurrentPassword),
                                user.PasswordHash, StringComparison.Ordinal))
                throw new BadRequestException("Current password is incorrect.");

            user.PasswordHash = PasswordHasher.ComputeSha256(dto.NewPassword);
            await _repo.UpdateAsync(user);
        }

        public async Task UpdateRoleAsync(int userId, string role)
        {
            var user = await _repo.GetByIdAsync(userId)
                ?? throw new NotFoundException("User", userId);

            if (!AllowedRoles.Contains(role.ToLower()))
                throw new BadRequestException(
                    $"'{role}' is not a valid role. Allowed: {string.Join(", ", AllowedRoles)}");

            user.Role = role.ToLower();
            await _repo.UpdateAsync(user);
        }

        public async Task UpdateStatusAsync(int userId, bool isActive)
        {
            var user = await _repo.GetByIdAsync(userId)
                ?? throw new NotFoundException("User", userId);

            user.IsActive = isActive;
            await _repo.UpdateAsync(user);
        }

        private LoginResponseDto BuildTokenResponse(User user)
        {
            var jwt = _config.GetSection("JwtSettings");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Secret"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiry = DateTime.UtcNow.AddMinutes(int.Parse(jwt["ExpiryInMinutes"]!));

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name,           user.Username),
                new Claim(ClaimTypes.Email,          user.Email),
                new Claim(ClaimTypes.Role,           user.Role)
            };

            var token = new JwtSecurityToken(
                issuer: jwt["Issuer"],
                audience: jwt["Audience"],
                claims: claims,
                expires: expiry,
                signingCredentials: creds
            );

            return new LoginResponseDto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Username = user.Username,
                Role = user.Role,
                ExpiresAt = expiry
            };
        }

        private static UserDto ToDto(User u) => new()
        {
            UserId = u.UserId,
            Username = u.Username,
            Email = u.Email,
            Role = u.Role,
            RefId = u.RefId,
            IsActive = u.IsActive,
            CreatedAt = u.CreatedAt
        };
    }
}