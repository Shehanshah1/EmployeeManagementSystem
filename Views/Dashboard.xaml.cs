namespace EmployeeManagementSystem.Dashboard;
using EmployeeManagementSystem.EmployeeManagement;
using EmployeeManagementSystem.LeaveRequests;
using EmployeeManagementSystem.AdminSettings;
using EmployeeManagementSystem.UserSettings;
using EmployeeManagementSystem.LoginView;
public partial class Dashboard : ContentPage
{
	public Dashboard()
	{
		InitializeComponent();
	}
    private async void OnEmployeeManagementButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//EmployeeManagement");
    }

    private async void OnLeaveRequestsButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//LeaveRequests");
    }
    private async void OnAdminSettingsButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//AdminSettings");
    }
    private async void OnUserSettingsButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//UserSettings");
    }

    private async void OnLogOutButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//LoginView");
    }
}