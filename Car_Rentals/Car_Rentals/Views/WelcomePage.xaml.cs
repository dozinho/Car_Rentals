using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Car_Rentals.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class WelcomePage : ContentPage
	{
		public WelcomePage ()
		{
			InitializeComponent ();
		}

        private void OnLetsGoClicked(object sender, EventArgs e)
        {
            // Navigate to another page
          // Application.Current.MainPage = new NavigationPage(new CarsPage());
          //  Application.Current.MainPage = new NavigationPage(new LoginPage());
             Application.Current.MainPage = new AppShell();
        }
    }
}