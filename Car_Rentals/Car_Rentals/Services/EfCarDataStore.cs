using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Car_Rentals.Models;

namespace Car_Rentals.Services
{
    public class EfCarDataStore : ICarDataStore
    {
        private readonly CarRentalDbContext _context;

        public EfCarDataStore()
        {
            _context = new CarRentalDbContext();
            _context.Database.EnsureCreated();
        }

        public async Task<bool> AddCarAsync(Car car)
        {
            _context.Cars.Add(car);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateCarAsync(Car car)
        {
            _context.Cars.Update(car);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteCarAsync(string id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car != null)
            {
                _context.Cars.Remove(car);
                return await _context.SaveChangesAsync() > 0;
            }
            return false;
        }

        public async Task<Car> GetCarAsync(string id)
        {
            return await _context.Cars.FindAsync(id);
        }

        public async Task<IEnumerable<Car>> GetCarsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(_context.Cars.ToList());
        }

        public async Task<IEnumerable<Car>> GetCarsByCategoryAsync(string category)
        {
            return await Task.FromResult(_context.Cars.Where(c => !string.IsNullOrEmpty(c.Category) && c.Category == category).ToList());
        }

        public async Task<IEnumerable<Car>> GetAvailableCarsAsync()
        {
            return await Task.FromResult(_context.Cars.Where(c => c.IsAvailable == true).ToList());
        }

        public async Task<bool> UpdateCarAvailabilityAsync(string carId, bool isAvailable)
        {
            var car = await _context.Cars.FindAsync(carId);
            if (car != null)
            {
                car.IsAvailable = isAvailable;
                _context.Cars.Update(car);
                return await _context.SaveChangesAsync() > 0;
            }
            return false;
        }


    }
}
