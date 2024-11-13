using EmployeeManagementSystem.Services;
using EmployeeManagementSystem.Views;
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

            // Setting up the initial page to LoginView using a NavigationPage
            MainPage = new NavigationPage(new LoginView());
        }

        // A helper method to simplify navigation
        public static async Task NavigateToPage(Page page)
        {
            if (Current.MainPage is NavigationPage navigationPage)
            {
                await navigationPage.PushAsync(page);
            }
        }

        // A helper method for navigating back
        public static async Task GoBack()
        {
            if (Current.MainPage is NavigationPage navigationPage)
            {
                await navigationPage.PopAsync();
            }
        }
    }
}
