namespace EmployeeManagementSystem.CreateAccount;
using EmployeeManagementSystem.LoginView;
public partial class CreateAccount : ContentPage
{
	public CreateAccount()
	{
		InitializeComponent();
	}
    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        // Navigate to Dashboard after login
        await Shell.Current.GoToAsync("//LoginView");
    }
}