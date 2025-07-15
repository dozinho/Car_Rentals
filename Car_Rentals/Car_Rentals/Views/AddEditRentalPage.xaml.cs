using Car_Rentals.Models;
using Car_Rentals.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Car_Rentals.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddEditRentalPage : ContentPage
    {
        public AddEditRentalPage(Rental rental = null)
        {
            InitializeComponent();
            BindingContext = new AddEditRentalViewModel(rental);
        }
    }
}
