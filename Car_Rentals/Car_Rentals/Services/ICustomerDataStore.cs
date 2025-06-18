using Car_Rentals.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Car_Rentals.Services
{
    public interface ICustomerDataStore
    {
        Task<bool> AddCustomerAsync(Customer customer);
        Task<bool> UpdateCustomerAsync(Customer customer);
        Task<bool> DeleteCustomerAsync(string id);
        Task<Customer> GetCustomerAsync(string id);
        Task<Customer> GetCustomerByEmailAsync(string email);
        Task<IEnumerable<Customer>> GetCustomersAsync(bool forceRefresh = false);
        Task<bool> ValidateLicenseAsync(string licenseNumber);
    }
} 