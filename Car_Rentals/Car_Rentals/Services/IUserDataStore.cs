using System.Collections.Generic;
using System.Threading.Tasks;
using Car_Rentals.Models;

namespace Car_Rentals.Services
{
    public interface IUserDataStore
    {
        Task<bool> AddUserAsync(User user);
        Task<bool> UpdateUserAsync(User user);
        Task<bool> DeleteUserAsync(string id);
        Task<User> GetUserAsync(string id);
        Task<IEnumerable<User>> GetUsersAsync(bool forceRefresh = false);
    }
}
