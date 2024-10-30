namespace EmployeeManagementSystem.UserSettings;
using EmployeeManagementSystem.EmployeeManagement;
using EmployeeManagementSystem.LeaveRequests;
using EmployeeManagementSystem.AdminSettings;
using EmployeeManagementSystem.Dashboard;

public partial class UserSettings : ContentPage
{
	public UserSettings()
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
}