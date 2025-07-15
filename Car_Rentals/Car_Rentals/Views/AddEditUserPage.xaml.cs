using Car_Rentals.Models;
using Car_Rentals.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Car_Rentals.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddEditUserPage : ContentPage
    {
        public AddEditUserPage(User user = null)
        {
            InitializeComponent();
            BindingContext = new AddEditUserViewModel(user);
        }
    }
}
