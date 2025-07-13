using Microsoft.EntityFrameworkCore;
using Car_Rentals.Models;
using System;
using System.Linq;

namespace Car_Rentals
{
    public class CarRentalDbContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Rental> Rentals { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Item> Items { get; set; }

        public CarRentalDbContext()
        {
            try
            {
                this.Database.EnsureCreated();
                SeedData();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("DB INIT ERROR: " + ex);
                if (ex.InnerException != null)
                    System.Diagnostics.Debug.WriteLine("INNER: " + ex.InnerException);
                throw;
            }
        }

        private void SeedData()
        {
            // Only seed if database is empty
            if (Cars.Any() && Users.Any() && Customers.Any())
                return;

            // Seed Cars
            if (!Cars.Any())
            {
                var cars = new[]
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
                        Mileage = 18000,
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
                        PhotoName = "DodgeDurango.png",
                        Description = "The Dodge Durango is a powerful mid-size SUV with aggressive styling and three-row seating – ideal for family adventures or road trips with extra muscle.",
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
                        Description = "A full-size luxury SUV known for its spacious interior, premium comfort, and smooth V8 power – perfect for VIP family or executive rentals.",
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
                Cars.AddRange(cars);
            }

            // Seed Customers
            if (!Customers.Any())
            {
                var customers = new[]
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
                Customers.AddRange(customers);

                // Seed Users (after customers)
                if (!Users.Any())
                {
                    var users = new[]
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
                    Users.AddRange(users);
                }
            }

            SaveChanges();
            System.Diagnostics.Debug.WriteLine("[DB] Database seeded successfully");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var dbPath = System.IO.Path.Combine(
                System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), "car_rental.db");
            optionsBuilder.UseSqlite($"Filename={dbPath}");
            System.Diagnostics.Debug.WriteLine($"[DB] Android DB Path: {dbPath}");
        }
    }
}
