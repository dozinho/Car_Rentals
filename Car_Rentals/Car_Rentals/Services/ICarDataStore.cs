using Car_Rentals.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Car_Rentals.Services
{
    public interface ICarDataStore
    {
        Task<bool> AddCarAsync(Car car);
        Task<bool> UpdateCarAsync(Car car);
        Task<bool> DeleteCarAsync(string id);
        Task<Car> GetCarAsync(string id);
        Task<IEnumerable<Car>> GetCarsAsync(bool forceRefresh = false);
        Task<IEnumerable<Car>> GetCarsByCategoryAsync(string category);
        Task<IEnumerable<Car>> GetAvailableCarsAsync();
        Task<bool> UpdateCarAvailabilityAsync(string carId, bool isAvailable);
    }
} 