namespace EmployeeManagementSystem.ForgotPassword;
using EmployeeManagementSystem.LoginView;
using EmployeeManagementSystem.ResetPassword;
public partial class ForgotPassword : ContentPage
{
	public ForgotPassword()
	{
		InitializeComponent();
	}
    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        // Navigate to Dashboard after login
        await Shell.Current.GoToAsync("//LoginView");
    }

    private async void OnSubmitButtonClicked(object sender, EventArgs e)
    {
        // Navigate to ResetPassword after clicking submit
        await Shell.Current.GoToAsync("//ResetPassword");
    }
}
