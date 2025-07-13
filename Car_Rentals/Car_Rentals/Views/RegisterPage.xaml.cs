using Car_Rentals.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Car_Rentals.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterPage : ContentPage
    {
        public RegisterPage()
        {
            InitializeComponent();
            BindingContext = new RegisterViewModel();
        }
    }
} 