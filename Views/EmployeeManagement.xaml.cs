namespace EmployeeManagementSystem.EmployeeManagement;
using EmployeeManagementSystem.Dashboard;
using EmployeeManagementSystem.LeaveRequests;
using EmployeeManagementSystem.Models;
using EmployeeManagementSystem.UserSettings;
using System.Collections.ObjectModel;
using System.ComponentModel;

public partial class EmployeeManagement : ContentPage
{
    public ObservableCollection<Employee> Employees { get; set; } = new ObservableCollection<Employee>();
    public ObservableCollection<Employee> FilteredEmployees { get; set; } = new ObservableCollection<Employee>();
    public ObservableCollection<Department> Departments { get; set; } = new ObservableCollection<Department>();

    public EmployeeManagement()
    {
        InitializeComponent();
        BindingContext = this;
        LoadEmployeeManagementData();
    }

    private async void LoadEmployeeManagementData()
    {
        var employeesFromDb = await App.Database.GetAllEmployeesAsync();
        foreach (var emp in employeesFromDb)
        {
            Employees.Add(emp);
            FilteredEmployees.Add(emp); // Initialize with all employees

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

    // Search method to update the filtered list based on the search text
    private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        var searchText = e.NewTextValue?.ToLower() ?? string.Empty;

        // Clear existing filtered list
        FilteredEmployees.Clear();

        // Filter the employees based on the search text
        var filtered = Employees.Where(emp =>
            emp.Name.ToLower().Contains(searchText) ||
            emp.Email.ToLower().Contains(searchText) ||
            emp.EmployeeID.ToString().Contains(searchText) ||
            emp.Department.ToLower().Contains(searchText));

        foreach (var emp in filtered)
        {
            FilteredEmployees.Add(emp);
        }
    }

    private async void OnEmployeeManagementButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//EmployeeManagement");
    }

    private async void OnDashboardButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//Dashboard");
    }
    private async void OnLogOutButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//LoginView");
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

}