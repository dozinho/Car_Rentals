using Car_Rentals.Services;
using Car_Rentals.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Car_Rentals.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly IAuthService _authService;
        
        public string Username { get; set; }
        public string Password { get; set; }
        public string ErrorMessage { get; set; }
        public bool IsErrorVisible { get; set; }
        
        public Command LoginCommand { get; }
        public Command RegisterCommand { get; }

        public LoginViewModel()
        {
            _authService = DependencyService.Get<IAuthService>();
            LoginCommand = new Command(async () => await OnLoginClicked());
            RegisterCommand = new Command(async () => await OnRegisterClicked());
        }

        private async Task OnLoginClicked()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            IsErrorVisible = false;

            try
            {
                if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
                {
                    ErrorMessage = "Please enter both username and password.";
                    IsErrorVisible = true;
                    return;
                }

                var success = await _authService.LoginAsync(Username, Password);
                
                if (success)
                {
                    // Update nav menu
                    if (Application.Current.MainPage is AppShell shell)
                        shell.UpdateAuthMenuItems();

                    // Navigate to main app
                    await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");
                }
                else
                {
                    ErrorMessage = "Invalid username or password. Please try again.";
                    IsErrorVisible = true;
                }
            }
            catch
            {
                ErrorMessage = "An error occurred during login. Please try again.";
                IsErrorVisible = true;
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task OnRegisterClicked()
        {
            // For now, just show a message that registration is not implemented
            await Application.Current.MainPage.DisplayAlert("Registration", 
                "Registration feature is coming soon! Please use the demo credentials to login.", "OK");
        }
    }
}
