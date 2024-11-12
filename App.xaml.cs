using EmployeeManagementSystem.Services;
using Microsoft.Maui.Controls;

namespace EmployeeManagementSystem
{
    public partial class App : Application
    {
        private static DatabaseService database;
        public static DatabaseService Database => database ??= new DatabaseService();

        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();
        }
    }
}