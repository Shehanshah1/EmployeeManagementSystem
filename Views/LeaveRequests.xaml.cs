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


        public LeaveRequests()
        {
            InitializeComponent();
            _databaseService = new DatabaseService();
            LeaveRequestsList = new ObservableCollection<LeaveRequest>();

         BindingContext = this;
        }



        // Button Click Event Methods
        private async void OnDashboardButtonClicked(object sender, EventArgs e) { await App.NavigateToPage(new Dashboard()); }
        private async void OnEmployeeManagementButtonClicked(object sender, EventArgs e) { await App.NavigateToPage(new EmployeeManagement()); }
        private async void OnAdminSettingsButtonClicked(object sender, EventArgs e) { await App.NavigateToPage(new AdminSettings()); }
        private async void OnLogOutButtonClicked(object sender, EventArgs e) { await App.NavigateToPage(new LoginView()); }
        private async void OnUserSettingsButtonClicked(object sender, EventArgs e) { await App.NavigateToPage(new UserSettings()); }
    }

       
}
