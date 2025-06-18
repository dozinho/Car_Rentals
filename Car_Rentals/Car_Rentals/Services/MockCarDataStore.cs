using Car_Rentals.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Car_Rentals.Services
{
    public class MockCarDataStore : ICarDataStore
    {
        readonly List<Car> cars;

        public MockCarDataStore()
        {
            cars = new List<Car>()
            {
                new Car { 
                    Id = Guid.NewGuid().ToString(), 
                    Brand = "Toyota", 
                    Model = "Camry", 
                    Year = 2022, 
                    Color = "Silver", 
                    Transmission = "Automatic", 
                    FuelType = "Gasoline", 
                    Mileage = 15000, 
                    DailyRate = 45.00m, 
                    ImageUrl = "https://example.com/camry.jpg", 
                    Description = "Reliable mid-size sedan perfect for daily commuting and business trips.", 
                    IsAvailable = true, 
                    LicensePlate = "ABC123", 
                    Seats = 5, 
                    Category = "Mid-size" 
                },
                new Car { 
                    Id = Guid.NewGuid().ToString(), 
                    Brand = "Honda", 
                    Model = "Civic", 
                    Year = 2021, 
                    Color = "Blue", 
                    Transmission = "Automatic", 
                    FuelType = "Gasoline", 
                    Mileage = 22000, 
                    DailyRate = 35.00m, 
                    ImageUrl = "https://example.com/civic.jpg", 
                    Description = "Fuel-efficient compact car ideal for city driving.", 
                    IsAvailable = true, 
                    LicensePlate = "XYZ789", 
                    Seats = 5, 
                    Category = "Compact" 
                },
                new Car { 
                    Id = Guid.NewGuid().ToString(), 
                    Brand = "Ford", 
                    Model = "Explorer", 
                    Year = 2023, 
                    Color = "Black", 
                    Transmission = "Automatic", 
                    FuelType = "Gasoline", 
                    Mileage = 8000, 
                    DailyRate = 65.00m, 
                    ImageUrl = "https://example.com/explorer.jpg", 
                    Description = "Spacious SUV perfect for family trips and outdoor adventures.", 
                    IsAvailable = true, 
                    LicensePlate = "DEF456", 
                    Seats = 7, 
                    Category = "SUV" 
                },
                new Car { 
                    Id = Guid.NewGuid().ToString(), 
                    Brand = "BMW", 
                    Model = "3 Series", 
                    Year = 2022, 
                    Color = "White", 
                    Transmission = "Automatic", 
                    FuelType = "Gasoline", 
                    Mileage = 12000, 
                    DailyRate = 85.00m, 
                    ImageUrl = "https://example.com/bmw3.jpg", 
                    Description = "Luxury sedan with premium features and excellent performance.", 
                    IsAvailable = true, 
                    LicensePlate = "GHI789", 
                    Seats = 5, 
                    Category = "Luxury" 
                },
                new Car { 
                    Id = Guid.NewGuid().ToString(), 
                    Brand = "Nissan", 
                    Model = "Altima", 
                    Year = 2021, 
                    Color = "Red", 
                    Transmission = "Automatic", 
                    FuelType = "Gasoline", 
                    Mileage = 18000, 
                    DailyRate = 40.00m, 
                    ImageUrl = "https://example.com/altima.jpg", 
                    Description = "Comfortable mid-size sedan with great fuel economy.", 
                    IsAvailable = false, 
                    LicensePlate = "JKL012", 
                    Seats = 5, 
                    Category = "Mid-size" 
                },
                new Car { 
                    Id = Guid.NewGuid().ToString(), 
                    Brand = "Chevrolet", 
                    Model = "Spark", 
                    Year = 2020, 
                    Color = "Green", 
                    Transmission = "Manual", 
                    FuelType = "Gasoline", 
                    Mileage = 25000, 
                    DailyRate = 25.00m, 
                    ImageUrl = "https://example.com/spark.jpg", 
                    Description = "Economical subcompact car perfect for budget-conscious travelers.", 
                    IsAvailable = true, 
                    LicensePlate = "MNO345", 
                    Seats = 4, 
                    Category = "Economy" 
                }
            };
        }

        public async Task<bool> AddCarAsync(Car car)
        {
            cars.Add(car);
            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateCarAsync(Car car)
        {
            var oldCar = cars.Where((Car arg) => arg.Id == car.Id).FirstOrDefault();
            cars.Remove(oldCar);
            cars.Add(car);
            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteCarAsync(string id)
        {
            var oldCar = cars.Where((Car arg) => arg.Id == id).FirstOrDefault();
            cars.Remove(oldCar);
            return await Task.FromResult(true);
        }

        public async Task<Car> GetCarAsync(string id)
        {
            return await Task.FromResult(cars.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<Car>> GetCarsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(cars);
        }

        public async Task<IEnumerable<Car>> GetCarsByCategoryAsync(string category)
        {
            return await Task.FromResult(cars.Where(c => c.Category == category));
        }

        public async Task<IEnumerable<Car>> GetAvailableCarsAsync()
        {
            return await Task.FromResult(cars.Where(c => c.IsAvailable));
        }

        public async Task<bool> UpdateCarAvailabilityAsync(string carId, bool isAvailable)
        {
            var car = cars.FirstOrDefault(c => c.Id == carId);
            if (car != null)
            {
                car.IsAvailable = isAvailable;
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }
    }
} 