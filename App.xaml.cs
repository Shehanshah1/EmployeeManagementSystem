using EmployeeManagementSystem.Vieww;

namespace EmployeeManagementSystem
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new LoginView();
        }
    }
}
