using Car_Rentals.ViewModels;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace Car_Rentals.Views
{
    public partial class MyRentalsPage : ContentPage
    {
        MyRentalsViewModel _viewModel;

        public MyRentalsPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new MyRentalsViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
            _viewModel.LoadRentalsCommand.Execute(null);
        }

        private void OnRentalSelected(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.Count > 0)
            {
                var rentalItem = e.CurrentSelection[0] as RentalItem;
                if (rentalItem != null)
                {
                    _viewModel.RentalTapped.Execute(rentalItem);
                }
                ((CollectionView)sender).SelectedItem = null;
            }
        }
    }
} 