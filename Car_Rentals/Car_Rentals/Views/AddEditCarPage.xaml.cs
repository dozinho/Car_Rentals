using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Car_Rentals.Models;
using Car_Rentals.ViewModels;

namespace Car_Rentals.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddEditCarPage : ContentPage
    {
        public AddEditCarPage(Car car = null)
        {
            InitializeComponent();
            BindingContext = new AddEditCarViewModel(car);
        }
    }
} 