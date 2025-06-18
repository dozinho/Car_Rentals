using Car_Rentals.Models;
using System;
using System.Threading.Tasks;

namespace Car_Rentals.Services
{
    public interface IAuthService
    {
        Task<bool> LoginAsync(string username, string password);
        Task<bool> RegisterAsync(User user, Customer customer);
        Task<bool> LogoutAsync();
        Task<User> GetCurrentUserAsync();
        Task<Customer> GetCurrentCustomerAsync();
        Task<bool> IsAuthenticatedAsync();
        Task<bool> ChangePasswordAsync(string currentPassword, string newPassword);
    }
} 