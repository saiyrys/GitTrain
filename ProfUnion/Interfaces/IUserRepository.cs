using Profunion.Models;

namespace Profunion.Interfaces
{
    public interface IUserRepository
    {
        Task<ICollection<User>> GetUsers();
        Task<User> GetUserByID(string ID);
        Task<User> GetUserByUsername(string username);
        Task<bool> UserExists(string userid);
        Task<bool> CreateUser(User user);
        Task<bool> UpdateUser(User user);
        Task<bool> DeleteUser(User user);
        Task<bool> SaveUser();
    }
}
