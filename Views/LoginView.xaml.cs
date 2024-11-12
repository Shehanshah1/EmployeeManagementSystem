using EmployeeManagementSystem.Dashboard;
using EmployeeManagementSystem.ForgotPassword;
using EmployeeManagementSystem.CreateAccount;
using System.Text.RegularExpressions;
using Microsoft.Maui.Controls;
using System;
using System.Threading.Tasks;

namespace EmployeeManagementSystem.LoginView
{
    public partial class LoginView : ContentPage
    {
        private bool _isPasswordVisible = false;

        public LoginView()
        {
            InitializeComponent();
            LoadRememberedCredentials();
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
            string correctEmail = "user@example.com";
            string correctPassword = "password123";

            if (enteredEmail == correctEmail && enteredPassword == correctPassword)
            {
                if (rememberMeCheckBox.IsChecked)
                {
                    SaveEmail(enteredEmail);
                }
                else
                {
                    ClearSavedEmail();
                }

                await Shell.Current.GoToAsync("//Dashboard");
            }
            else
            {
                errorMessageLabel.Text = "Incorrect email or password.";
                errorMessageLabel.IsVisible = true;
            }

            // Re-enable login button after processing
            loginButton.IsEnabled = true;
        }

        private async void OnForgotPasswordButtonClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//ForgotPassword");
        }

        private async void OnCreateAccountButtonClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//CreateAccount");
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
