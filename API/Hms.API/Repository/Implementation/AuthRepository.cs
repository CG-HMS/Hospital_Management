using Hms.API.Data;
using Hms.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Hms.API.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly MyAppDbContext _db;
        public AuthRepository(MyAppDbContext db) => _db = db;

        public Task<User?> GetByIdAsync(int userId)
            => _db.Users.FindAsync(userId).AsTask();

        public Task<User?> GetByEmailAsync(string email)
            => _db.Users.FirstOrDefaultAsync(u => u.Email == email.ToLower());

        public async Task<IEnumerable<User>> GetAllAsync()
            => await _db.Users
                .OrderBy(u => u.Role).ThenBy(u => u.Username)
                .ToListAsync();

        public async Task<User> CreateAsync(User user)
        {
            _db.Users.Add(user);
            await _db.SaveChangesAsync();
            return user;
        }

        public async Task UpdateAsync(User user)
        {
            _db.Users.Update(user);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int userId)
        {
            var user = await _db.Users.FindAsync(userId);
            if (user is not null)
            {
                _db.Users.Remove(user);
                await _db.SaveChangesAsync();
            }
        }

        public Task<bool> EmailExistsAsync(string email)
            => _db.Users.AnyAsync(u => u.Email == email.ToLower());

        public Task<bool> UsernameExistsAsync(string username)
            => _db.Users.AnyAsync(u => u.Username == username.ToLower());
    }
}
