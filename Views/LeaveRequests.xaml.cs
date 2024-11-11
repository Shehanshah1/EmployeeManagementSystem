using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using EmployeeManagementSystem.Models;

namespace EmployeeManagementSystem.LeaveRequests
{
    public partial class LeaveRequests : ContentPage
    {
        public ObservableCollection<LeaveRequest> LeaveRequestsList { get; set; }
        public ICommand AddLeaveRequestCommand { get; set; }
        public ICommand EditLeaveRequestCommand { get; set; }
        public ICommand DeleteLeaveRequestCommand { get; set; }

        public LeaveRequests()
        {
            InitializeComponent();
            LeaveRequestsList = new ObservableCollection<LeaveRequest>();
            AddLeaveRequestCommand = new Command(AddLeaveRequest);
            EditLeaveRequestCommand = new Command(EditLeaveRequest);
            DeleteLeaveRequestCommand = new Command(DeleteLeaveRequest);
            BindingContext = this;
        }

        private async void OnDashboardButtonClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//Dashboard");
        }

        private async void OnEmployeeManagementButtonClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//EmployeeManagement");
        }

        private async void OnAdminSettingsButtonClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//AdminSettings");
        }

        private async void OnLogOutButtonClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//LoginView");
        }

        private async void OnUserSettingsButtonClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//UserSettings");
        }

        private void AddLeaveRequest()
        {
            LeaveRequestsList.Add(new LeaveRequest { EmployeeID = 1, StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(2), Reason = "Personal Leave", ApprovalStatus = "Pending" });
        }

        private void EditLeaveRequest()
        {
            if (LeaveRequestsList.Count > 0)
                LeaveRequestsList[0].Reason = "Updated Reason";
        }

        private void DeleteLeaveRequest()
        {
            if (LeaveRequestsList.Count > 0)
                LeaveRequestsList.RemoveAt(0);
        }
    }
}
