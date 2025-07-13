using Car_Rentals.Models;
using Car_Rentals.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Car_Rentals.ViewModels
{
    public class UserListViewModel : BaseViewModel
    {
        public ObservableCollection<User> Users { get; set; } = new ObservableCollection<User>();
        private readonly IUserDataStore _userDataStore;

        public UserListViewModel()
        {
            Title = "User List";
            _userDataStore = DependencyService.Get<IUserDataStore>();
            LoadUsersCommand = new Command(async () => await LoadUsersAsync());
            LoadUsersCommand.Execute(null);
        }

        public Command LoadUsersCommand { get; }

        private async Task LoadUsersAsync()
        {
            Users.Clear();
            var users = await _userDataStore.GetUsersAsync();
            foreach (var user in users)
            {
                Users.Add(user);
            }
        }
    }
} 