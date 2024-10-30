namespace EmployeeManagementSystem.LeaveRequests;
using EmployeeManagementSystem.EmployeeManagement;
using EmployeeManagementSystem.Dashboard;
using EmployeeManagementSystem.AdminSettings;
using EmployeeManagementSystem.UserSettings;
public partial class LeaveRequests : ContentPage
{
	public LeaveRequests()
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