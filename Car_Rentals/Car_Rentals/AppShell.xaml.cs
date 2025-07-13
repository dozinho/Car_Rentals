using Car_Rentals.Services;
using Car_Rentals.ViewModels;
using Car_Rentals.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Linq; // Added for FirstOrDefault

namespace Car_Rentals
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        private readonly IAuthService _authService;
        private FlyoutItem _loginFlyoutItem;

        public AppShell()
        {
            InitializeComponent();
            _authService = DependencyService.Get<IAuthService>();
            
            // Register routes for car-related pages
            Routing.RegisterRoute(nameof(CarDetailPage), typeof(CarDetailPage));
            Routing.RegisterRoute(nameof(CarsPage), typeof(CarsPage));
            Routing.RegisterRoute(nameof(MyRentalsPage), typeof(MyRentalsPage));
            Routing.RegisterRoute("LoginPage", typeof(Views.LoginPage));
            Routing.RegisterRoute("RegisterPage", typeof(Views.RegisterPage));

            // Initialize dynamic menu items
            CreateDynamicMenuItems();
            UpdateAuthMenuItems();
        }

        private void CreateDynamicMenuItems()
        {
            // Login FlyoutItem
            _loginFlyoutItem = new FlyoutItem
            {
                Title = "Login",
                Icon = "login_icon.png",
                Items =
                {
                    new ShellContent
                    {
                        Route = "LoginPage",
                        ContentTemplate = new DataTemplate(typeof(Views.LoginPage))
                    }
                }
            };
        }

        public void UpdateAuthMenuItems()
        {
            // Remove old Login FlyoutItem
            var itemsToRemove = new List<ShellItem>();
            foreach (var item in Items.ToList())
            {
                if (item is FlyoutItem flyout && flyout.Title == "Login")
                    itemsToRemove.Add(flyout);
            }
            foreach (var item in itemsToRemove)
            {
                Items.Remove(item);
            }

            bool loggedIn = _authService?.IsLoggedIn ?? false;

            // Show/hide Login FlyoutItem
            if (!loggedIn)
            {
                Items.Add(_loginFlyoutItem);
            }
        }

        private async void OnLogoutMenuItemClicked(object sender, EventArgs e)
        {
            try
            {
                await _authService.LogoutAsync();
                UpdateAuthMenuItems();
                await Shell.Current.GoToAsync("//LoginPage");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"An error occurred during logout: {ex.Message}", "OK");
            }
        }
    }
}
