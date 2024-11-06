namespace EmployeeManagementSystem.AdminSettings
{
    using EmployeeManagementSystem.EmployeeManagement;
    using EmployeeManagementSystem.LeaveRequests;
    using EmployeeManagementSystem.Dashboard;
    using EmployeeManagementSystem.UserSettings;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Microsoft.Maui.Controls;

    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
    }

    public class Department
    {
        public string Title { get; set; }
        public int CurrentEmployees { get; set; }
        public ObservableCollection<Employee> Employees { get; set; } = new ObservableCollection<Employee>();
    }

    public partial class AdminSettings : ContentPage
    {
        public ObservableCollection<Employee> Employees { get; set; } = new ObservableCollection<Employee>();
        public ObservableCollection<Department> Departments { get; set; } = new ObservableCollection<Department>();

        public AdminSettings()
        {
            InitializeComponent();
            BindingContext = this;
        }

        private async void OnLogOutButtonClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//LoginView");
        }

        private async void OnDashboardButtonClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//Dashboard");
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

        private async void OnAddEmployeeButtonClicked(object sender, EventArgs e)
        {
            string idInput = await DisplayPromptAsync("New Employee", "Enter the employee's ID:");
            if (string.IsNullOrWhiteSpace(idInput) || !int.TryParse(idInput, out int id))
            {
                await DisplayAlert("Error", "Please enter a valid numeric ID.", "OK");
                return;
            }

            string name = await DisplayPromptAsync("New Employee", "Enter the employee's name:");
            if (string.IsNullOrWhiteSpace(name))
            {
                await DisplayAlert("Error", "Employee name cannot be empty.", "OK");
                return;
            }

            string department = await DisplayPromptAsync("New Employee", "Enter the employee's department:");
            if (string.IsNullOrWhiteSpace(department))
            {
                await DisplayAlert("Error", "Department cannot be empty.", "OK");
                return;
            }

            var newEmployee = new Employee { Id = id, Name = name, Department = department };
            Employees.Add(newEmployee);
            await DisplayAlert("Success", "Employee added successfully!", "OK");

            var existingDepartment = Departments.FirstOrDefault(d => d.Title == department);
            if (existingDepartment != null)
            {
                existingDepartment.Employees.Add(newEmployee);
                existingDepartment.CurrentEmployees++;
            }
            else
            {
                Departments.Add(new Department { Title = department, CurrentEmployees = 1, Employees = new ObservableCollection<Employee> { newEmployee } });
            }
        }

        private async void OnEditEmployeeButtonClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var employee = button?.BindingContext as Employee;
            if (employee == null) return;

            string name = await DisplayPromptAsync("Edit Employee", "Enter the employee's name:", initialValue: employee.Name);
            if (!string.IsNullOrWhiteSpace(name)) employee.Name = name;

            string department = await DisplayPromptAsync("Edit Employee", "Enter the employee's department:", initialValue: employee.Department);
            if (!string.IsNullOrWhiteSpace(department) && department != employee.Department)
            {
                var oldDepartment = Departments.FirstOrDefault(d => d.Title == employee.Department);
                oldDepartment?.Employees.Remove(employee);
                if (oldDepartment != null) oldDepartment.CurrentEmployees--;

                employee.Department = department;
                var newDepartment = Departments.FirstOrDefault(d => d.Title == department);
                if (newDepartment != null)
                {
                    newDepartment.Employees.Add(employee);
                    newDepartment.CurrentEmployees++;
                }
                else
                {
                    Departments.Add(new Department { Title = department, CurrentEmployees = 1, Employees = new ObservableCollection<Employee> { employee } });
                }
            }
        }

        private async void OnDeleteEmployeeButtonClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var employee = button?.BindingContext as Employee;
            if (employee == null) return;

            bool confirmed = await DisplayAlert("Delete Employee", "Are you sure you want to delete this employee?", "Yes", "No");
            if (confirmed)
            {
                Employees.Remove(employee);

                var department = Departments.FirstOrDefault(d => d.Title == employee.Department);
                department?.Employees.Remove(employee);
                if (department != null) department.CurrentEmployees--;

                if (department != null && department.CurrentEmployees <= 0)
                {
                    Departments.Remove(department);
                }
            }
        }
    }
}
