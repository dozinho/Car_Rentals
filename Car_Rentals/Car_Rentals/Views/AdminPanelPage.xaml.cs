using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Car_Rentals.ViewModels;

namespace Car_Rentals.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AdminPanelPage : TabbedPage
    {
        public AdminPanelPage()
        {
            InitializeComponent();
            BindingContext = new AdminPanelViewModel();
        }
    }
} 