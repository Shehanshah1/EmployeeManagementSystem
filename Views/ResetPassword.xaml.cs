namespace EmployeeManagementSystem.Views;

public partial class ResetPassword : ContentPage
{
    public ResetPassword()
    {
        InitializeComponent();
    }

    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        // Navigate back to ForgotPassword page
        await App.GoBack();
    }

    private async void OnSubmitButtonClicked(object sender, EventArgs e)
    {
        
        string resetCode = resetCodeEntry.Text; 

        // Validate if the code is entered
        if (string.IsNullOrEmpty(resetCode))
        {
            await DisplayAlert("Error", "Please enter the reset code.", "OK");
            return;
        }

        
        if (resetCode == "123456") // Replace with actual validation logic
        {
            // Code is valid, navigate to the page where the user can reset their password
            await App.NavigateToPage(new NewPass()); ;
        }
        else
        {
            await DisplayAlert("Error", "Invalid reset code. Please try again.", "OK");
        }
    }
}
