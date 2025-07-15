using System;
using Car_Rentals.Models;
using Car_Rentals.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Linq;
using Xamarin.Forms;

namespace Car_Rentals.ViewModels
{
    public class AdminPanelViewModel : BaseViewModel
    {
        private readonly ICarDataStore _carDataStore;
        private readonly IUserDataStore _userDataStore;
        private readonly IRentalDataStore _rentalDataStore;
        public ObservableCollection<Car> Cars { get; set; }
        public ObservableCollection<User> Users { get; set; }
        public class RentalDisplay
        {
            public string Id { get; set; }
            public string CarPhoto { get; set; }
            public string CarBrand { get; set; }
            public string CarModel { get; set; }
            public string Username { get; set; }
            public string CustomerId { get; set; }
            public DateTime PickupDate { get; set; }
            public DateTime ReturnDate { get; set; }
            public string Status { get; set; }
        }
        public ObservableCollection<RentalDisplay> Rentals { get; set; }
        public Command AddCarCommand { get; }
        public Command<Car> EditCarCommand { get; }
        public Command<Car> DeleteCarCommand { get; }
        public Command AddUserCommand { get; }
        public Command<User> EditUserCommand { get; }
        public Command<User> DeleteUserCommand { get; }
        public Command AddRentalCommand { get; }
        public Command<RentalDisplay> EditRentalCommand { get; }
        public Command<RentalDisplay> DeleteRentalCommand { get; }

        public AdminPanelViewModel()
        {
            Title = "Admin Panel";
            _carDataStore = DependencyService.Get<ICarDataStore>();
            _userDataStore = DependencyService.Get<IUserDataStore>();
            Cars = new ObservableCollection<Car>();
            Users = new ObservableCollection<User>();
            Rentals = new ObservableCollection<RentalDisplay>();
            AddCarCommand = new Command(OnAddCar);
            EditCarCommand = new Command<Car>(OnEditCar);
            DeleteCarCommand = new Command<Car>(async (car) => await OnDeleteCar(car));
            AddUserCommand = new Command(OnAddUser);
            EditUserCommand = new Command<User>(OnEditUser);
            DeleteUserCommand = new Command<User>(async (user) => await OnDeleteUser(user));
            AddRentalCommand = new Command(OnAddRental);
            EditRentalCommand = new Command<RentalDisplay>(OnEditRental);
            DeleteRentalCommand = new Command<RentalDisplay>(async (rental) => await OnDeleteRental(rental));
            _rentalDataStore = DependencyService.Get<IRentalDataStore>();
            LoadCars();
            LoadUsers();
            LoadRentals();
        }

        private async void LoadCars()
        {
            Cars.Clear();
            var cars = await _carDataStore.GetCarsAsync();
            foreach (var car in cars)
                Cars.Add(car);
        }

        private async void OnAddCar()
        {
            await Shell.Current.Navigation.PushAsync(new Views.AddEditCarPage());
            LoadCars();
        }

        private async void OnEditCar(Car car)
        {
            if (car == null) return;
            await Shell.Current.Navigation.PushAsync(new Views.AddEditCarPage(car));
            LoadCars();
        }

        private async Task OnDeleteCar(Car car)
        {
            if (car == null) return;
            var confirm = await Application.Current.MainPage.DisplayAlert("Delete Car", $"Are you sure you want to delete {car.Brand} {car.Model}?", "Yes", "No");
            if (!confirm) return;
            await _carDataStore.DeleteCarAsync(car.Id);
            LoadCars();
        }

        private async void LoadUsers()
        {
            Users.Clear();
            var users = await _userDataStore.GetUsersAsync();
            foreach (var user in users)
                Users.Add(user);
        }

        private async void OnAddUser()
        {
            await Shell.Current.Navigation.PushAsync(new Views.AddEditUserPage());
            LoadUsers();
        }

        private async void OnEditUser(User user)
        {
            if (user == null) return;
            await Shell.Current.Navigation.PushAsync(new Views.AddEditUserPage(user));
            LoadUsers();
        }

        private async Task OnDeleteUser(User user)
        {
            if (user == null) return;
            var confirm = await Application.Current.MainPage.DisplayAlert("Delete User", $"Are you sure you want to delete {user.Username}?", "Yes", "No");
            if (!confirm) return;
            await _userDataStore.DeleteUserAsync(user.Id);
            LoadUsers();
        }
        private async void LoadRentals()
        {
            Rentals.Clear();
            var rentals = (await _rentalDataStore.GetRentalsAsync()).ToList();
            var cars = (await _carDataStore.GetCarsAsync()).ToList();
            var users = (await _userDataStore.GetUsersAsync()).ToList();
            foreach (var rental in rentals)
            {
                var car = cars.FirstOrDefault(c => c.Id == rental.CarId);
                var user = users.FirstOrDefault(u => u.Id == rental.CustomerId || u.CustomerId == rental.CustomerId);
                Rentals.Add(new RentalDisplay
                {
                    Id = rental.Id,
                    CarPhoto = car?.PhotoName,
                    CarBrand = car?.Brand,
                    CarModel = car?.Model,
                    Username = user?.Username,
                    CustomerId = rental.CustomerId,
                    PickupDate = rental.PickupDate,
                    ReturnDate = rental.ReturnDate,
                    Status = rental.Status
                });
            }
        }

        private async void OnAddRental()
        {
            await Shell.Current.Navigation.PushAsync(new Views.AddEditRentalPage());
            LoadRentals();
        }

        private async void OnEditRental(RentalDisplay rentalDisplay)
        {
            if (rentalDisplay == null) return;
            var rental = (await _rentalDataStore.GetRentalsAsync()).FirstOrDefault(r => r.Id == rentalDisplay.Id);
            if (rental == null) return;
            await Shell.Current.Navigation.PushAsync(new Views.AddEditRentalPage(rental));
            LoadRentals();
        }

        private async Task OnDeleteRental(RentalDisplay rentalDisplay)
        {
            if (rentalDisplay == null) return;
            var confirm = await Application.Current.MainPage.DisplayAlert("Delete Rental", $"Are you sure you want to delete this rental?", "Yes", "No");
            if (!confirm) return;
            await _rentalDataStore.DeleteRentalAsync(rentalDisplay.Id);
            LoadRentals();
        }
    }
}