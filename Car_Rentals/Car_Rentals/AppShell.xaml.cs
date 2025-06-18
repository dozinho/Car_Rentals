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

        public AppShell()
        {
            InitializeComponent();
            _authService = DependencyService.Get<IAuthService>();
            
            // Register routes for car-related pages
            Routing.RegisterRoute(nameof(CarDetailPage), typeof(CarDetailPage));
            Routing.RegisterRoute(nameof(CarsPage), typeof(CarsPage));
            Routing.RegisterRoute(nameof(MyRentalsPage), typeof(MyRentalsPage));
        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            try
            {
                await _authService.LogoutAsync();
                await Shell.Current.GoToAsync("//LoginPage");
            }
            catch
            {
                await DisplayAlert("Error", "An error occurred during logout.", "OK");
            }
        }
    }
}
