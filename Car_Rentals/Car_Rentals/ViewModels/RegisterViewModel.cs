using Car_Rentals.Models;
using Car_Rentals.Services;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Car_Rentals.ViewModels
{
    public class RegisterViewModel : BaseViewModel
    {
        private readonly IAuthService _authService;
        
        // Personal Information
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string DateOfBirth { get; set; }
        
        // Address
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        
        // License
        public string LicenseNumber { get; set; }
        public string LicenseExpiry { get; set; }
        
        // Account
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        
        // UI
        public string ErrorMessage { get; set; }
        public bool IsErrorVisible { get; set; }
        
        public Command RegisterCommand { get; }
        public Command GoToLoginCommand { get; }

        public RegisterViewModel()
        {
            _authService = DependencyService.Get<IAuthService>();
            RegisterCommand = new Command(async () => await OnRegisterClicked());
            GoToLoginCommand = new Command(async () => await OnGoToLoginClicked());
        }

        private async Task OnRegisterClicked()
        {
            System.Diagnostics.Debug.WriteLine($"[DEBUG] RegisterCommand fired. FirstName: {FirstName}, LastName: {LastName}, Username: {Username}, Password: {Password}, ConfirmPassword: {ConfirmPassword}");
            if (IsBusy)
                return;

            IsBusy = true;
            IsErrorVisible = false;

            try
            {
                // Validate required fields
                if (!ValidateRequiredFields())
                {
                    System.Diagnostics.Debug.WriteLine("[DEBUG] Validation failed in OnRegisterClicked.");
                    return;
                }

                // Validate email format (optional)
                if (!string.IsNullOrWhiteSpace(Email) && !IsValidEmail(Email))
                {
                    ErrorMessage = "Please enter a valid email address.";
                    IsErrorVisible = true;
                    return;
                }

                // Validate password match
                if (Password != ConfirmPassword)
                {
                    ErrorMessage = "Passwords do not match.";
                    IsErrorVisible = true;
                    return;
                }

                // Validate password strength
                if (Password.Length < 6)
                {
                    ErrorMessage = "Password must be at least 6 characters long.";
                    IsErrorVisible = true;
                    return;
                }

                // Parse dates (optional)
                DateTime? dob = null;
                if (!string.IsNullOrWhiteSpace(DateOfBirth) && DateTime.TryParse(DateOfBirth, out DateTime dobVal))
                    dob = dobVal;
                DateTime? licenseExpiry = null;
                if (!string.IsNullOrWhiteSpace(LicenseExpiry) && DateTime.TryParse(LicenseExpiry, out DateTime licVal))
                    licenseExpiry = licVal;

                // Create customer
                var customer = new Customer
                {
                    FirstName = FirstName,
                    LastName = LastName,
                    Email = Email,
                    Phone = Phone,
                    DateOfBirth = dob ?? default,
                    Address = Address,
                    City = City,
                    State = State,
                    ZipCode = ZipCode,
                    LicenseNumber = LicenseNumber,
                    LicenseExpiryDate = licenseExpiry ?? default
                };

                // Create user
                var user = new User
                {
                    Username = Username,
                    Email = Email,
                    PasswordHash = Password // In real app, this should be hashed
                };

                // Register user
                var success = await _authService.RegisterAsync(user, customer);
                System.Diagnostics.Debug.WriteLine($"[DEBUG] RegisterAsync returned: {success}");
                if (success)
                {
                    await Application.Current.MainPage.DisplayAlert("Success", 
                        "Account created successfully! You can now login.", "OK");
                    
                    // Navigate to login page
                    await Shell.Current.GoToAsync("///LoginPage");
                }
                else
                {
                    ErrorMessage = "Registration failed. Username or email may already exist.";
                    IsErrorVisible = true;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "An error occurred during registration. Please try again.";
                IsErrorVisible = true;
                System.Diagnostics.Debug.WriteLine($"Registration error: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task OnGoToLoginClicked()
        {
            await Shell.Current.GoToAsync("///LoginPage");
        }

        private bool ValidateRequiredFields()
        {
            if (string.IsNullOrWhiteSpace(FirstName))
            {
                ErrorMessage = "First name is required.";
                IsErrorVisible = true;
                return false;
            }

            if (string.IsNullOrWhiteSpace(LastName))
            {
                ErrorMessage = "Last name is required.";
                IsErrorVisible = true;
                return false;
            }

            if (string.IsNullOrWhiteSpace(Username))
            {
                ErrorMessage = "Username is required.";
                IsErrorVisible = true;
                return false;
            }

            if (string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Password is required.";
                IsErrorVisible = true;
                return false;
            }

            if (string.IsNullOrWhiteSpace(ConfirmPassword))
            {
                ErrorMessage = "Please confirm your password.";
                IsErrorVisible = true;
                return false;
            }

            return true;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
} 