using System.Collections.ObjectModel;

namespace EmployeeManagementSystem.Models
{
    public class Department
    {
        public string DepartmentName { get; set; }
        public ObservableCollection<Employee> Employees { get; set; } = new ObservableCollection<Employee>();
    }
}
