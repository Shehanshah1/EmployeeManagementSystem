namespace EmployeeManagementSystem.EmployeeManagement;
using EmployeeManagementSystem.Dashboard;
using EmployeeManagementSystem.LeaveRequests;
using EmployeeManagementSystem.Models;
using EmployeeManagementSystem.UserSettings;
public partial class EmployeeManagement : ContentPage
{
	public EmployeeManagement()
	{
		InitializeComponent();

	}
    private async void OnEmployeeManagementButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//EmployeeManagement");
    }

    private async void OnDashboardButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//Dashboard");
    }
    private async void OnLogOutButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//LoginView");
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

}