using Car_Rentals.ViewModels;
using Xamarin.Forms;

namespace Car_Rentals.Views
{
    public partial class CarDetailPage : ContentPage
    {
        CarDetailViewModel _viewModel;

        public CarDetailPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new CarDetailViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.LoadCarCommand.Execute(null);
        }
    }
} 