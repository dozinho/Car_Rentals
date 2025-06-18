using Car_Rentals.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace Car_Rentals.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}