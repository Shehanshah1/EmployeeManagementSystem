using System.Collections.ObjectModel;
using System.ComponentModel;
using EmployeeManagementSystem.Models;

namespace EmployeeManagementSystem.Dashboard
{
    public partial class Dashboard : ContentPage
    {
        public ObservableCollection<Employee> Employees { get; set; } = new ObservableCollection<Employee>();
        public ObservableCollection<Department> Departments { get; set; } = new ObservableCollection<Department>();

        public Dashboard()
        {
            InitializeComponent();
            LoadDashboardData();
            BindingContext = this;
        }

        private async void LoadDashboardData()
        {
            var employeesFromDb = await App.Database.GetAllEmployeesAsync();
            foreach (var emp in employeesFromDb)
            {
                Employees.Add(emp);

                // Ensure department consistency
                var existingDepartment = Departments.FirstOrDefault(d => d.DepartmentName == emp.Department);
                if (existingDepartment != null)
                {
                    existingDepartment.Employees.Add(emp);
                }
                else
                {
                    Departments.Add(new Department
                    {
                        DepartmentName = emp.Department,
                        Employees = new ObservableCollection<Employee> { emp }
                    });
                }
            }
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