namespace EmployeeManagementSystem.LoginView;
using EmployeeManagementSystem.Dashboard;
using EmployeeManagementSystem.ForgotPassword;
using EmployeeManagementSystem.CreateAccount;
using System.Text.RegularExpressions;
public partial class LoginView : ContentPage
{
	public LoginView()
	{
        InitializeComponent();
    }

    private async void OnLoginButtonClicked(object sender, EventArgs e)
    {
        // For simplicity, let's assume we are checking static credentials.
        string enteredEmail = emailEntry.Text; // Assuming emailEntry is the name of the email Entry
        string enteredPassword = passwordEntry.Text; // Assuming passwordEntry is the name of the password Entry

        // Sample credentials (just an example, don't use plain text passwords in production)
        string correctEmail = "user@example.com";
        string correctPassword = "password123";

        if (enteredEmail == correctEmail && enteredPassword == correctPassword)
        {
            // Successfully logged in, navigate to the Dashboard
            await Shell.Current.GoToAsync("//Dashboard");
        }
        else
        {
            // Show error message (for example, via an alert)
            await DisplayAlert("Login Failed", "Incorrect email or password. Please try again.", "OK");
        }
    }
    private async void OnForgotPasswordButtonClicked(object sender, EventArgs e)
    {
        // Navigate to Dashboard after login
        await Shell.Current.GoToAsync("//ForgotPassword");
    }

    private async void OnCreateAccountButtonClicked(object sender, EventArgs e)
    {
        // Navigate to Dashboard after login
        await Shell.Current.GoToAsync("//CreateAccount");
    }

    private bool IsValidEmail(string email)
    {
        string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        return Regex.IsMatch(email, pattern);
    }

}
