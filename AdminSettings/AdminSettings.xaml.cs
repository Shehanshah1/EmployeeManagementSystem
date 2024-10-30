namespace EmployeeManagementSystem.AdminSettings;
using EmployeeManagementSystem.EmployeeManagement;
using EmployeeManagementSystem.LeaveRequests;
using EmployeeManagementSystem.Dashboard;
using EmployeeManagementSystem.UserSettings;
public partial class AdminSettings : ContentPage
{
	public AdminSettings()
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