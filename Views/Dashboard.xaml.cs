using System.Collections.ObjectModel;
using System.ComponentModel;
using EmployeeManagementSystem.Models;

namespace EmployeeManagementSystem.Views
{
    public partial class Dashboard : ContentPage, INotifyPropertyChanged
    {
        private ObservableCollection<Employee> _employees;
        private ObservableCollection<Department> _departments;
        private string _userName;

        public ObservableCollection<Employee> Employees
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

        public ObservableCollection<Department> Departments
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

        public Dashboard()
        {
            InitializeComponent();
            Employees = new ObservableCollection<Employee>();
            Departments = new ObservableCollection<Department>();

            // Aelin: Temporary default name (needs to be personalized to user)
            UserName = "Aelin";

            // Aelin: Binding
            var greetingLabel = this.FindByName<Label>("GreetingLabel");
            if (greetingLabel != null)
            {
                greetingLabel.BindingContext = this;
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
