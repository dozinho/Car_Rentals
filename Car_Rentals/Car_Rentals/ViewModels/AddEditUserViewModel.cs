using Car_Rentals.Models;
using Car_Rentals.Services;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Car_Rentals.ViewModels
{
    public class AddEditUserViewModel : BaseViewModel
    {
        private readonly IUserDataStore _userDataStore;
        public User User { get; set; }
        public string PageTitle { get; set; }
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public INavigation Navigation { get; set; }

        public AddEditUserViewModel(User user = null)
        {
            _userDataStore = DependencyService.Get<IUserDataStore>();
            User = user != null ? new User
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                PasswordHash = user.PasswordHash,
                CustomerId = user.CustomerId,
                IsAdmin = user.IsAdmin,
                CreatedDate = user.CreatedDate,
                LastLoginDate = user.LastLoginDate,
                IsActive = user.IsActive
            } : new User { IsActive = true, CreatedDate = DateTime.Now };
            PageTitle = user == null ? "Add User" : "Edit User";
            SaveCommand = new Command(async () => await OnSave());
            CancelCommand = new Command(OnCancel);
        }

        private async Task OnSave()
        {
            if (string.IsNullOrWhiteSpace(User.Username) || string.IsNullOrWhiteSpace(User.Email))
            {
                await Application.Current.MainPage.DisplayAlert("Validation Error", "Username and Email are required.", "OK");
                return;
            }
            bool result;
            if (string.IsNullOrEmpty(User.Id))
            {
                User.Id = Guid.NewGuid().ToString();
                result = await _userDataStore.AddUserAsync(User);
            }
            else
            {
                result = await _userDataStore.UpdateUserAsync(User);
            }
            if (result)
            {
                await Application.Current.MainPage.DisplayAlert("Success", "User saved successfully.", "OK");
                await Application.Current.MainPage.Navigation.PopAsync();
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Failed to save user.", "OK");
            }
        }

        private void OnCancel()
        {
            Application.Current.MainPage.Navigation.PopAsync();
        }
    }
}
