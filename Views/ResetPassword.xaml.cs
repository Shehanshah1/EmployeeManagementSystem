namespace EmployeeManagementSystem.ResetPassword;

public partial class ResetPassword : ContentPage
{
    public ResetPassword()
    {
        InitializeComponent();
    }

    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        // Navigate back to ForgotPassword page
        await Shell.Current.GoToAsync("//ForgotPassword");
    }

    private async void OnSubmitButtonClicked(object sender, EventArgs e)
    {
        // Get the entered code from the Entry control
        string resetCode = resetCodeEntry.Text; // Ensure you name the Entry control accordingly

        // Validate if the code is entered
        if (string.IsNullOrEmpty(resetCode))
        {
            await DisplayAlert("Error", "Please enter the reset code.", "OK");
            return;
        }

        // Example: Validate the code (you can replace this with a backend validation)
        if (resetCode == "123456") // Replace with actual validation logic
        {
            // Code is valid, navigate to the page where the user can reset their password
            await Shell.Current.GoToAsync("//NewPassword");
        }
        else
        {
            await DisplayAlert("Error", "Invalid reset code. Please try again.", "OK");
        }
    }
}
