using Car_Rentals.Models;
using Car_Rentals.Services;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Car_Rentals.ViewModels
{
    public class MyRentalsViewModel : BaseViewModel
    {
        private readonly IRentalDataStore _rentalDataStore;
        private readonly ICarDataStore _carDataStore;
        private readonly IAuthService _authService;
        
        public ObservableCollection<RentalItem> Rentals { get; }
        public Command LoadRentalsCommand { get; }
        public Command<RentalItem> RentalTapped { get; }
        public Command RefreshCommand { get; }
        public Command BrowseCarsCommand { get; }
        public Command<RentalItem> CancelRentalCommand { get; }

        public MyRentalsViewModel()
        {
            Title = "My Rentals";
            Rentals = new ObservableCollection<RentalItem>();
            LoadRentalsCommand = new Command(async () => await ExecuteLoadRentalsCommand());
            RentalTapped = new Command<RentalItem>(OnRentalSelected);
            RefreshCommand = new Command(async () => await ExecuteLoadRentalsCommand());
            BrowseCarsCommand = new Command(async () => await ExecuteBrowseCarsCommand());
            CancelRentalCommand = new Command<RentalItem>(async (item) => await ExecuteCancelRentalCommand(item));
            
            _rentalDataStore = DependencyService.Get<IRentalDataStore>();
            _carDataStore = DependencyService.Get<ICarDataStore>();
            _authService = DependencyService.Get<IAuthService>();
        }

        async Task ExecuteLoadRentalsCommand()
        {
            IsBusy = true;

            try
            {
                Rentals.Clear();
                
                var currentUser = await _authService.GetCurrentUserAsync();
                if (currentUser == null)
                {
                    // Show message for guest users
                    return;
                }

                var currentCustomer = await _authService.GetCurrentCustomerAsync();
                if (currentCustomer == null)
                {
                    return;
                }

                var userRentals = await _rentalDataStore.GetRentalsByCustomerAsync(currentCustomer.Id);
                
                foreach (var rental in userRentals.OrderByDescending(r => r.PickupDate))
                {
                    // Skip cancelled rentals
                    if (rental.Status == "Cancelled")
                        continue;
                    var car = await _carDataStore.GetCarAsync(rental.CarId);
                    if (car != null)
                    {
                        var rentalItem = new RentalItem
                        {
                            Rental = rental,
                            Car = car,
                            Status = GetRentalStatus(rental),
                            StatusColor = GetStatusColor(rental),
                            TotalCost = rental.TotalCost,
                            Duration = $"{(rental.ReturnDate - rental.PickupDate).Days} days"
                        };
                        
                        Rentals.Add(rentalItem);
                    }
                }

                if (!Rentals.Any())
                {
                    // Show empty state message
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await Application.Current.MainPage.DisplayAlert("Error", "Failed to load rentals", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        async Task ExecuteBrowseCarsCommand()
        {
            await Shell.Current.GoToAsync("//CarsPage");
        }

        private string GetRentalStatus(Rental rental)
        {
            var now = DateTime.Now;
            
            if (rental.PickupDate > now)
                return "Upcoming";
            else if (rental.ReturnDate < now)
                return "Completed";
            else
                return "Active";
        }

        private string GetStatusColor(Rental rental)
        {
            var status = GetRentalStatus(rental);
            return status switch
            {
                "Upcoming" => "#FFA500", // Orange
                "Active" => "#4CAF50",   // Green
                "Completed" => "#2196F3", // Blue
                _ => "#9E9E9E"           // Gray
            };
        }

        public void OnAppearing()
        {
            IsBusy = true;
        }

        async void OnRentalSelected(RentalItem rentalItem)
        {
            if (rentalItem == null)
                return;

            // Show rental details
            await Application.Current.MainPage.DisplayAlert("Rental Details", 
                $"Car: {rentalItem.Car.Brand} {rentalItem.Car.Model}\n" +
                $"Pickup: {rentalItem.Rental.PickupDate:MMM dd, yyyy}\n" +
                $"Return: {rentalItem.Rental.ReturnDate:MMM dd, yyyy}\n" +
                $"Duration: {rentalItem.Duration}\n" +
                $"Total Cost: ${rentalItem.TotalCost:F2}\n" +
                $"Status: {rentalItem.Status}", "OK");
        }

        private async Task ExecuteCancelRentalCommand(RentalItem rentalItem)
        {
            if (rentalItem == null || rentalItem.Status != "Upcoming")
                return;
            var confirm = await Application.Current.MainPage.DisplayAlert(
                "Cancel Rental",
                $"Are you sure you want to cancel your rental for {rentalItem.Car.Brand} {rentalItem.Car.Model}?",
                "Yes", "No");
            if (!confirm) return;
            var result = await _rentalDataStore.CancelRentalAsync(rentalItem.Rental.Id);
            if (result)
            {
                // Mark the car as available again
                await _carDataStore.UpdateCarAvailabilityAsync(rentalItem.Car.Id, true);
                await Application.Current.MainPage.DisplayAlert("Cancelled", "Your rental has been cancelled.", "OK");
                await ExecuteLoadRentalsCommand();
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Failed to cancel rental.", "OK");
            }
        }
    }

    public class RentalItem
    {
        public Rental Rental { get; set; }
        public Car Car { get; set; }
        public string Status { get; set; }
        public string StatusColor { get; set; }
        public decimal TotalCost { get; set; }
        public string Duration { get; set; }
    }
} 