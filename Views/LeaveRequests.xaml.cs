using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using EmployeeManagementSystem.Models;
using EmployeeManagementSystem.Services;
using Microsoft.Maui.Controls;

namespace EmployeeManagementSystem.LeaveRequests
{
    public partial class LeaveRequests : ContentPage
    {
        private readonly DatabaseService _databaseService;
        public ObservableCollection<LeaveRequest> LeaveRequestsList { get; set; }
        public ICommand AddLeaveRequestCommand { get; set; }
        public ICommand EditLeaveRequestCommand { get; set; }
        public ICommand DeleteLeaveRequestCommand { get; set; }

        public LeaveRequests()
        {
            InitializeComponent();
            _databaseService = new DatabaseService();
            LeaveRequestsList = new ObservableCollection<LeaveRequest>();

            AddLeaveRequestCommand = new Command(AddLeaveRequest);
            EditLeaveRequestCommand = new Command(EditLeaveRequest);
            DeleteLeaveRequestCommand = new Command(DeleteLeaveRequest);

            LoadLeaveRequests();
            BindingContext = this;
        }

        // Load Leave Requests from Database
        private async void LoadLeaveRequests()
        {
            var requests = await _databaseService.GetAllLeaveRequestsAsync();
            LeaveRequestsList.Clear();
            foreach (var request in requests)
            {
                LeaveRequestsList.Add(request);
            }
        }

        // Button Click Event Methods
        private async void OnDashboardButtonClicked(object sender, EventArgs e) => await Shell.Current.GoToAsync("//Dashboard");
        private async void OnEmployeeManagementButtonClicked(object sender, EventArgs e) => await Shell.Current.GoToAsync("//EmployeeManagement");
        private async void OnAdminSettingsButtonClicked(object sender, EventArgs e) => await Shell.Current.GoToAsync("//AdminSettings");
        private async void OnLogOutButtonClicked(object sender, EventArgs e) => await Shell.Current.GoToAsync("//LoginView");
        private async void OnUserSettingsButtonClicked(object sender, EventArgs e) => await Shell.Current.GoToAsync("//UserSettings");

        private async void AddLeaveRequest()
        {
            var newRequest = new LeaveRequest
            {
                EmployeeID = 1,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(2),
                Reason = "Personal Leave",
                ApprovalStatus = "Pending"
            };
            await _databaseService.AddLeaveRequestAsync(newRequest);
            LeaveRequestsList.Add(newRequest);
        }

        private async void EditLeaveRequest()
        {
            if (LeaveRequestsList.Count == 0) return;
            var requestToUpdate = LeaveRequestsList[0]; // Example: edit first item
            requestToUpdate.Reason = "Updated Reason";
            await _databaseService.UpdateLeaveRequestAsync(requestToUpdate);
        }

        private async void DeleteLeaveRequest()
        {
            if (LeaveRequestsList.Count == 0) return;
            var requestToDelete = LeaveRequestsList[0]; // Example: delete first item
            await _databaseService.DeleteLeaveRequestAsync(requestToDelete.LeaveRequestID);
            LeaveRequestsList.Remove(requestToDelete);
        }

        // Button Click Handlers in XAML
        private void OnAddNewLeaveRequestButtonClicked(object sender, EventArgs e) => AddLeaveRequest();
        private void OnEditSelectedRequestButtonClicked(object sender, EventArgs e) => EditLeaveRequest();
        private void OnDeleteSelectedRequestClicked(object sender, EventArgs e) => DeleteLeaveRequest();
    }
}
