namespace EmployeeManagementSystem.LoginView;
using EmployeeManagementSystem.Dashboard;

public partial class LoginView : ContentPage
{
	public LoginView()
	{
        InitializeComponent();
    }

    private async void OnLoginButtonClicked(object sender, EventArgs e)
    {
        // Navigate to Dashboard after login
        await Shell.Current.GoToAsync("//Dashboard");
    }



}