namespace EmployeeManagementSystem.Views;
using EmployeeManagementSystem.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;

public partial class EmployeeManagement : ContentPage
{
    public ObservableCollection<Employee> Employees { get; set; } = new ObservableCollection<Employee>();
    public ObservableCollection<Employee> FilteredEmployees { get; set; } = new ObservableCollection<Employee>();
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
    public EmployeeManagement()
    {
        InitializeComponent();
        ProfileImageSource = Preferences.Get("ProfileImagePath", "logo_app.png");

        // Aelin: Added mock employee data but backend devs need to remove this and add the logic 
        FilteredEmployees = new ObservableCollection<Employee>
            {
                new Employee { Name = "Reham Afzal", Position = "Backend Developer", Department = "Development", Email = "reham.afzal@gmail.com" },
                new Employee { Name = "Ron Dickson", Position = "Backend Developer", Department = "Development", Email = "ron.dickson@gmail.com" },
                new Employee { Name = "Sanskar Bhattarai", Position = "Backend Developer", Department = "Development", Email = "sanskar.bhattarai@gmail.com" },
                new Employee { Name = "Muhammad Hashmi", Position = "Frontend Developer", Department = "UI/UX", Email = "muhammad.hashmi@gmail.com" },
                new Employee { Name = "Aelin Med", Position = "Frontend Developer", Department = "UI/UX", Email = "aelin.med@gmail.com" }
            };

        BindingContext = this;
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
        await App.NavigateToPage(new EmployeeManagement());
    }

    private async void OnDashboardButtonClicked(object sender, EventArgs e)
    {
        await App.NavigateToPage(new Dashboard());
    }
    private async void OnLogOutButtonClicked(object sender, EventArgs e)
    {
        await App.NavigateToPage(new LoginView());
    }
    private async void OnLeaveRequestsButtonClicked(object sender, EventArgs e)
    {
        await App.NavigateToPage(new LeaveRequests());
    }
    private async void OnAdminSettingsButtonClicked(object sender, EventArgs e)
    {
        await App.NavigateToPage(new AdminSettings());
    }
    private async void OnUserSettingsButtonClicked(object sender, EventArgs e)
    {
        await App.NavigateToPage(new UserSettings());
    }

}