using Car_Rentals.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Car_Rentals.Services
{
    public class EfAuthService : IAuthService
    {
        private readonly IUserDataStore _userDataStore;
        private readonly ICustomerDataStore _customerDataStore;
        private User currentUser;
        private Customer currentCustomer;

        public EfAuthService()
        {
            _userDataStore = DependencyService.Get<IUserDataStore>();
            _customerDataStore = DependencyService.Get<ICustomerDataStore>();
            // Restore session if present
            if (Application.Current.Properties.ContainsKey("CurrentUserId"))
            {
                var userId = Application.Current.Properties["CurrentUserId"] as string;
                if (!string.IsNullOrEmpty(userId))
                {
                    currentUser = _userDataStore.GetUserAsync(userId).Result;
                }
            }
            if (Application.Current.Properties.ContainsKey("CurrentCustomerId"))
            {
                var customerId = Application.Current.Properties["CurrentCustomerId"] as string;
                if (!string.IsNullOrEmpty(customerId))
                {
                    currentCustomer = _customerDataStore.GetCustomerAsync(customerId).Result;
                }
            }
        }

        public bool IsLoggedIn => currentUser != null;

        public async Task<bool> LoginAsync(string username, string password)
        {
            try
            {
                var users = await _userDataStore.GetUsersAsync();
                System.Diagnostics.Debug.WriteLine($"[DEBUG] Attempting login for: {username}");
                foreach (var u in users)
                {
                    System.Diagnostics.Debug.WriteLine($"[DEBUG] User: {u.Username}, Email: {u.Email}, IsActive: {u.IsActive}, PasswordHash: {u.PasswordHash}");
                }
                var user = users.FirstOrDefault(u => 
                    (u.Username == username || u.Email == username) && 
                    u.PasswordHash == password && 
                    u.IsActive);

                if (user != null)
                {
                    currentUser = user;
                    user.LastLoginDate = DateTime.Now;
                    await _userDataStore.UpdateUserAsync(user);
                    Application.Current.Properties["CurrentUserId"] = user.Id;
                    await Application.Current.SavePropertiesAsync();
                    
                    if (user.CustomerId != null)
                    {
                        currentCustomer = await _customerDataStore.GetCustomerAsync(user.CustomerId);
                        Application.Current.Properties["CurrentCustomerId"] = currentCustomer.Id;
                        await Application.Current.SavePropertiesAsync();
                    }
                    else
                    {
                        Application.Current.Properties.Remove("CurrentCustomerId");
                        await Application.Current.SavePropertiesAsync();
                    }
                    System.Diagnostics.Debug.WriteLine($"[DEBUG] Login successful for: {username}");
                    return true;
                }
                System.Diagnostics.Debug.WriteLine($"[DEBUG] Login failed for: {username}");
                return false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Login error: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> RegisterAsync(User user, Customer customer)
        {
            try
            {
                var users = await _userDataStore.GetUsersAsync();
                System.Diagnostics.Debug.WriteLine($"[DEBUG] Registering user: {user.Username}, Email: {user.Email}");
                foreach (var u in users)
                {
                    System.Diagnostics.Debug.WriteLine($"[DEBUG] Existing User: {u.Username}, Email: {u.Email}, IsActive: {u.IsActive}, PasswordHash: {u.PasswordHash}");
                }
                // Check if username or email already exists
                if (users.Any(u => u.Username == user.Username || u.Email == user.Email))
                {
                    System.Diagnostics.Debug.WriteLine($"[DEBUG] Registration failed: Username or email already exists.");
                    return false;
                }

                // Add customer first
                customer.Id = Guid.NewGuid().ToString();
                customer.CreatedDate = DateTime.Now;
                customer.IsActive = true;
                var customerAdded = await _customerDataStore.AddCustomerAsync(customer);

                if (!customerAdded)
                    return false;

                // Add user
                user.Id = Guid.NewGuid().ToString();
                user.CustomerId = customer.Id;
                user.CreatedDate = DateTime.Now;
                user.IsActive = true;
                var userAdded = await _userDataStore.AddUserAsync(user);

                System.Diagnostics.Debug.WriteLine($"[DEBUG] Registration {(userAdded ? "successful" : "failed")} for: {user.Username}");
                return userAdded;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Registration error: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> LogoutAsync()
        {
            currentUser = null;
            currentCustomer = null;
            Application.Current.Properties.Remove("CurrentUserId");
            Application.Current.Properties.Remove("CurrentCustomerId");
            await Application.Current.SavePropertiesAsync();
            return await Task.FromResult(true);
        }

        public async Task<User> GetCurrentUserAsync()
        {
            return await Task.FromResult(currentUser);
        }

        public async Task<Customer> GetCurrentCustomerAsync()
        {
            return await Task.FromResult(currentCustomer);
        }

        public async Task<bool> IsAuthenticatedAsync()
        {
            return await Task.FromResult(IsLoggedIn);
        }

        public async Task<bool> ChangePasswordAsync(string currentPassword, string newPassword)
        {
            try
            {
                if (currentUser == null)
                    return false;

                if (currentUser.PasswordHash != currentPassword)
                    return false;

                currentUser.PasswordHash = newPassword;
                return await _userDataStore.UpdateUserAsync(currentUser);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Change password error: {ex.Message}");
                return false;
            }
        }
    }
} 