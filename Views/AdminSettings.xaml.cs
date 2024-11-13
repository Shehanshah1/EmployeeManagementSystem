using EmployeeManagementSystem.Models;
using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagementSystem.Views
{
    public partial class AdminSettings : ContentPage
    {
        public ObservableCollection<Employee> Employees { get; set; } = new ObservableCollection<Employee>();
        public ObservableCollection<Department> Departments { get; set; } = new ObservableCollection<Department>();

        public AdminSettings()
        {
            InitializeComponent();
            LoadEmployees();
            BindingContext = this;
        }

        private async void LoadEmployees()
        {
            var employeesFromDb = await App.Database.GetAllEmployeesAsync();
            foreach (var emp in employeesFromDb)
            {
                Employees.Add(emp);

                // Add employee to department list
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


        // Navigation Buttons
        private async void OnEmployeeManagementButtonClicked(object sender, EventArgs e)
        {
            await App.NavigateToPage(new EmployeeManagement());
        }

        private async void OnLeaveRequestsButtonClicked(object sender, EventArgs e)
        {
            await App.NavigateToPage(new LeaveRequests());
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

        // Add Employee
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

            string email = await DisplayPromptAsync("New Employee", "Enter the employee's email:");
            if (string.IsNullOrWhiteSpace(email))
            {
                await DisplayAlert("Error", "Email cannot be empty.", "OK");
                return;
            }

            string position = await DisplayPromptAsync("New Employee", "Enter the employee's position:");
            if (string.IsNullOrWhiteSpace(position))
            {
                await DisplayAlert("Error", "Position cannot be empty.", "OK");
                return;
            }

            var newEmployee = new Employee { EmployeeID = id, Name = name, Department = department, Email = email, Position = position };
            await App.Database.AddEmployeeAsync(newEmployee);
            Employees.Add(newEmployee);

            // Update Department Collection
            var existingDepartment = Departments.FirstOrDefault(d => d.DepartmentName == department);
            if (existingDepartment != null)
            {
                existingDepartment.Employees.Add(newEmployee);
            }
            else
            {
                Departments.Add(new Department { DepartmentName = department, Employees = new ObservableCollection<Employee> { newEmployee } });
            }

            await DisplayAlert("Success", "Employee added successfully!", "OK");
        }

        // Edit Employee
        private async void OnEditEmployeeButtonClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var employee = button?.BindingContext as Employee;
            if (employee == null) return;

            string name = await DisplayPromptAsync("Edit Employee", "Enter the employee's name:", initialValue: employee.Name);
            if (!string.IsNullOrWhiteSpace(name)) employee.Name = name;

            string department = await DisplayPromptAsync("Edit Employee", "Enter the employee's department:", initialValue: employee.Department);
            if (!string.IsNullOrWhiteSpace(department)) employee.Department = department;

            await App.Database.UpdateEmployeeAsync(employee);
            await DisplayAlert("Success", "Employee updated successfully!", "OK");
        }

        // Delete Employee
        private async void OnDeleteEmployeeButtonClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var employee = button?.BindingContext as Employee;
            if (employee == null) return;

            bool confirm = await DisplayAlert("Delete Employee", $"Are you sure you want to delete {employee.Name}?", "Yes", "No");
            if (!confirm) return;

            await App.Database.DeleteEmployeeAsync(employee.EmployeeID);
            Employees.Remove(employee);

            // Remove from Department Collection
            var department = Departments.FirstOrDefault(d => d.DepartmentName == employee.Department);
            department?.Employees.Remove(employee);

            // Clean up department if no employees left
            if (department != null && !department.Employees.Any())
            {
                Departments.Remove(department);
            }

            await DisplayAlert("Success", "Employee deleted successfully!", "OK");
        }
    }
}
