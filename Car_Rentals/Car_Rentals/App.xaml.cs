using Car_Rentals.Services;
using Car_Rentals.ViewModels;
using Car_Rentals.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Car_Rentals.Models;

namespace Car_Rentals
{
    public partial class App : Application
    {

        public App()
        {
            SQLitePCL.Batteries_V2.Init(); // Ensure SQLite is initialized
            InitializeComponent();

            // Register services
            DependencyService.Register<ICarDataStore, EfCarDataStore>(); // EF Core data store for cars
            DependencyService.Register<IAuthService, EfAuthService>(); // EF Core authentication service
            DependencyService.Register<IDataStore<Item>, EfDataStore>(); // EF Core data store for items
            DependencyService.Register<IRentalDataStore, EfRentalDataStore>(); // EF Core data store for rentals
            DependencyService.Register<ICustomerDataStore, EfCustomerDataStore>(); // EF Core data store for customers
            DependencyService.Register<IUserDataStore, EfUserDataStore>(); // EF Core data store for users


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
