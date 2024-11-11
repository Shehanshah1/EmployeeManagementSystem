using EmployeeManagementSystem.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmployeeManagementSystem.Services
{
    public class DatabaseService
    {
        private readonly List<Employee> employees = new();

        public Task<List<Employee>> GetAllEmployeesAsync()
        {
            return Task.FromResult(employees);
        }

        public Task AddEmployeeAsync(Employee employee)
        {
            employees.Add(employee);
            return Task.CompletedTask;
        }

        public Task UpdateEmployeeAsync(Employee employee)
        {
            var existingEmployee = employees.Find(e => e.EmployeeID == employee.EmployeeID);
            if (existingEmployee != null)
            {
                existingEmployee.Name = employee.Name;
                existingEmployee.Department = employee.Department;
            }
            return Task.CompletedTask;
        }

        public Task DeleteEmployeeAsync(int employeeId)
        {
            var employee = employees.Find(e => e.EmployeeID == employeeId);
            if (employee != null) employees.Remove(employee);
            return Task.CompletedTask;
        }
    }
}
