namespace EmployeeManagementSystem.Views;

public partial class CreateAccount : ContentPage
{
	public CreateAccount()
	{
		InitializeComponent();
	}
    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        // Navigate to Dashboard after login
        await App.NavigateToPage(new LoginView());
    }
}