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
                    Id = "benz", 
                    Brand = "Mercedes-Benz", 
                    Model = "G-Class", 
                    Year = 2022, 
                    Color = "Yellow", 
                    Transmission = "A", 
                    FuelConsumption = 12.5, 
                    Mileage = 15000, 
                    DailyRate = 245.00m, 
                    PhotoName = "GWagon.png", 
                    Description = "It's a luxury off-roader and status symbol, often used in premium rental fleets, music videos, and high-end personal transport.", 
                    IsAvailable = true, 
                    LicensePlate = "ABC123", 
                    Seats = 5, 
                    Category = "Mid-size" 
                },
new Car {
    Id = "vclass",
    Brand = "Mercedes-Benz",
    Model = "V-Class",
    Year = 2022,
    Color = "Black",
    Transmission = "A",
    FuelConsumption = 11.5,
    Mileage = 18000, // Estimated; change as needed
    DailyRate = 1600.00m,
    PhotoName = "vclass.png",
    Description = "A premium 8-seater minivan perfect for executive transport, airport pickups, or high-end group travel.",
    IsAvailable = true,
    LicensePlate = "VANLUX8",
    Seats = 8,
    Category = "Luxury"
},
 new Car {
    Id = "durango",
    Brand = "Dodge",
    Model = "Durango",
    Year = 2021,
    Color = "Dark Gray",
    Transmission = "A",
    FuelConsumption = 13.0,
    Mileage = 26000,
    DailyRate = 980m,
    PhotoName = "DodgeDurango.png", // Add to Resources folder
    Description = "The Dodge Durango is a powerful mid-size SUV with aggressive styling and three-row seating � ideal for family adventures or road trips with extra muscle.",
    IsAvailable = true,
    LicensePlate = "DURANGO1",
    Seats = 7,
    Category = "Mid-size"
},
  new Car {
    Id = "qx80",
    Brand = "Infiniti",
    Model = "QX80",
    Year = 2022,
    Color = "Champagne Gold",
    Transmission = "A",
    FuelConsumption = 12.5,
    Mileage = 22000,
    DailyRate = 1200m,
    PhotoName = "qx80.png",
    Description = "A full-size luxury SUV known for its spacious interior, premium comfort, and smooth V8 power � perfect for VIP family or executive rentals.",
    IsAvailable = true,
    LicensePlate = "QX80LUX",
    Seats = 7,
    Category = "SUV"
},

                new Car { 
                    Id = "civic", 
                    Brand = "Honda", 
                    Model = "Civic", 
                    Year = 2021, 
                    Color = "Blue", 
                    Transmission = "A",
                    FuelConsumption = 7.0, 
                    Mileage = 22000, 
                    DailyRate = 35.00m, 
                    PhotoName = "civic.jpg", 
                    Description = "Fuel-efficient compact car ideal for city driving.", 
                    IsAvailable = true, 
                    LicensePlate = "XYZ789", 
                    Seats = 5, 
                    Category = "Compact" 
                },
 

                new Car { 
                    Id = "bmw3", 
                    Brand = "BMW", 
                    Model = "3 Series", 
                    Year = 2022, 
                    Color = "White", 
                    Transmission = "A",
                    FuelConsumption= 9.2, 
                    Mileage = 12000, 
                    DailyRate = 85.00m, 
                    PhotoName = "bmw3.jpg", 
                    Description = "Luxury sedan with premium features and excellent performance.", 
                    IsAvailable = true, 
                    LicensePlate = "GHI789", 
                    Seats = 5, 
                    Category = "Luxury" 
                },

                new Car { 
                    Id = "altima", 
                    Brand = "Nissan", 
                    Model = "Altima", 
                    Year = 2021, 
                    Color = "Red", 
                    Transmission = "A",
                      FuelConsumption = 8.5,
                    Mileage = 18000, 
                    DailyRate = 40.00m, 
                    PhotoName = "altima.jpg", 
                    Description = "Comfortable mid-size sedan with great fuel economy.", 
                    IsAvailable = false, 
                    LicensePlate = "JKL012", 
                    Seats = 5, 
                    Category = "Mid-size" 
                },
                new Car { 
                    Id = "spark", 
                    Brand = "Chevrolet", 
                    Model = "Spark", 
                    Year = 2020, 
                    Color = "Green", 
                    Transmission = "M",
                    FuelConsumption = 6.5,
                    Mileage = 25000, 
                    DailyRate = 25.00m, 
                    PhotoName = "spark.jpg", 
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