using Car_Rentals.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Car_Rentals.Services
{
    public interface IRentalDataStore
    {
        Task<bool> AddRentalAsync(Rental rental);
        Task<bool> UpdateRentalAsync(Rental rental);
        Task<bool> DeleteRentalAsync(string id);
        Task<Rental> GetRentalAsync(string id);
        Task<IEnumerable<Rental>> GetRentalsAsync(bool forceRefresh = false);
        Task<IEnumerable<Rental>> GetRentalsByCustomerAsync(string customerId);
        Task<IEnumerable<Rental>> GetRentalsByCarAsync(string carId);
        Task<IEnumerable<Rental>> GetActiveRentalsAsync();
        Task<bool> CancelRentalAsync(string rentalId);
    }
} 