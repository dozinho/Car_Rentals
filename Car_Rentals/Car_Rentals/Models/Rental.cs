using System;

namespace Car_Rentals.Models
{
    public class Rental
    {
        public string Id { get; set; }
        public string CarId { get; set; }
        public string CustomerId { get; set; }
        public DateTime PickupDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public decimal TotalCost { get; set; }
        public string Status { get; set; } // Reserved, Active, Completed, Cancelled
        public DateTime CreatedDate { get; set; }
        public string PickupLocation { get; set; }
        public string ReturnLocation { get; set; }
        public string Notes { get; set; }
    }
} 