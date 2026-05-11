using AutoMapper;
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
        private readonly IMapper _mapper;

        private static readonly string[] AllowedRoles = ["admin", "physician", "nurse", "patient"];

        public AuthService(IAuthRepository repo, IConfiguration config, IMapper mapper)
        {
            _repo = repo;
            _config = config;
            _mapper = mapper;
        }

        // ── Login ──────────────────────────────────────────────────────────────
        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto dto)
        {
            var user = await _repo.GetByEmailAsync(dto.Email)
                ?? throw new UnauthorizedException("Invalid email or password.");

            if (!user.IsActive)
                throw new ForbiddenException("Your account has been deactivated. Please contact admin.");

            if (string.IsNullOrWhiteSpace(dto.Password) || string.IsNullOrWhiteSpace(user.PasswordHash))
                throw new UnauthorizedException("Invalid email or password.");

            var hashedPassword = PasswordHasher.ComputeSha256(dto.Password);
            if (!string.Equals(hashedPassword, user.PasswordHash, StringComparison.Ordinal))
                throw new UnauthorizedException("Invalid email or password.");

            return GenerateTokenResponse(user);
        }

        // ── Get All Users ──────────────────────────────────────────────────────
        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
            => _mapper.Map<IEnumerable<UserDto>>(await _repo.GetAllAsync());

        // ── Get User By Id ─────────────────────────────────────────────────────
        public async Task<UserDto> GetUserByIdAsync(int userId)
        {
            var user = await _repo.GetByIdAsync(userId)
                ?? throw new NotFoundException("User", userId);
            return _mapper.Map<UserDto>(user);
        }

        // ── Create User ────────────────────────────────────────────────────────
        public async Task<UserDto> CreateUserAsync(CreateUserDto dto)
        {
            if (await _repo.EmailExistsAsync(dto.Email))
                throw new ConflictException($"Email '{dto.Email}' is already registered.");

            if (await _repo.UsernameExistsAsync(dto.Username))
                throw new ConflictException($"Username '{dto.Username}' is already taken.");

            var user = _mapper.Map<User>(dto);
            user.Username     = dto.Username.ToLower().Trim();
            user.Email        = dto.Email.ToLower().Trim();
            user.Role         = dto.Role.ToLower();
            user.PasswordHash = PasswordHasher.ComputeSha256(dto.Password);
            user.IsActive     = true;
            user.CreatedAt    = DateTime.UtcNow;

            return _mapper.Map<UserDto>(await _repo.CreateAsync(user));
        }

        // ── Change Password ────────────────────────────────────────────────────
        public async Task ChangePasswordAsync(int userId, ChangePasswordDto dto)
        {
            var user = await _repo.GetByIdAsync(userId)
                ?? throw new NotFoundException("User", userId);

            var currentPasswordHash = PasswordHasher.ComputeSha256(dto.CurrentPassword);
            if (!string.Equals(currentPasswordHash, user.PasswordHash, StringComparison.Ordinal))
                throw new BadRequestException("Current password is incorrect.");

            user.PasswordHash = PasswordHasher.ComputeSha256(dto.NewPassword);
            await _repo.UpdateAsync(user);
        }

        // ── Update Role ────────────────────────────────────────────────────────
        public async Task UpdateRoleAsync(int userId, string role)
        {
            var user = await _repo.GetByIdAsync(userId)
                ?? throw new NotFoundException("User", userId);

            if (!AllowedRoles.Contains(role.ToLower()))
                throw new BadRequestException($"'{role}' is not a valid role. Allowed: {string.Join(", ", AllowedRoles)}");

            user.Role = role.ToLower();
            await _repo.UpdateAsync(user);
        }

        // ── Update Status ──────────────────────────────────────────────────────
        public async Task UpdateStatusAsync(int userId, bool isActive)
        {
            var user = await _repo.GetByIdAsync(userId)
                ?? throw new NotFoundException("User", userId);

            user.IsActive = isActive;
            await _repo.UpdateAsync(user);
        }

        // ── Delete User ────────────────────────────────────────────────────────
        public async Task DeleteUserAsync(int userId)
        {
            if (await _repo.GetByIdAsync(userId) is null)
                throw new NotFoundException("User", userId);

            await _repo.DeleteAsync(userId);
        }

        // ── Helper ─────────────────────────────────────────────────────────────
        private LoginResponseDto GenerateTokenResponse(User user)
        {
            var jwtSection = _config.GetSection("JwtSettings");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSection["Secret"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiry = DateTime.UtcNow.AddMinutes(int.Parse(jwtSection["ExpiryInMinutes"]!));

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name,           user.Username),
                new Claim(ClaimTypes.Email,          user.Email),
                new Claim(ClaimTypes.Role,           user.Role)
            };

            var token = new JwtSecurityToken(
                issuer: jwtSection["Issuer"],
                audience: jwtSection["Audience"],
                claims: claims,
                expires: expiry,
                signingCredentials: creds
            );

            return new LoginResponseDto
            {
                Token     = new JwtSecurityTokenHandler().WriteToken(token),
                Username  = user.Username,
                Role      = user.Role,
                ExpiresAt = expiry
            };
        }
    }
}
