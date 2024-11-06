namespace EmployeeManagementSystem.AdminSettings;
    using EmployeeManagementSystem.EmployeeManagement;
    using EmployeeManagementSystem.LeaveRequests;
    using EmployeeManagementSystem.Dashboard;
    using EmployeeManagementSystem.UserSettings;

    public class Employee
    {
        public int Id { get; set; }                // Employee ID
        public string Name { get; set; }           // Employee Name
        public string Department { get; set; }     // Employee Department
        // Add any additional properties as needed
    }

    public partial class AdminSettings : ContentPage
    {
    private async void OnLogOutButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//LoginView");
    }
    private async void OnDashboardButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//Dashboard");
    }
    private static List<Employee> employees = new List<Employee>();

        public AdminSettings()
        {
            InitializeComponent();
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

        // Event handler for Add Employee button click
        private async void OnAddEmployeeButtonClicked(object sender, EventArgs e)
        {
            // Prompt user for the employee's ID
            string idInput = await DisplayPromptAsync("New Employee", "Enter the employee's ID:");
            if (string.IsNullOrWhiteSpace(idInput) || !int.TryParse(idInput, out int id))
            {
                await DisplayAlert("Error", "Please enter a valid numeric ID.", "OK");
                return;
            }

            // Prompt user for the employee's name
            string name = await DisplayPromptAsync("New Employee", "Enter the employee's name:");
            if (string.IsNullOrWhiteSpace(name))
            {
                await DisplayAlert("Error", "Employee name cannot be empty.", "OK");
                return;
            }

            // Prompt user for the employee's department
            string department = await DisplayPromptAsync("New Employee", "Enter the employee's department:");
            if (string.IsNullOrWhiteSpace(department))
            {
                await DisplayAlert("Error", "Department cannot be empty.", "OK");
                return;
            }

            // Create a new Employee object with the provided information
            Employee newEmployee = new Employee
            {
                Id = id,
                Name = name,
                Department = department
            };

            // Add the new employee to the list
            employees.Add(newEmployee);

            // Notify user of successful addition
            await DisplayAlert("Success", "Employee added successfully!", "OK");
        }


       

        // Method to get all employees
        private List<Employee> GetAllEmployees()
        {
            return employees;
        }

        // Method to get an employee by ID
        private Employee GetEmployeeById(int id)
        {
            return employees.FirstOrDefault(e => e.Id == id);
        }

        // Method to update an employee
        private void UpdateEmployee(Employee employee)
        {
            var existingEmployee = GetEmployeeById(employee.Id);
            if (existingEmployee != null)
            {
                existingEmployee.Name = employee.Name;
                existingEmployee.Department = employee.Department;
                // Update other properties as needed
            }
        }

        // Method to delete an employee
        private void DeleteEmployee(int id)
        {
            var employeeToRemove = GetEmployeeById(id);
            if (employeeToRemove != null)
            {
                employees.Remove(employeeToRemove);
            }
        }

    }



