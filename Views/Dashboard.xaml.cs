using System.Collections.ObjectModel;
using System.ComponentModel;
using EmployeeManagementSystem.Models;

namespace EmployeeManagementSystem.Dashboard
{
    public partial class Dashboard : ContentPage, INotifyPropertyChanged
    {
        private ObservableCollection<Department> _departments;

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

        public Dashboard()
        {
            InitializeComponent();
            LoadData();
            BindingContext = this;
        }

        private void LoadData()
        {
            // Sample data to simulate loading actual data
            Departments = new ObservableCollection<Department>
            {
                new Department { DepartmentID = 1, DepartmentName = "HR" },
                new Department { DepartmentID = 2, DepartmentName = "Engineering" }
            };

            // Add employees to the departments
            Departments[0].Employees.Add(new Employee { EmployeeID = 1, Name = "Alice", Department = "HR", Position = "Manager" });
            Departments[1].Employees.Add(new Employee { EmployeeID = 2, Name = "Bob", Department = "Engineering", Position = "Engineer" });
            Departments[1].Employees.Add(new Employee { EmployeeID = 3, Name = "Charlie", Department = "Engineering", Position = "Senior Engineer" });
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
}