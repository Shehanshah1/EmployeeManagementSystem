using EmployeeManagementSystem.Models;
using EmployeeManagementSystem.Services;
using System.Text.RegularExpressions;

namespace EmployeeManagementSystem.Views
{
    public partial class UserSettings : ContentPage
    {
        private readonly DatabaseService _databaseService;
        private string _profileImageSource;
        public string ProfileImageSource
        {
            get => _profileImageSource;
            set
            {
                _profileImageSource = value;
                OnPropertyChanged();
            }
        }
        public UserSettings()
        {
            InitializeComponent();
            _databaseService = new DatabaseService();
            ProfileImageSource = Preferences.Get("ProfileImagePath", "logo_app.png");
            BindingContext = this;
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
            try
            {
                if (MediaPicker.Default.IsCaptureSupported)
                {
                    var photo = await MediaPicker.Default.PickPhotoAsync();

                    if (photo != null)
                    {
                        string localFilePath = Path.Combine(FileSystem.AppDataDirectory, photo.FileName);
                        using (var stream = await photo.OpenReadAsync())
                        using (var newStream = File.OpenWrite(localFilePath))
                        {
                            await stream.CopyToAsync(newStream);
                        }
                        Preferences.Set("ProfileImagePath", localFilePath); // Save path
                        // Display success message
                        await DisplayAlert("Success", "Picture selected successfully!", "OK");
                    }
                }
                else
                {
                    await DisplayAlert("Not Supported", "Photo picking is not supported on this device.", "OK");
                }
                await App.NavigateToPage(new UserSettings());
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }


        private async void OnChangeSettingsButtonClicked(object sender, EventArgs e)
        {
            string entereduserEmail = userNameEntry.Text?.Trim();
            string enterednewPassword = passwordEntry.Text;

            // Validate email format
            if (string.IsNullOrEmpty(entereduserEmail) || !IsValidEmail(entereduserEmail))
            {
                await DisplayAlert("Invalid Input", "Please enter a valid email address.", "OK");
                return;
            }

            // Validate password field
            if (string.IsNullOrEmpty(enterednewPassword))
            {
                await DisplayAlert("Invalid Input", "Password cannot be empty.", "OK");
                return;
            }
            bool changesSaved = await SaveUserSettingsAsync(entereduserEmail,enterednewPassword);

            if (changesSaved)
            {
                await DisplayAlert("Success", "Username and password have been changed successfully.", "OK");
                await App.NavigateToPage(new EmployeeManagementSystem.Views.UserSettings());
            }
            else
            {
                await DisplayAlert("Error", "Failed to change username and password. Please try again.", "OK");
            }
        }
        private bool IsValidEmail(string email)
        {
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern);
        }
        private async Task<bool> SaveUserSettingsAsync(string newEmail,string newPassword)
        {
            var currentSessionId=  Preferences.Get("CurrentUserSessionId",1);
            return await _databaseService.UpdateUserEmailAndPasswordAsync(currentSessionId,newEmail,newPassword);
        }
    }
}
