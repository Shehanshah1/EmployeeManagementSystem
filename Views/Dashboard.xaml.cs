using System.Collections.ObjectModel;
using System.ComponentModel;
using EmployeeManagementSystem.Models;
using EmployeeManagementSystem.Services;

namespace EmployeeManagementSystem.Views
{
    public partial class Dashboard : ContentPage, INotifyPropertyChanged
    {
        private readonly DatabaseService _databaseService;
        private int _employees;
        private int _departments;
        private string _userName;
        private ObservableCollection<LeaveRequest> _leaveRequests;
        public ObservableCollection<LeaveRequest> LeaveRequestsList
        {
            get { return _leaveRequests; }
            set { _leaveRequests = value; OnPropertyChanged(); }
        }
        public int Employees
        {
            get => _employees;
            set
            {
                if (_employees != value)
                {
                    _employees = value;
                    OnPropertyChanged(nameof(Employees));
                }
            }
        }

        public int Departments
        {
            get => _departments;
            set
            {
                if (_departments != value)
                {
                    _departments = value;
                    OnPropertyChanged(nameof(Departments));
                }
            }
        }

        public string UserName // Aelin: Added username
        {
            get => _userName;
            set
            {
                if (_userName != value)
                {
                    _userName = value;
                    OnPropertyChanged(nameof(UserName));
                }
            }
        }
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
        public  Dashboard()
        {
            InitializeComponent();
            _databaseService = new DatabaseService();
            Employees = Preferences.Get("EmployeeCount", 0); 
            Departments = Preferences.Get("DepartmentCount", 0);
            LeaveRequestsList = new ObservableCollection<LeaveRequest>();
            ProfileImageSource = Preferences.Get("ProfileImagePath", "logo_app.png");
            // Aelin: Temporary default name (needs to be personalized to user)
            UserName = Preferences.Get("CurrentUserEmail", "Aelin");
            // Aelin: Binding
            BindingContext = this;
            LoadLeaveRequests();

        }
        private async void LoadLeaveRequests()
        {
            try
            {
                var leaveRequests = await _databaseService.GetLeaveRequestsAsync();
                foreach (var request in leaveRequests)
                {
                    LeaveRequestsList.Add(request);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to load leave requests: {ex.Message}", "OK");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async void OnEmployeeManagementButtonClicked(object sender, EventArgs e)
        {
            await NavigateToPageAsync(new EmployeeManagement());
        }

        private async void OnLeaveRequestsButtonClicked(object sender, EventArgs e)
        {
            await NavigateToPageAsync(new LeaveRequests());
        }

        private async void OnAdminSettingsButtonClicked(object sender, EventArgs e)
        {
            await NavigateToPageAsync(new AdminSettings());
        }

        private async void OnUserSettingsButtonClicked(object sender, EventArgs e)
        {
            await NavigateToPageAsync(new UserSettings());
        }

        private async void OnLogOutButtonClicked(object sender, EventArgs e)
        {
            await NavigateToPageAsync(new LoginView());
        }

        private async Task NavigateToPageAsync(ContentPage targetPage)
        {
            try
            {
                // Ensure the navigation is handled centrally
                await Navigation.PushAsync(targetPage);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Navigation Error", $"Unable to navigate: {ex.Message}", "OK");
            }
        }
    }
}
