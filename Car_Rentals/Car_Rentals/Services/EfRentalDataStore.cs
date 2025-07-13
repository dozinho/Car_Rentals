using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Car_Rentals.Models;

namespace Car_Rentals.Services
{
    public class EfRentalDataStore : IRentalDataStore
    {
        private readonly CarRentalDbContext _context;

        public EfRentalDataStore()
        {
            _context = new CarRentalDbContext();
            _context.Database.EnsureCreated();
        }

        public async Task<bool> AddRentalAsync(Rental rental)
        {
            _context.Rentals.Add(rental);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateRentalAsync(Rental rental)
        {
            _context.Rentals.Update(rental);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteRentalAsync(string id)
        {
            var rental = await _context.Rentals.FindAsync(id);
            if (rental != null)
            {
                _context.Rentals.Remove(rental);
                return await _context.SaveChangesAsync() > 0;
            }
            return false;
        }

        public async Task<Rental> GetRentalAsync(string id)
        {
            return await _context.Rentals.FindAsync(id);
        }

        public async Task<IEnumerable<Rental>> GetRentalsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(_context.Rentals.ToList());
        }

        public async Task<IEnumerable<Rental>> GetRentalsByCustomerAsync(string customerId)
        {
            return await Task.FromResult(_context.Rentals.Where(r => r.CustomerId == customerId).ToList());
        }

        public async Task<IEnumerable<Rental>> GetRentalsByCarAsync(string carId)
        {
            return await Task.FromResult(_context.Rentals.Where(r => r.CarId == carId).ToList());
        }

        public async Task<IEnumerable<Rental>> GetActiveRentalsAsync()
        {
            return await Task.FromResult(_context.Rentals.Where(r => r.Status == "Active").ToList());
        }

        public async Task<bool> CancelRentalAsync(string rentalId)
        {
            var rental = await _context.Rentals.FindAsync(rentalId);
            if (rental != null)
            {
                rental.Status = "Cancelled";
                _context.Rentals.Update(rental);
                return await _context.SaveChangesAsync() > 0;
            }
            return false;
        }


    }
}
