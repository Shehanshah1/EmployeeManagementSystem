using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using EmployeeManagementSystem.Models;
using EmployeeManagementSystem.Services;
using Microsoft.Maui.Controls;

namespace EmployeeManagementSystem.Views
{
    public partial class LeaveRequests : ContentPage
    {
        private readonly DatabaseService _databaseService;
        public ObservableCollection<LeaveRequest> LeaveRequestsList { get; set; }
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

        // Handle the "Add New Leave Request" button
        private async void OnAddNewLeaveRequestClicked(object sender, EventArgs e)
        {
            var newRequest = new LeaveRequest
            {
                EmployeeName = "New Employee",
                LeaveType = "Annual",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(5),
                Status = "Pending"
            };

            await _databaseService.AddLeaveRequestAsync(newRequest);
            LeaveRequestsList.Add(newRequest);
        }

        // Handle the "Edit Selected Request" button
        private async void OnEditSelectedRequestClicked(object sender, EventArgs e)
        {
            if (_selectedLeaveRequest == null)
            {
                await DisplayAlert("Error", "Please select a leave request to edit.", "OK");
                return;
            }

            _selectedLeaveRequest.Status = "Approved";  // Example modification
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

            await _databaseService.DeleteLeaveRequestAsync(_selectedLeaveRequest.Id);
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
