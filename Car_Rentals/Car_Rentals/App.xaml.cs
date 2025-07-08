using Car_Rentals.Services;
using Car_Rentals.ViewModels;
using Car_Rentals.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Car_Rentals
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            // Register services
            DependencyService.Register<ICarDataStore, MockCarDataStore>(); // Mock data store for cars
            DependencyService.Register<IAuthService, MockAuthService>(); // Mock authentication service
           //  DependencyService.Register<IRentalDataStore, IRentalDataStore>();


            // MainPage = new AppShell();
            MainPage = new NavigationPage(new WelcomePage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
