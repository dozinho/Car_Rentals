using System;

namespace Car_Rentals.Models
{
    public class Car
    {
        public string Id { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string Color { get; set; }
        public string Transmission { get; set; } // Manual, Automatic
        public double FuelConsumption { get; set; } 
        public int Mileage { get; set; }
        public decimal DailyRate { get; set; }
        public string PhotoName { get; set; }
        public string Description { get; set; }
        public bool IsAvailable { get; set; }
        public string LicensePlate { get; set; }
        public int Seats { get; set; }
        public string Category { get; set; } // Economy, Compact, Mid-size, Full-size, SUV, Luxury
    }
} 