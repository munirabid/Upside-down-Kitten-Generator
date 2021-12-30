using Microsoft.EntityFrameworkCore;
using RealworldOneBackendTest.Data;
using RealworldOneBackendTest.Models;
using RealworldOneBackendTest.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RealworldOneBackendTest.Services
{
    public class UserService : IUserService
    {
        private readonly UserContext _context;

        public UserService(UserContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            return user;
        }
        public async Task<User> RegisterUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public Task<User> Authenticate(string username, string password)
        {
            return _context.Users.SingleOrDefaultAsync(x => x.Username == username && x.Password == password);
        }

        public Task<bool> UserExists(string username)
        {
            return _context.Users.AnyAsync(x => x.Username == username);
        }
    }
}
