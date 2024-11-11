using EmployeeManagementSystem.Models;
using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagementSystem.AdminSettings
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

            var newEmployee = new Employee { EmployeeID = id, Name = name, Department = department };
            await App.Database.AddEmployeeAsync(newEmployee);
            Employees.Add(newEmployee);
            await DisplayAlert("Success", "Employee added successfully!", "OK");

            var existingDepartment = Departments.FirstOrDefault(d => d.DepartmentName == department);
            if (existingDepartment != null)
            {
                existingDepartment.Employees.Add(newEmployee);
            }
            else
            {
                Departments.Add(new Department { DepartmentName = department, Employees = new ObservableCollection<Employee> { newEmployee } });
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
            if (!string.IsNullOrWhiteSpace(department)) employee.Department = department;

            await App.Database.UpdateEmployeeAsync(employee);
            await DisplayAlert("Success", "Employee updated successfully!", "OK");
        }

        private async void OnDeleteEmployeeButtonClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var employee = button?.BindingContext as Employee;
            if (employee == null) return;

            bool confirmed = await DisplayAlert("Delete Employee", "Are you sure you want to delete this employee?", "Yes", "No");
            if (confirmed)
            {
                await App.Database.DeleteEmployeeAsync(employee.EmployeeID);
                Employees.Remove(employee);

                var department = Departments.FirstOrDefault(d => d.DepartmentName == employee.Department);
                if (department != null)
                {
                    department.Employees.Remove(employee);
                    if (department.Employees.Count == 0) Departments.Remove(department);
                }

                await DisplayAlert("Success", "Employee deleted successfully!", "OK");
            }
        }
    }
}
