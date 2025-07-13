using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Car_Rentals.Models;

namespace Car_Rentals.Services
{
    public class EfUserDataStore : IUserDataStore
    {
        private readonly CarRentalDbContext _context;

        public EfUserDataStore()
        {
            _context = new CarRentalDbContext();
            _context.Database.EnsureCreated();
        }

        public async Task<bool> AddUserAsync(User user)
        {
            _context.Users.Add(user);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteUserAsync(string id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                return await _context.SaveChangesAsync() > 0;
            }
            return false;
        }

        public async Task<User> GetUserAsync(string id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<IEnumerable<User>> GetUsersAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(_context.Users.ToList());
        }
    }
}

