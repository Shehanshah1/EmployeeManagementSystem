namespace EmployeeManagementSystem.LoginView;
using EmployeeManagementSystem.Dashboard;
using EmployeeManagementSystem.ForgotPassword;
using EmployeeManagementSystem.CreateAccount;

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


}
