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
        private readonly IUserDataStore _userDataStore;
        public ObservableCollection<Car> Cars { get; set; }
        public ObservableCollection<User> Users { get; set; }
        public Command AddCarCommand { get; }
        public Command<Car> EditCarCommand { get; }
        public Command<Car> DeleteCarCommand { get; }
        public Command AddUserCommand { get; }
        public Command<User> EditUserCommand { get; }
        public Command<User> DeleteUserCommand { get; }

        public AdminPanelViewModel()
        {
            Title = "Admin Panel";
            _carDataStore = DependencyService.Get<ICarDataStore>();
            _userDataStore = DependencyService.Get<IUserDataStore>();
            Cars = new ObservableCollection<Car>();
            Users = new ObservableCollection<User>();
            AddCarCommand = new Command(OnAddCar);
            EditCarCommand = new Command<Car>(OnEditCar);
            DeleteCarCommand = new Command<Car>(async (car) => await OnDeleteCar(car));
            AddUserCommand = new Command(OnAddUser);
            EditUserCommand = new Command<User>(OnEditUser);
            DeleteUserCommand = new Command<User>(async (user) => await OnDeleteUser(user));
            LoadCars();
            LoadUsers();
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
    }
}