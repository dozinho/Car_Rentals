using Car_Rentals.Models;
using Car_Rentals.Services;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Car_Rentals.ViewModels
{
    public class AddEditRentalViewModel : BaseViewModel
    {
        private readonly IRentalDataStore _rentalDataStore;
        public Rental Rental { get; set; }
        public ObservableCollection<string> StatusOptions { get; set; }
        public string PageTitle { get; set; }
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public AddEditRentalViewModel(Rental rental = null)
        {
            _rentalDataStore = DependencyService.Get<IRentalDataStore>();
            StatusOptions = new ObservableCollection<string> { "Reserved", "Active", "Completed", "Cancelled" };
            Rental = rental != null ? new Rental
            {
                Id = rental.Id,
                CarId = rental.CarId,
                CustomerId = rental.CustomerId,
                PickupDate = rental.PickupDate,
                ReturnDate = rental.ReturnDate,
                TotalCost = rental.TotalCost,
                Status = rental.Status,
                CreatedDate = rental.CreatedDate,
                PickupLocation = rental.PickupLocation,
                ReturnLocation = rental.ReturnLocation,
                Notes = rental.Notes
            } : new Rental { PickupDate = DateTime.Now, ReturnDate = DateTime.Now.AddDays(1), Status = "Reserved", CreatedDate = DateTime.Now };
            PageTitle = rental == null ? "Add Rental" : "Edit Rental";
            SaveCommand = new Command(async () => await OnSave());
            CancelCommand = new Command(OnCancel);
        }

        private async Task OnSave()
        {
            if (string.IsNullOrWhiteSpace(Rental.CarId) || string.IsNullOrWhiteSpace(Rental.CustomerId))
            {
                await Application.Current.MainPage.DisplayAlert("Validation Error", "Car ID and Customer ID are required.", "OK");
                return;
            }
            bool result;
            if (string.IsNullOrEmpty(Rental.Id))
            {
                Rental.Id = Guid.NewGuid().ToString();
                result = await _rentalDataStore.AddRentalAsync(Rental);
            }
            else
            {
                result = await _rentalDataStore.UpdateRentalAsync(Rental);
            }
            if (result)
            {
                await Application.Current.MainPage.DisplayAlert("Success", "Rental saved successfully.", "OK");
                await Application.Current.MainPage.Navigation.PopAsync();
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Failed to save rental.", "OK");
            }
        }

        private void OnCancel()
        {
            Application.Current.MainPage.Navigation.PopAsync();
        }
    }
}
