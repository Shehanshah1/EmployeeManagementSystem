using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json;
using System.Windows.Input;
using EmployeeManagementSystem.Models;
using EmployeeManagementSystem.Services;
using Microsoft.Maui.Controls;

namespace EmployeeManagementSystem.Views
{
    public partial class LeaveRequests : ContentPage
    {
        private readonly DatabaseService _databaseService;

        private ObservableCollection<LeaveRequest> _leaveRequests;

        public ObservableCollection<LeaveRequest> LeaveRequestsList
        {
            get { return _leaveRequests; }
            set { _leaveRequests = value; OnPropertyChanged(); }
        }
        private LeaveRequest _selectedLeaveRequest;

        public LeaveRequests()
        {
            InitializeComponent();
            _databaseService = new DatabaseService();
            LeaveRequestsList = new ObservableCollection<LeaveRequest>();

            BindingContext = this;
            LoadLeaveRequests();
        }

        // Load leave requests from the database
        private async void LoadLeaveRequests()
        {
            var leaveRequests = await _databaseService.GetLeaveRequestsAsync();
            LeaveRequestsList.Clear();

            foreach (var request in leaveRequests)
            {
                LeaveRequestsList.Add(request);
            }
        }

        private async void OnAddNewLeaveRequestClicked(object sender, EventArgs e)
        {
            // Show the popup

            string idInput = await DisplayPromptAsync("Leave Request", "Enter the employee's ID:");
            if (string.IsNullOrWhiteSpace(idInput) || !int.TryParse(idInput, out int id))
            {
                await DisplayAlert("Error", "Please enter a valid numeric ID.", "OK");
                return;
            }

            string name = await DisplayPromptAsync("Leave Request", "Enter the employee's name:");
            if (string.IsNullOrWhiteSpace(name))
            {
                await DisplayAlert("Error", "Employee name cannot be empty.", "OK");
                return;
            }

            string department = await DisplayPromptAsync("Leave Request", "Enter the employee's department:");
            if (string.IsNullOrWhiteSpace(department))
            {
                await DisplayAlert("Error", "Department cannot be empty.", "OK");
                return;
            }

            string startDate = await DisplayPromptAsync("Leave Request", "Enter start date:");
            if (string.IsNullOrWhiteSpace(startDate))
            {
                await DisplayAlert("Error", "StartDate cannot be empty.", "OK");
                return;
            }

            string days = await DisplayPromptAsync("Leave Request", "Enter number of days:");
            if (string.IsNullOrWhiteSpace(days))
            {
                await DisplayAlert("Error", "days cannot be empty.", "OK");
                return;
            }

            string reason_ = await DisplayPromptAsync("Leave Request", "Enter the reason for leave:");
            if (string.IsNullOrWhiteSpace(reason_))
            {
                await DisplayAlert("Error", "Reason cannot be empty.", "OK");
                return;
            }

            var newRequest = new LeaveRequest
            {
                EmployeeName = name,
                EmployeeID = int.Parse(idInput),
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(int.Parse(days)),
                days = (int.Parse(days)),
                Reason = reason_,
                ApprovalStatus = "Pending"
            };

            await _databaseService.AddLeaveRequestAsync(newRequest);
            LeaveRequestsList.Add(newRequest);
        }
        private async void OnAddRequestSubmitted(object sender, EventArgs e)
        {
            var newRequest = new LeaveRequest
            {
                EmployeeName = "New Employee",
                LeaveRequestID = 1234,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(5),
                ApprovalStatus = "Pending"
            };

            await _databaseService.AddLeaveRequestAsync(newRequest);
            LeaveRequestsList.Add(newRequest);
            LoadLeaveRequests();
        }

        // Handle the "Edit Selected Request" button
        private async void OnEditSelectedRequestClicked(object sender, EventArgs e)
        {
            if (_selectedLeaveRequest == null)
            {
                await DisplayAlert("Error", "Please select a leave request to edit.", "OK");
                return;
            }

            _selectedLeaveRequest.ApprovalStatus = "Approved";  // Example modification
            await _databaseService.UpdateLeaveRequestAsync(_selectedLeaveRequest);
            LoadLeaveRequests();
        }

        // Handle the "Delete Selected Request" button
        private async void OnDeleteSelectedRequestClicked(object sender, EventArgs e)
        {
            if (_selectedLeaveRequest == null)
            {
                await DisplayAlert("Error", "Please select a leave request to delete.", "OK");
                return;
            }

            await _databaseService.DeleteLeaveRequestAsync(_selectedLeaveRequest.LeaveRequestID);
            LeaveRequestsList.Remove(_selectedLeaveRequest);
        }

        // Set the selected leave request when an item is tapped
        private void OnLeaveRequestSelected(object sender, SelectionChangedEventArgs e)
        {
            _selectedLeaveRequest = e.CurrentSelection.FirstOrDefault() as LeaveRequest;
        }

        // Navigation button click events
        private async void OnDashboardButtonClicked(object sender, EventArgs e) { await App.NavigateToPage(new Dashboard()); }
        private async void OnEmployeeManagementButtonClicked(object sender, EventArgs e) { await App.NavigateToPage(new EmployeeManagement()); }
        private async void OnAdminSettingsButtonClicked(object sender, EventArgs e) { await App.NavigateToPage(new AdminSettings()); }
        private async void OnLogOutButtonClicked(object sender, EventArgs e) { await App.NavigateToPage(new LoginView()); }
        private async void OnUserSettingsButtonClicked(object sender, EventArgs e) { await App.NavigateToPage(new UserSettings()); }
    }
}
