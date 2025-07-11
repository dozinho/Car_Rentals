using Car_Rentals.Services;
using Car_Rentals.ViewModels;
using Car_Rentals.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Car_Rentals
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        private readonly IAuthService _authService;
        private FlyoutItem _loginFlyoutItem;
        private MenuItem _logoutMenuItem;

        public AppShell()
        {
            InitializeComponent();
            _authService = DependencyService.Get<IAuthService>();
            
            // Register routes for car-related pages
            Routing.RegisterRoute(nameof(CarDetailPage), typeof(CarDetailPage));
            Routing.RegisterRoute(nameof(CarsPage), typeof(CarsPage));
            Routing.RegisterRoute(nameof(MyRentalsPage), typeof(MyRentalsPage));
            Routing.RegisterRoute("LoginPage", typeof(Views.LoginPage));

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

            // Logout MenuItem
            _logoutMenuItem = new MenuItem
            {
                Text = "Logout"
            };
            _logoutMenuItem.Clicked += OnMenuItemClicked;
        }

        public void UpdateAuthMenuItems()
        {
            bool loggedIn = _authService?.IsLoggedIn ?? false;

            // Remove both if present
            if (Items.Contains(_loginFlyoutItem)) Items.Remove(_loginFlyoutItem);
            if (Items.Contains(_logoutMenuItem)) Items.Remove(_logoutMenuItem);

            if (loggedIn)
            {
                Items.Add(_logoutMenuItem);
            }
            else
            {
                Items.Add(_loginFlyoutItem);
            }
        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            try
            {
                await _authService.LogoutAsync();
                await Shell.Current.GoToAsync("LoginPage"); // Use relative route
                UpdateAuthMenuItems();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"An error occurred during logout: {ex.Message}", "OK");
            }
        }
    }
}
