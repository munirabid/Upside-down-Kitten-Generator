using RealworldOneBackendTest.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RealworldOneBackendTest.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetUsers();
        Task<User> GetUser(int id);
        Task<User> RegisterUser(User user);
        Task<User> Authenticate(string username, string password);
        Task<bool> UserExists(string username);
    }
}
