namespace EmployeeManagementSystem.Views
{
    public partial class UserSettings : ContentPage
    {
        public UserSettings()
        {
            InitializeComponent();
        }

        private async void OnEmployeeManagementButtonClicked(object sender, EventArgs e)
        {
            await App.NavigateToPage(new EmployeeManagement());
        }

        private async void OnLeaveRequestsButtonClicked(object sender, EventArgs e)
        {
            await App.NavigateToPage(new LeaveRequests());
        }

        private async void OnAdminSettingsButtonClicked(object sender, EventArgs e)
        {
            await App.NavigateToPage(new AdminSettings());
        }

        private async void OnUserSettingsButtonClicked(object sender, EventArgs e)
        {
            await App.NavigateToPage(new UserSettings());
        }

        private async void OnLogOutButtonClicked(object sender, EventArgs e)
        {
            // Navigate back to the LoginView
            await App.NavigateToPage(new LoginView());
        }

        private async void OnDashboardButtonClicked(object sender, EventArgs e)
        {
            await App.NavigateToPage(new Dashboard());
        }
    }
}
