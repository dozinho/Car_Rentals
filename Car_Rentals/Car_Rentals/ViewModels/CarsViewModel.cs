using Car_Rentals.Models;
using Car_Rentals.Services;
using Car_Rentals.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Car_Rentals.ViewModels
{
    public class CarsViewModel : BaseViewModel
    {
        private readonly ICarDataStore _carDataStore;
        
        public ObservableCollection<Car> Cars { get; }
        public Command LoadCarsCommand { get; }
        public Command<Car> CarTapped { get; }
        public Command FilterCommand { get; }
        public Command SearchCommand { get; }
        
        public string SelectedCategory { get; set; }
        public string SearchText { get; set; }
        public bool ShowOnlyAvailable { get; set; } = true;

        public CarsViewModel()
        {
            Title = "Available Cars";
            Cars = new ObservableCollection<Car>();
            LoadCarsCommand = new Command(async () => await ExecuteLoadCarsCommand());
            CarTapped = new Command<Car>(OnCarSelected);
            FilterCommand = new Command(async () => await ExecuteLoadCarsCommand());
            SearchCommand = new Command(async () => await ExecuteLoadCarsCommand());
            
            _carDataStore = DependencyService.Get<ICarDataStore>();
        }

        async Task ExecuteLoadCarsCommand()
        {
            IsBusy = true;

            try
            {
                Cars.Clear();
                IEnumerable<Car> cars;
                
                if (ShowOnlyAvailable)
                {
                    cars = await _carDataStore.GetAvailableCarsAsync();
                }
                else
                {
                    cars = await _carDataStore.GetCarsAsync(true);
                }

                // Apply category filter
                if (!string.IsNullOrWhiteSpace(SelectedCategory))
                {
                    cars = await _carDataStore.GetCarsByCategoryAsync(SelectedCategory);
                }

                // Apply search filter
                if (!string.IsNullOrWhiteSpace(SearchText))
                {
                    var searchLower = SearchText.ToLower();
                    cars = cars.Where(c => 
                        c.Brand.ToLower().Contains(searchLower) ||
                        c.Model.ToLower().Contains(searchLower) ||
                        c.Color.ToLower().Contains(searchLower));
                }

                foreach (var car in cars)
                {
                    Cars.Add(car);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void OnAppearing()
        {
            IsBusy = true;
        }

        async void OnCarSelected(Car car)
        {
            if (car == null)
                return;

            // This will push the CarDetailPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(CarDetailPage)}?{nameof(CarDetailViewModel.CarId)}={car.Id}");
        }
    }
} 