using EmployeeManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagementSystem.Services
{
    public class DatabaseService
    {
        // In-memory storage for employees and leave requests
        private readonly List<Employee> employees = new();
        private readonly List<LeaveRequest> leaveRequests = new();

        // --------------- Employee Methods ---------------

        /// <summary>
        /// Retrieves all employees.
        /// </summary>
        /// <returns>A list of all employees.</returns>
        public Task<List<Employee>> GetAllEmployeesAsync()
        {
            return Task.FromResult(employees);
        }

        /// <summary>
        /// Retrieves a specific employee by ID.
        /// </summary>
        /// <param name="employeeId">The ID of the employee to retrieve.</param>
        /// <returns>The employee object if found; otherwise, null.</returns>
        public Task<Employee> GetEmployeeByIdAsync(int employeeId)
        {
            var employee = employees.FirstOrDefault(e => e.EmployeeID == employeeId);
            return Task.FromResult(employee);
        }

        /// <summary>
        /// Adds a new employee.
        /// </summary>
        /// <param name="employee">The employee object to add.</param>
        public Task AddEmployeeAsync(Employee employee)
        {
            // Assign a new ID if it doesn't exist
            if (employee.EmployeeID == 0)
            {
                employee.EmployeeID = employees.Count > 0 ? employees.Max(e => e.EmployeeID) + 1 : 1;
            }
            employees.Add(employee);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Updates an existing employee.
        /// </summary>
        /// <param name="employee">The employee object with updated data.</param>
        public Task UpdateEmployeeAsync(Employee employee)
        {
            var existingEmployee = employees.FirstOrDefault(e => e.EmployeeID == employee.EmployeeID);
            if (existingEmployee != null)
            {
                existingEmployee.Name = employee.Name;
                existingEmployee.Department = employee.Department;
                existingEmployee.Position = employee.Position;
            }
            return Task.CompletedTask;
        }

        /// <summary>
        /// Deletes an employee by ID.
        /// </summary>
        /// <param name="employeeId">The ID of the employee to delete.</param>
        public Task DeleteEmployeeAsync(int employeeId)
        {
            var employee = employees.FirstOrDefault(e => e.EmployeeID == employeeId);
            if (employee != null)
            {
                employees.Remove(employee);
            }
            return Task.CompletedTask;
        }

        /// <summary>
        /// Searches employees by name or department.
        /// </summary>
        /// <param name="query">Search query.</param>
        /// <returns>A list of employees matching the query.</returns>
        public Task<List<Employee>> SearchEmployeesAsync(string query)
        {
            var result = employees
                .Where(e => e.Name.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                            e.Department.Contains(query, StringComparison.OrdinalIgnoreCase))
                .ToList();
            return Task.FromResult(result);
        }

        // --------------- Leave Request Methods ---------------

        /// <summary>
        /// Retrieves all leave requests.
        /// </summary>
        /// <returns>A list of all leave requests.</returns>
        public Task<List<LeaveRequest>> GetAllLeaveRequestsAsync()
        {
            return Task.FromResult(leaveRequests);
        }

        /// <summary>
        /// Retrieves a specific leave request by ID.
        /// </summary>
        /// <param name="leaveRequestId">The ID of the leave request to retrieve.</param>
        /// <returns>The leave request object if found; otherwise, null.</returns>
        public Task<LeaveRequest> GetLeaveRequestByIdAsync(int leaveRequestId)
        {
            var leaveRequest = leaveRequests.FirstOrDefault(lr => lr.LeaveRequestID == leaveRequestId);
            return Task.FromResult(leaveRequest);
        }

        /// <summary>
        /// Adds a new leave request.
        /// </summary>
        /// <param name="leaveRequest">The leave request object to add.</param>
        public Task AddLeaveRequestAsync(LeaveRequest leaveRequest)
        {
            // Assign a new ID if it doesn't exist
            if (leaveRequest.LeaveRequestID == 0)
            {
                leaveRequest.LeaveRequestID = leaveRequests.Count > 0 ? leaveRequests.Max(lr => lr.LeaveRequestID) + 1 : 1;
            }
            leaveRequests.Add(leaveRequest);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Updates an existing leave request.
        /// </summary>
        /// <param name="leaveRequest">The leave request object with updated data.</param>
        public Task UpdateLeaveRequestAsync(LeaveRequest leaveRequest)
        {
            var existingRequest = leaveRequests.FirstOrDefault(lr => lr.LeaveRequestID == leaveRequest.LeaveRequestID);
            if (existingRequest != null)
            {
                existingRequest.EmployeeID = leaveRequest.EmployeeID;
                existingRequest.StartDate = leaveRequest.StartDate;
                existingRequest.EndDate = leaveRequest.EndDate;
                existingRequest.Reason = leaveRequest.Reason;
                existingRequest.ApprovalStatus = leaveRequest.ApprovalStatus;
            }
            return Task.CompletedTask;
        }

        /// <summary>
        /// Deletes a leave request by ID.
        /// </summary>
        /// <param name="leaveRequestId">The ID of the leave request to delete.</param>
        public Task DeleteLeaveRequestAsync(int leaveRequestId)
        {
            var leaveRequest = leaveRequests.FirstOrDefault(lr => lr.LeaveRequestID == leaveRequestId);
            if (leaveRequest != null)
            {
                leaveRequests.Remove(leaveRequest);
            }
            return Task.CompletedTask;
        }

        /// <summary>
        /// Searches leave requests by employee name or status.
        /// </summary>
        /// <param name="query">Search query.</param>
        /// <returns>A list of leave requests matching the query.</returns>
        public Task<List<LeaveRequest>> SearchLeaveRequestsAsync(string query)
        {
            var result = leaveRequests
                .Where(lr => lr.Reason.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                             lr.ApprovalStatus.Contains(query, StringComparison.OrdinalIgnoreCase))
                .ToList();
            return Task.FromResult(result);
        }

        /// <summary>
        /// Updates the approval status of a leave request.
        /// </summary>
        /// <param name="leaveRequestId">ID of the leave request.</param>
        /// <param name="status">New status (e.g., "Approved", "Rejected").</param>
        public Task UpdateLeaveRequestStatusAsync(int leaveRequestId, string status)
        {
            var leaveRequest = leaveRequests.FirstOrDefault(lr => lr.LeaveRequestID == leaveRequestId);
            if (leaveRequest != null)
            {
                leaveRequest.ApprovalStatus = status;
            }
            return Task.CompletedTask;
        }
    }
}
