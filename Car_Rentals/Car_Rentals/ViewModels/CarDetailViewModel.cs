using Car_Rentals.Models;
using Car_Rentals.Services;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Car_Rentals.ViewModels
{
    [QueryProperty(nameof(CarId), nameof(CarId))]
    public class CarDetailViewModel : BaseViewModel
    {
        private readonly ICarDataStore _carDataStore;
        private readonly IAuthService _authService;
        
        private string carId;
        public string CarId
        {
            get => carId;
            set
            {
                carId = value;
                LoadCarCommand.Execute(null);
            }
        }
        private Car car;
        public Car Car
        {
            get => car;
            set
            {
                car = value;
                OnPropertyChanged(nameof(Car));
            }
        }
        public Command LoadCarCommand { get; }
        public Command RentCarCommand { get; }
        public Command AddToFavoritesCommand { get; }
        
        public DateTime PickupDate { get; set; } = DateTime.Now.AddDays(1);
        public DateTime ReturnDate { get; set; } = DateTime.Now.AddDays(2);
        public decimal TotalCost { get; set; }
        public string PickupLocation { get; set; } = "Main Office";
        public string ReturnLocation { get; set; } = "Main Office";

        public CarDetailViewModel()
        {
            LoadCarCommand = new Command(async () => await ExecuteLoadCarCommand());
            RentCarCommand = new Command(async () => await ExecuteRentCarCommand());
            AddToFavoritesCommand = new Command(async () => await ExecuteAddToFavoritesCommand());
            
            _carDataStore = DependencyService.Get<ICarDataStore>();
            _authService = DependencyService.Get<IAuthService>();
        }

        async Task ExecuteLoadCarCommand()
        {
            IsBusy = true;

            try
            {
                Car = await _carDataStore.GetCarAsync(CarId);
                if (Car != null)
                {
                    Title = $"{Car.Brand} {Car.Model}";
                    CalculateTotalCost();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        async Task ExecuteRentCarCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                // Check if user is authenticated
                if (!await _authService.IsAuthenticatedAsync())
                {
                    await Application.Current.MainPage.DisplayAlert("Login Required", "Please log in to rent a car.", "OK");
                    return;
                }

                // Validate dates
                if (PickupDate >= ReturnDate)
                {
                    await Application.Current.MainPage.DisplayAlert("Invalid Dates", "Return date must be after pickup date.", "OK");
                    return;
                }

                if (PickupDate < DateTime.Today)
                {
                    await Application.Current.MainPage.DisplayAlert("Invalid Date", "Pickup date cannot be in the past.", "OK");
                    return;
                }

                // Check if car is still available
                if (!Car.IsAvailable)
                {
                    await Application.Current.MainPage.DisplayAlert("Car Not Available", "This car is no longer available for rent.", "OK");
                    return;
                }

                // Create rental
                var customer = await _authService.GetCurrentCustomerAsync();
                var rental = new Rental
                {
                    Id = Guid.NewGuid().ToString(),
                    CarId = Car.Id,
                    CustomerId = customer.Id,
                    PickupDate = PickupDate,
                    ReturnDate = ReturnDate,
                    TotalCost = TotalCost,
                    Status = "Reserved",
                    CreatedDate = DateTime.Now,
                    PickupLocation = PickupLocation,
                    ReturnLocation = ReturnLocation
                };

                // TODO: Add rental to data store
                // await _rentalDataStore.AddRentalAsync(rental);

                // Update car availability
                await _carDataStore.UpdateCarAvailabilityAsync(Car.Id, false);

                await Application.Current.MainPage.DisplayAlert("Success", 
                    $"Car rented successfully! Total cost: ${TotalCost:F2}", "OK");
                
                // Navigate back to cars list
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await Application.Current.MainPage.DisplayAlert("Error", "An error occurred while processing your rental.", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        async Task ExecuteAddToFavoritesCommand()
        {
            // TODO: Implement favorites functionality
            await Application.Current.MainPage.DisplayAlert("Favorites", "Added to favorites!", "OK");
        }

        private void CalculateTotalCost()
        {
            if (Car != null)
            {
                var days = (ReturnDate - PickupDate).Days;
                TotalCost = Car.DailyRate * days;
            }
        }
    }
} 