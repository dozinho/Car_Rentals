using Car_Rentals.Models;
using Car_Rentals.ViewModels;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace Car_Rentals.Views
{
    public partial class CarsPage : ContentPage
    {
        CarsViewModel _viewModel;

        public CarsPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new CarsViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }

        private void OnCarSelected(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.Count > 0)
            {
                var car = e.CurrentSelection[0] as Car;
                if (car != null)
                {
                    _viewModel.CarTapped.Execute(car);
                }
                ((CollectionView)sender).SelectedItem = null;
            }
        }
    }
} 