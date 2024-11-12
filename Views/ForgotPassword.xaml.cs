using System.ComponentModel;
using System.Text.RegularExpressions;
using EmployeeManagementSystem.Services; // Assuming your DB Service is here
using EmployeeManagementSystem.LoginView;

namespace EmployeeManagementSystem.ForgotPassword;

public partial class ForgotPassword : ContentPage, INotifyPropertyChanged
{
    public ForgotPassword()
    {
        InitializeComponent();
        BindingContext = this; // Set BindingContext to enable data binding
    }

    // Email Property
    private string _email;
    public string Email
    {
        get => _email;
        set
        {
            _email = value;
            OnPropertyChanged(nameof(Email));
        }
    }

    // Navigate Back to Login Page
    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//LoginView");
    }

    // Handle Submit Button Click
    private async void OnSubmitButtonClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(Email) || !IsValidEmail(Email))
        {
            statusLabel.Text = "Please enter a valid email address.";
            statusLabel.IsVisible = true;
            return;
        }

        // Check if email exists in the database
        var emailExists = await CheckIfEmailExists(Email);
        if (!emailExists)
        {
            statusLabel.Text = "Email not found. Please enter a registered email.";
            statusLabel.IsVisible = true;
            return;
        }

        statusLabel.IsVisible = false;
        await DisplayAlert("Success", $"An email has been sent to {Email} for password reset.", "OK");
        await Shell.Current.GoToAsync("//ResetPassword");
    }

    // Email Validation Method
    private bool IsValidEmail(string email)
    {
        var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        return Regex.IsMatch(email, emailPattern);
    }

    // Check if email exists in the database
    private async Task<bool> CheckIfEmailExists(string email)
    {
        try
        {
            var user = await App.Database.GetAllEmployeesAsync();
            return user != null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error checking email: {ex.Message}");
            return false;
        }
    }

    // Implement INotifyPropertyChanged
    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
