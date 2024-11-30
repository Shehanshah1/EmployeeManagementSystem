using EmployeeManagementSystem.Models;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagementSystem.Views
{
    public partial class AdminSettings : ContentPage
    {
        public ObservableCollection<Employee> Employees { get; set; } = new ObservableCollection<Employee>();
        public ObservableCollection<Department> Departments { get; set; } = new ObservableCollection<Department>();
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

        public AdminSettings()
        {
            InitializeComponent();
            BindingContext = this;
            ProfileImageSource = Preferences.Get("ProfileImagePath", "logo_app.png");

            // Load saved employee data from Preferences
            LoadEmployeesFromPreferences();
        }

        // Load Employees from Preferences
        private void LoadEmployeesFromPreferences()
        {
            // Retrieve the number of employees stored in Preferences
            int employeeCount = Preferences.Get("EmployeeCount", 0);

            // Load employees and their data from Preferences
            for (int i = 1; i <= employeeCount; i++)
            {
                string name = Preferences.Get($"Employee_{i}_Name", string.Empty);
                string department = Preferences.Get($"Employee_{i}_Department", string.Empty);
                string email = Preferences.Get($"Employee_{i}_Email", string.Empty);
                string position = Preferences.Get($"Employee_{i}_Position", string.Empty);

                if (!string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(department))
                {
                    var employee = new Employee
                    {
                        EmployeeID = i,
                        Name = name,
                        Department = department,
                        Email = email,
                        Position = position
                    };

                    Employees.Add(employee);

                    var departmentEntry = Departments.FirstOrDefault(d => d.DepartmentName == department);
                    if (departmentEntry != null)
                    {
                        departmentEntry.Employees.Add(employee);
                    }
                    else
                    {
                        Departments.Add(new Department { DepartmentName = department, Employees = new ObservableCollection<Employee> { employee } });
                        // Update the employee count in Preferences
                        Preferences.Set("DepartmentCount", Departments.Count);
                    }
                }
            }
        }

        // New Department Button Functionality
        private async void OnAddDepartmentButtonClicked(object sender, EventArgs e)
        {
            // Prompt for the new department name
            string departmentName = await Application.Current.MainPage.DisplayPromptAsync("New Department", "Enter the name of the department:");
            if (string.IsNullOrWhiteSpace(departmentName))
            {
                await DisplayAlert("Error", "Department name cannot be empty.", "OK");
                return;
            }

            // Check if the department already exists
            if (Departments.Any(d => d.DepartmentName.Equals(departmentName, StringComparison.OrdinalIgnoreCase)))
            {
                await DisplayAlert("Error", "A department with this name already exists.", "OK");
                return;
            }

            // Add the new department
            Departments.Add(new Department
            {
                DepartmentName = departmentName,
                Employees = new ObservableCollection<Employee>()
            });

            await DisplayAlert("Success", $"Department '{departmentName}' added successfully!", "OK");
        }

        // Add Employee Button Functionality
        private async void OnAddEmployeeButtonClicked(object sender, EventArgs e)
        {
            // Prompt for employee details
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

            // Create and add the new employee
            var newEmployee = new Employee { EmployeeID = id, Name = name, Department = department, Email = email, Position = position };
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

            // Save the new employee data to Preferences
            SaveEmployeeToPreferences(newEmployee);

            await DisplayAlert("Success", "Employee added successfully!", "OK");
        }

        // Save Employee Data to Preferences
        private void SaveEmployeeToPreferences(Employee employee)
        {
            // Get the current number of employees in Preferences
            int employeeCount = Preferences.Get("EmployeeCount", 0);

            // Increment employee count
            employeeCount++;

            // Save employee data to Preferences
            Preferences.Set($"Employee_{employeeCount}_Name", employee.Name);
            Preferences.Set($"Employee_{employeeCount}_Department", employee.Department);
            Preferences.Set($"Employee_{employeeCount}_Email", employee.Email);
            Preferences.Set($"Employee_{employeeCount}_Position", employee.Position);

            // Update the employee count in Preferences
            Preferences.Set("EmployeeCount", employeeCount);
        }

        // Edit Employee Button Functionality
        private async void OnEditEmployeeButtonClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var employee = button?.BindingContext as Employee;
            if (employee == null) return;

            string name = await DisplayPromptAsync("Edit Employee", "Enter the employee's name:", initialValue: employee.Name);
            if (!string.IsNullOrWhiteSpace(name)) employee.Name = name;

            string department = await DisplayPromptAsync("Edit Employee", "Enter the employee's department:", initialValue: employee.Department);
            if (!string.IsNullOrWhiteSpace(department)) employee.Department = department;

            // Update Preferences with new data
            UpdateEmployeeInPreferences(employee);

            await DisplayAlert("Success", "Employee updated successfully!", "OK");
        }

        // Update Employee Data in Preferences
        private void UpdateEmployeeInPreferences(Employee employee)
        {
            Preferences.Set($"Employee_{employee.EmployeeID}_Name", employee.Name);
            Preferences.Set($"Employee_{employee.EmployeeID}_Department", employee.Department);
            Preferences.Set($"Employee_{employee.EmployeeID}_Email", employee.Email);
            Preferences.Set($"Employee_{employee.EmployeeID}_Position", employee.Position);
        }

        // Delete Employee Button Functionality
        private async void OnDeleteEmployeeButtonClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var employee = button?.BindingContext as Employee;
            if (employee == null) return;

            bool confirm = await DisplayAlert("Delete Employee", $"Are you sure you want to delete {employee.Name}?", "Yes", "No");
            if (!confirm) return;

            Employees.Remove(employee);

            // Remove from Department Collection
            var department = Departments.FirstOrDefault(d => d.DepartmentName == employee.Department);
            department?.Employees.Remove(employee);

            // Clean up department if no employees left
            if (department != null && !department.Employees.Any())
            {
                Departments.Remove(department);
            }

            // Delete from Preferences
            DeleteEmployeeFromPreferences(employee.EmployeeID);

            await DisplayAlert("Success", "Employee deleted successfully!", "OK");
        }

        // Delete Employee Data from Preferences
        private void DeleteEmployeeFromPreferences(int employeeId)
        {
            // Remove employee data from Preferences
            Preferences.Remove($"Employee_{employeeId}_Name");
            Preferences.Remove($"Employee_{employeeId}_Department");
            Preferences.Remove($"Employee_{employeeId}_Email");
            Preferences.Remove($"Employee_{employeeId}_Position");
        }

        // Log Out Button Functionality
        private async void OnLogOutButtonClicked(object sender, EventArgs e)
        {
            await App.NavigateToPage(new LoginView());
        }

        // Navigate to the Dashboard
        private async void OnDashboardButtonClicked(object sender, EventArgs e)
        {
            await App.NavigateToPage(new Dashboard());
        }

        // Navigate to Employee Management Page
        private async void OnEmployeeManagementButtonClicked(object sender, EventArgs e)
        {
            await App.NavigateToPage(new EmployeeManagement());
        }

        // Navigate to User Settings
        private async void OnUserSettingsButtonClicked(object sender, EventArgs e)
        {
            await App.NavigateToPage(new UserSettings());
        }

        private async void OnLeaveRequestsButtonClicked(object sender, EventArgs e)
        {
            await App.NavigateToPage(new LeaveRequests());
        }
    }


}
