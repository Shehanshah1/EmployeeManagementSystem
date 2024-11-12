using System.Collections.ObjectModel;
using System.ComponentModel;

namespace EmployeeManagementSystem.Models
{
    public class Department : INotifyPropertyChanged
    {
        private int _departmentId;
        private string _departmentName;
        private ObservableCollection<Employee> _employees;

        public int DepartmentID
        {
            get => _departmentId;
            set
            {
                if (_departmentId != value)
                {
                    _departmentId = value;
                    OnPropertyChanged(nameof(DepartmentID));
                }
            }
        }

        public string DepartmentName
        {
            get => _departmentName;
            set
            {
                if (_departmentName != value)
                {
                    _departmentName = value;
                    OnPropertyChanged(nameof(DepartmentName));
                }
            }
        }

        public ObservableCollection<Employee> Employees
        {
            get => _employees ??= new ObservableCollection<Employee>();
            set
            {
                if (_employees != value)
                {
                    _employees = value;
                    OnPropertyChanged(nameof(Employees));
                }
            }
        }

        public string CurrentEmployees => Employees.Count.ToString();

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Department()
        {
            _employees = new ObservableCollection<Employee>();
            _employees.CollectionChanged += (s, e) => OnPropertyChanged(nameof(CurrentEmployees));
        }
    }
}
