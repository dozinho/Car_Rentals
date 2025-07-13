using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Car_Rentals.Models;

namespace Car_Rentals.Services
{
    public class EfCustomerDataStore : ICustomerDataStore
    {
        private readonly CarRentalDbContext _context;

        public EfCustomerDataStore()
        {
            _context = new CarRentalDbContext();
            _context.Database.EnsureCreated();
        }

        public async Task<bool> AddCustomerAsync(Customer customer)
        {
            _context.Customers.Add(customer);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateCustomerAsync(Customer customer)
        {
            _context.Customers.Update(customer);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteCustomerAsync(string id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
                return await _context.SaveChangesAsync() > 0;
            }
            return false;
        }

        public async Task<Customer> GetCustomerAsync(string id)
        {
            return await _context.Customers.FindAsync(id);
        }

        public async Task<Customer> GetCustomerByEmailAsync(string email)
        {
            return await Task.FromResult(_context.Customers.FirstOrDefault(c => c.Email == email));
        }

        public async Task<IEnumerable<Customer>> GetCustomersAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(_context.Customers.ToList());
        }

        public async Task<bool> ValidateLicenseAsync(string licenseNumber)
        {
            var customer = await Task.FromResult(_context.Customers.FirstOrDefault(c => c.LicenseNumber == licenseNumber));
            return customer != null && customer.LicenseExpiryDate > DateTime.Now;
        }


    }
}
