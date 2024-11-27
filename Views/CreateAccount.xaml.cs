namespace EmployeeManagementSystem.Views;

public partial class CreateAccount : ContentPage
{
    public CreateAccount()
    {
        InitializeComponent();
    }

    private async void OnCreateAccountButtonClicked(object sender, EventArgs e)
    {
        // Logic for creating account goes here
        // Example: await CreateAccountAsync();

        // Navigate to Dashboard after creating account
        await App.NavigateToPage(new Dashboard());
    }

    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        // Navigate to Dashboard after login
        await App.NavigateToPage(new LoginView());
    }
}
