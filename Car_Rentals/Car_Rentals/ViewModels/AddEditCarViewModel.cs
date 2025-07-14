using Car_Rentals.Models;
using Car_Rentals.Services;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.IO;
using System;

namespace Car_Rentals.ViewModels
{
    public class AddEditCarViewModel : BaseViewModel
    {
        private readonly ICarDataStore _carDataStore;
        public Car Car { get; set; }
        public string PageTitle { get; set; }
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand PickPhotoCommand { get; }
        private bool isEditMode;

        public AddEditCarViewModel(Car car = null)
        {
            _carDataStore = DependencyService.Get<ICarDataStore>();
            if (car != null)
            {
                Car = new Car
                {
                    Id = car.Id,
                    Brand = car.Brand,
                    Model = car.Model,
                    Year = car.Year,
                    Color = car.Color,
                    Transmission = car.Transmission,
                    FuelConsumption = car.FuelConsumption,
                    Mileage = car.Mileage,
                    DailyRate = car.DailyRate,
                    PhotoName = car.PhotoName,
                    Description = car.Description,
                    IsAvailable = car.IsAvailable,
                    LicensePlate = car.LicensePlate,
                    Seats = car.Seats,
                    Category = car.Category
                };
                PageTitle = "Edit Car";
                isEditMode = true;
            }
            else
            {
                Car = new Car();
                PageTitle = "Add New Car";
                isEditMode = false;
            }
            SaveCommand = new Command(async () => await OnSave());
            CancelCommand = new Command(async () => await OnCancel());
            PickPhotoCommand = new Command(async () => await OnPickPhoto());
        }

        private async Task OnSave()
        {
            if (isEditMode)
                await _carDataStore.UpdateCarAsync(Car);
            else
            {
                Car.Id = System.Guid.NewGuid().ToString();
                await _carDataStore.AddCarAsync(Car);
            }
            await Shell.Current.Navigation.PopAsync();
        }

        private async Task OnCancel()
        {
            await Shell.Current.Navigation.PopAsync();
        }

        private async Task OnPickPhoto()
        {
            try
            {
                var status = await Permissions.RequestAsync<Permissions.StorageRead>();
                if (status != PermissionStatus.Granted)
                {
                    if (DeviceInfo.Platform == DevicePlatform.Android && DeviceInfo.Version.Major >= 13)
                    {
                        await Application.Current.MainPage.DisplayAlert(
                            "Permission Required",
                            "Please grant 'Photos and Media' permission for this app in your device settings to pick a photo.",
                            "OK");
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert(
                            "Permission Denied",
                            "Storage permission is required to pick a photo.",
                            "OK");
                    }
                    return;
                }
                var result = await MediaPicker.PickPhotoAsync();
                if (result != null)
                {
                    // Save the picked photo to local app storage
                    var newFile = Path.Combine(FileSystem.AppDataDirectory, result.FileName);
                    using (var stream = await result.OpenReadAsync())
                    using (var newStream = File.OpenWrite(newFile))
                        await stream.CopyToAsync(newStream);
                    Car.PhotoName = newFile;
                    OnPropertyChanged(nameof(Car));
                    OnPropertyChanged(nameof(Car.PhotoName));
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Photo pick failed: {ex.Message}", "OK");
            }
        }
    }
} 