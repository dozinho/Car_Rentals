using Car_Rentals.Models;
using Car_Rentals.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Car_Rentals.ViewModels
{
    public class AdminPanelViewModel : BaseViewModel
    {
        private readonly ICarDataStore _carDataStore;
        public ObservableCollection<Car> Cars { get; set; }
        public Command AddCarCommand { get; }
        public Command<Car> EditCarCommand { get; }
        public Command<Car> DeleteCarCommand { get; }

        public AdminPanelViewModel()
        {
            Title = "Admin Panel";
            _carDataStore = DependencyService.Get<ICarDataStore>();
            Cars = new ObservableCollection<Car>();
            AddCarCommand = new Command(OnAddCar);
            EditCarCommand = new Command<Car>(OnEditCar);
            DeleteCarCommand = new Command<Car>(async (car) => await OnDeleteCar(car));
            LoadCars();
        }

        private async void LoadCars()
        {
            Cars.Clear();
            var cars = await _carDataStore.GetCarsAsync();
            foreach (var car in cars)
                Cars.Add(car);
        }

        private void OnAddCar()
        {
            // TODO: Show add car UI
        }

        private void OnEditCar(Car car)
        {
            // TODO: Show edit car UI
        }

        private async Task OnDeleteCar(Car car)
        {
            if (car == null) return;
            var confirm = await Application.Current.MainPage.DisplayAlert("Delete Car", $"Are you sure you want to delete {car.Brand} {car.Model}?", "Yes", "No");
            if (!confirm) return;
            await _carDataStore.DeleteCarAsync(car.Id);
            LoadCars();
        }
    }
} 