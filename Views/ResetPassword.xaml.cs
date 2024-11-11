namespace EmployeeManagementSystem.ResetPassword;

public partial class ResetPassword : ContentPage
{
	public ResetPassword()
	{
		InitializeComponent();
	}
    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        // Navigate to Dashboard after login
        await Shell.Current.GoToAsync("//ForgotPassword");
    }

    private async void OnSubmitButtonClicked(object sender, EventArgs e)
    {
        // Navigate to ResetPassword after clicking submit
        await Shell.Current.GoToAsync("//LoginView");
    }
}