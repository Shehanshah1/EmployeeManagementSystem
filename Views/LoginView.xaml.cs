
using EmployeeManagementSystem.Models;
using EmployeeManagementSystem.Services;
using Microsoft.Maui.ApplicationModel.Communication;
using System.Text.RegularExpressions;

namespace EmployeeManagementSystem.Views
{
    public partial class LoginView : ContentPage
    {
        private bool _isPasswordVisible = false;
        private readonly DatabaseService _databaseService;

        public LoginView()
        {
            _databaseService = new DatabaseService();
            InitializeComponent();
            LoadRememberedCredentials();
            AddDefaultLoginCredentialsAsync();
        }

        private async void OnLoginButtonClicked(object sender, EventArgs e)
        {
            // Disable login button to prevent multiple clicks
            loginButton.IsEnabled = false;
            errorMessageLabel.IsVisible = false; // Hide previous error message

            string enteredEmail = emailEntry.Text?.Trim();
            string enteredPassword = passwordEntry.Text;

            // Validate email format
            if (string.IsNullOrEmpty(enteredEmail) || !IsValidEmail(enteredEmail))
            {
                await DisplayAlert("Invalid Input", "Please enter a valid email address.", "OK");
                loginButton.IsEnabled = true;
                return;
            }

            // Validate password field
            if (string.IsNullOrEmpty(enteredPassword))
            {
                await DisplayAlert("Invalid Input", "Password cannot be empty.", "OK");
                loginButton.IsEnabled = true;
                return;
            }

            // Simulate backend processing delay
            await Task.Delay(1000);

            // Check credentials (for demonstration purposes; replace with actual authentication)
            var userAccount = await _databaseService.AuthenticateUserAsync(enteredEmail);

            if (userAccount!=null && userAccount.PasswordHash==enteredPassword)
            {
                if (rememberMeCheckBox.IsChecked)
                {
                    SaveEmail(enteredEmail);
                }
                else
                {
                    ClearSavedEmail();
                }
                Preferences.Set("CurrentUserSessionId", userAccount.UserID);
                Preferences.Set("CurrentUserEmail", userAccount.Email);
                await App.NavigateToPage(new Dashboard());
            }
            else
            {
                errorMessageLabel.Text = "Incorrect email or password.";
                errorMessageLabel.IsVisible = true;
            }

            // Re-enable login button after processing
            loginButton.IsEnabled = true;
        }

        private async void OnCreateAccountButtonClicked(object sender, EventArgs e)
        {
            await App.NavigateToPage(new CreateAccount());
        }



        private async void OnForgotPasswordButtonClicked(object sender, EventArgs e)
        {
            await App.NavigateToPage(new ForgotPassword());
        }
        private void OnShowHidePasswordButtonClicked(object sender, EventArgs e)
        {
            _isPasswordVisible = !_isPasswordVisible;
            passwordEntry.IsPassword = !_isPasswordVisible;
            showHidePasswordButton.Text = _isPasswordVisible ? "Hide" : "Show";
        }

        private bool IsValidEmail(string email)
        {
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern);
        }

        private void LoadRememberedCredentials()
        {
            if (Preferences.ContainsKey("RememberedEmail"))
            {
                emailEntry.Text = Preferences.Get("RememberedEmail", string.Empty);
                rememberMeCheckBox.IsChecked = true;
            }
        }
        private async void AddDefaultLoginCredentialsAsync()
        {
            var existingUsers = await _databaseService.GetUserAccountsAsync();
            if (existingUsers.Count == 0)
            {
                var defaultUser = new UserAccount
                {
                    Email = "user@example.com",
                    PasswordHash = "password123",
                    Role = "User"
                };
                await _databaseService.AddUserAccountAsync(defaultUser);
            }
        }
        private void SaveEmail(string email)
        {
            Preferences.Set("RememberedEmail", email);
        }

        private void ClearSavedEmail()
        {
            Preferences.Remove("RememberedEmail");
        }
    }
}
