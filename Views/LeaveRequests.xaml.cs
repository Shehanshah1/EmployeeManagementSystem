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
        public LeaveRequests()
        {
            InitializeComponent();
            _databaseService = new DatabaseService();
            LeaveRequestsList = new ObservableCollection<LeaveRequest>();
            ProfileImageSource = Preferences.Get("ProfileImagePath", "logo_app.png");
            BindingContext = this;
            LoadLeaveRequests();
        }

        // Load leave requests from the database
        private async void LoadLeaveRequests()
        {
            var leaveRequests = await _databaseService.GetLeaveRequestsAsync();
            _allLeaveRequests = new ObservableCollection<LeaveRequest>(leaveRequests); // Full list
            LeaveRequestsList = new ObservableCollection<LeaveRequest>(_allLeaveRequests); // Filtered list
            //LeaveRequestsList.Clear();

            /*foreach (var request in leaveRequests)
            {
                LeaveRequestsList.Add(request);
            }*/
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
                StartDate = DateTime.Parse(startDate),
                EndDate = DateTime.Parse(startDate).AddDays(int.Parse(days)),
                days = (int.Parse(days)),
                Reason = reason_,
                ApprovalStatus = "Pending"
            };

            await _databaseService.AddLeaveRequestAsync(newRequest);
            LeaveRequestsList.Add(newRequest);
            LoadLeaveRequests();
        }

        private async void OnEditSelectedRequestClicked(object sender, EventArgs e)
        {
            // Step 1: Ask for the LeaveRequest ID
            string idInput = await DisplayPromptAsync("Edit Leave Request", "Enter the Leave Request ID to edit:");
            if (string.IsNullOrWhiteSpace(idInput) || !int.TryParse(idInput, out int leaveRequestId))
            {
                await DisplayAlert("Error", "Please enter a valid numeric Leave Request ID.", "OK");
                return;
            }

            var requestToEdit = LeaveRequestsList.FirstOrDefault(lr => lr.LeaveRequestID == leaveRequestId);
            if (requestToEdit == null)
            {
                await DisplayAlert("Error", "Leave Request not found.", "OK");
                return;
            }

            string parameterName = await DisplayPromptAsync("Edit Leave Request", "Enter the parameter to update (e.g., EmployeeName, EmployeeID, StartDate, EndDate, Reason, ApprovalStatus):");
            if (string.IsNullOrWhiteSpace(parameterName))
            {
                await DisplayAlert("Error", "Parameter name cannot be empty.", "OK");
                return;
            }

            string newValue = await DisplayPromptAsync("Edit Leave Request", $"Enter the new value for {parameterName}:");
            if (string.IsNullOrWhiteSpace(newValue))
            {
                await DisplayAlert("Error", "New value cannot be empty.", "OK");
                return;
            }
            try
            {
                switch (parameterName)
                {
                    case "EmployeeName":
                        requestToEdit.EmployeeName = newValue;
                        break;

                    case "EmployeeID":
                        requestToEdit.EmployeeID = int.Parse(newValue);
                        break;

                    case "StartDate":
                        if (DateTime.TryParse(newValue, out DateTime startDate))
                        {
                            requestToEdit.StartDate = startDate;
                        }
                        else
                        {
                            await DisplayAlert("Error", "Invalid date format.", "OK");
                            return;
                        }
                        break;

                    case "EndDate":
                        if (DateTime.TryParse(newValue, out DateTime endDate))
                        {
                            requestToEdit.EndDate = endDate;
                        }
                        else
                        {
                            await DisplayAlert("Error", "Invalid date format.", "OK");
                            return;
                        }
                        break;

                    case "Reason":
                        requestToEdit.Reason = newValue;
                        break;

                    case "ApprovalStatus":
                        requestToEdit.ApprovalStatus = newValue;
                        break;

                    case "days":
                        if (int.TryParse(newValue, out int days))
                        {
                            requestToEdit.days = days;
                        }
                        else
                        {
                            await DisplayAlert("Error", "Invalid number format for days.", "OK");
                            return;
                        }
                        break;

                    default:
                        await DisplayAlert("Error", "Invalid parameter name. Please try again.", "OK");
                        return;
                }

                await _databaseService.UpdateLeaveRequestAsync(requestToEdit);
                LoadLeaveRequests();

                await DisplayAlert("Success", "Leave Request updated successfully.", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"An error occurred while updating: {ex.Message}", "OK");
            }
        }

        private async void OnDeleteSelectedRequestClicked(object sender, EventArgs e)
        {
            string idInput = await DisplayPromptAsync("Delete Leave Request", "Enter the Leave Request ID to delete:");
            if (string.IsNullOrWhiteSpace(idInput) || !int.TryParse(idInput, out int leaveRequestId))
            {
                await DisplayAlert("Error", "Please enter a valid numeric Leave Request ID.", "OK");
                return;
            }

            var requestToDelete = LeaveRequestsList.FirstOrDefault(lr => lr.LeaveRequestID == leaveRequestId);
            if (requestToDelete == null)
            {
                await DisplayAlert("Error", "Leave Request not found.", "OK");
                return;
            }

            // Confirm deletion
            bool confirm = await DisplayAlert("Confirm Delete", $"Are you sure you want to delete the Leave Request for Employee '{requestToDelete.EmployeeName}'?", "Yes", "No");
            if (!confirm)
                return;

            await _databaseService.DeleteLeaveRequestAsync(requestToDelete.LeaveRequestID);
            LeaveRequestsList.Remove(requestToDelete);
            LoadLeaveRequests();
            await DisplayAlert("Success", "Leave Request deleted successfully.", "OK");
        }

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
                FilterLeaveRequests();
            }
        }

        private string _searchStatus;
        public string SearchStatus
        {
            get => _searchStatus;
            set
            {
                _searchStatus = value;
                OnPropertyChanged();
                FilterLeaveRequests();
            }
        }

        private ObservableCollection<LeaveRequest> _allLeaveRequests;

        private void FilterLeaveRequests()
        {
            if (_allLeaveRequests == null) return;

            var filtered = _allLeaveRequests
                .Where(r =>
                    (string.IsNullOrWhiteSpace(SearchText) ||
                     r.EmployeeID.ToString().Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                     r.EmployeeName.Contains(SearchText, StringComparison.OrdinalIgnoreCase)) &&
                    (string.IsNullOrWhiteSpace(SearchStatus) ||
                     r.ApprovalStatus.Contains(SearchStatus, StringComparison.OrdinalIgnoreCase)))
                .ToList();

            LeaveRequestsList.Clear();
            foreach (var request in filtered)
            {
                LeaveRequestsList.Add(request);
            }
        }


        private async void OnDashboardButtonClicked(object sender, EventArgs e) { await App.NavigateToPage(new Dashboard()); }
        private async void OnEmployeeManagementButtonClicked(object sender, EventArgs e) { await App.NavigateToPage(new EmployeeManagement()); }
        private async void OnAdminSettingsButtonClicked(object sender, EventArgs e) { await App.NavigateToPage(new AdminSettings()); }
        private async void OnLogOutButtonClicked(object sender, EventArgs e) { await App.NavigateToPage(new LoginView()); }
        private async void OnUserSettingsButtonClicked(object sender, EventArgs e) { await App.NavigateToPage(new UserSettings()); }

        private void OnLeaveRequestsButtonClicked(object sender, EventArgs e)
        {
            // add code
        }

        private async void OnApproveRequest(object sender, EventArgs e)
        {
            var button = sender as Button;
            if (button?.BindingContext is LeaveRequest requestToApprove)
            {
                requestToApprove.ApprovalStatus = "Approved";

                await _databaseService.UpdateLeaveRequestAsync(requestToApprove);

                LoadLeaveRequests();

                await DisplayAlert("Success", $"Leave Request for {requestToApprove.EmployeeName} has been approved.", "OK");
            }
        }

        private async void OnDenyRequest(object sender, EventArgs e)
        {
            var button = sender as Button;
            if (button?.BindingContext is LeaveRequest requestToDeny)
            {
                requestToDeny.ApprovalStatus = "Denied";

                await _databaseService.UpdateLeaveRequestAsync(requestToDeny);

                LoadLeaveRequests();

                await DisplayAlert("Success", $"Leave Request for {requestToDeny.EmployeeName} has been denied.", "OK");
            }
        }
    }
}
