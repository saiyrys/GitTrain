using Konscious.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Profunion.Data;
using Profunion.Interfaces;
using Profunion.Models;
using System.Text;

namespace Profunion.Services.UserServices
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserByID(string ID)
        {
            return await _context.Users.Where(u => u.UserID == ID).FirstOrDefaultAsync();
        }

        public async Task<User> GetUserByUsername(string username)
        {
            return await _context.Users.Where(u => u.Username == username).FirstOrDefaultAsync();
        }

        public async Task<ICollection<User>> GetUsers()
        {
            return await _context.Users.OrderBy(u => u.UserID).ToListAsync();
        }

        public async Task<bool> UserExists(string userid)
        {
            return await _context.Users.AnyAsync(u => u.UserID == userid);
        }
        public async Task<bool> CreateUser(User user)
        {
            HashPassword(user);
           
            _context.Add(user);
            await _context.SaveChangesAsync();

            return await SaveUser();
        }

        private void HashPassword(User user)
        {
            using (var hasher = new Argon2id(Encoding.UTF8.GetBytes(user.PasswordHash)))
            {
                // You can tweak parameters as needed
                hasher.DegreeOfParallelism = 8;
                hasher.MemorySize = 65536; // 64MB
                hasher.Iterations = 100;

                user.PasswordHash = Convert.ToBase64String(hasher.GetBytes(32)); // 32 bytes for a secure hash
            }
        }
        public async Task<bool> SaveUser()
        {
            var saved = await _context.SaveChangesAsync();

            return saved > 0 ? true : false;
        }

        public async Task<bool> UpdateUser(User user)
        {
            _context.Update(user);

            return await _context.SaveChangesAsync() > 0;


        }
        public async Task<bool> DeleteUser(User user)
        {
            _context.Remove(user);

            return await SaveUser();
        }

    }
}
