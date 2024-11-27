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
            await App.NavigateToPage(new LoginView());
        }

        private async void OnDashboardButtonClicked(object sender, EventArgs e)
        {
            await App.NavigateToPage(new Dashboard());
        }

        private async void OnSelectPictureButtonClicked(object sender, EventArgs e)
        {
            // Implement picture selection logic here
        }

        private async void OnChangeSettingsButtonClicked(object sender, EventArgs e)
        {
            // Assuming there's a method to save user settings
            bool changesSaved = await SaveUserSettingsAsync();

            if (changesSaved)
            {
                await App.NavigateToPage(new EmployeeManagementSystem.Views.UserSettings());
            }
        }

        private async Task<bool> SaveUserSettingsAsync()
        {
            // Implementation for saving user settings goes here
            // Return true if successful, false otherwise
            return true;
        }
    }
}
