using Car_Rentals.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Car_Rentals.Services
{
    public class MockAuthService : IAuthService
    {
        private readonly List<User> users;
        private readonly List<Customer> customers;
        private User currentUser;
        private Customer currentCustomer;

        public MockAuthService()
        {
            customers = new List<Customer>
            {
                new Customer
                {
                    Id = Guid.NewGuid().ToString(),
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "john.doe@email.com",
                    Phone = "555-0123",
                    LicenseNumber = "DL123456789",
                    LicenseExpiryDate = DateTime.Now.AddYears(3),
                    DateOfBirth = new DateTime(1985, 5, 15),
                    Address = "123 Main St",
                    City = "New York",
                    State = "NY",
                    ZipCode = "10001",
                    CreatedDate = DateTime.Now.AddDays(-30),
                    IsActive = true
                },
                new Customer
                {
                    Id = Guid.NewGuid().ToString(),
                    FirstName = "Jane",
                    LastName = "Smith",
                    Email = "jane.smith@email.com",
                    Phone = "555-0456",
                    LicenseNumber = "DL987654321",
                    LicenseExpiryDate = DateTime.Now.AddYears(2),
                    DateOfBirth = new DateTime(1990, 8, 22),
                    Address = "456 Oak Ave",
                    City = "Los Angeles",
                    State = "CA",
                    ZipCode = "90210",
                    CreatedDate = DateTime.Now.AddDays(-15),
                    IsActive = true
                }
            };

            users = new List<User>
            {
                new User
                {
                    Id = Guid.NewGuid().ToString(),
                    Username = "johndoe",
                    Email = "john.doe@email.com",
                    PasswordHash = "password123", // In real app, this would be hashed
                    CustomerId = customers[0].Id,
                    IsAdmin = false,
                    CreatedDate = DateTime.Now.AddDays(-30),
                    LastLoginDate = DateTime.Now.AddDays(-1),
                    IsActive = true
                },
                new User
                {
                    Id = Guid.NewGuid().ToString(),
                    Username = "janesmith",
                    Email = "jane.smith@email.com",
                    PasswordHash = "password123", // In real app, this would be hashed
                    CustomerId = customers[1].Id,
                    IsAdmin = false,
                    CreatedDate = DateTime.Now.AddDays(-15),
                    LastLoginDate = DateTime.Now.AddDays(-2),
                    IsActive = true
                },
                new User
                {
                    Id = Guid.NewGuid().ToString(),
                    Username = "admin",
                    Email = "admin@carrentals.com",
                    PasswordHash = "admin123", // In real app, this would be hashed
                    CustomerId = null,
                    IsAdmin = true,
                    CreatedDate = DateTime.Now.AddDays(-60),
                    LastLoginDate = DateTime.Now.AddDays(-5),
                    IsActive = true
                }
            };
        }

        public bool IsLoggedIn => currentUser != null;

        public async Task<bool> LoginAsync(string username, string password)
        {
            var user = users.FirstOrDefault(u => 
                (u.Username == username || u.Email == username) && 
                u.PasswordHash == password && 
                u.IsActive);

            if (user != null)
            {
                currentUser = user;
                user.LastLoginDate = DateTime.Now;
                
                if (user.CustomerId != null)
                {
                    currentCustomer = customers.FirstOrDefault(c => c.Id == user.CustomerId);
                }
                
                return await Task.FromResult(true);
            }
            
            return await Task.FromResult(false);
        }

        public async Task<bool> RegisterAsync(User user, Customer customer)
        {
            // Check if username or email already exists
            if (users.Any(u => u.Username == user.Username || u.Email == user.Email))
            {
                return await Task.FromResult(false);
            }

            // Add customer first
            customer.Id = Guid.NewGuid().ToString();
            customer.CreatedDate = DateTime.Now;
            customer.IsActive = true;
            customers.Add(customer);

            // Add user
            user.Id = Guid.NewGuid().ToString();
            user.CustomerId = customer.Id;
            user.CreatedDate = DateTime.Now;
            user.IsActive = true;
            users.Add(user);

            return await Task.FromResult(true);
        }

        public async Task<bool> LogoutAsync()
        {
            currentUser = null;
            currentCustomer = null;
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
            return await Task.FromResult(currentUser != null);
        }

        public async Task<bool> ChangePasswordAsync(string currentPassword, string newPassword)
        {
            if (currentUser != null && currentUser.PasswordHash == currentPassword)
            {
                currentUser.PasswordHash = newPassword;
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }
    }
} 