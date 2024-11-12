
using EmployeeManagementSystem.Models;
using Microsoft.Data.Sqlite;
using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmployeeManagementSystem.Services
{
    public class DatabaseService
    {
        private readonly SQLiteAsyncConnection _database;


        public DatabaseService()
        {
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "employees.db3");
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Employee>().Wait();
        }


        // SQLite database connection string
        private readonly string _connectionString = "Data Source=EmployeeManagement.db";


        // Initialize the SQLite database with tables for employees, departments, and leave requests
        private void InitializeDatabase()
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"
                CREATE TABLE IF NOT EXISTS Employees (
                    EmployeeID INTEGER UNIQUE PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL,
                    Department TEXT,
                    Position TEXT,
                    Email TEXT
                );

                CREATE TABLE IF NOT EXISTS Departments (
                    DepartmentID INTEGER UNIQUE PRIMARY KEY AUTOINCREMENT,
                    DepartmentName TEXT NOT NULL
                );

                CREATE TABLE IF NOT EXISTS LeaveRequests (
                    LeaveRequestID INTEGER PRIMARY KEY AUTOINCREMENT,
                    EmployeeID INTEGER,
                    StartDate TEXT,
                    EndDate TEXT,
                    Reason TEXT,
                    ApprovalStatus TEXT,
                    FOREIGN KEY (EmployeeID) REFERENCES Employees (EmployeeID)
                );
            ";
            command.ExecuteNonQuery();
        }

        // --------------- Employee Methods ---------------

        // Retrieve all employees from the database
        public async Task<List<Employee>> GetAllEmployeesAsync()
        {
            var employees = new List<Employee>();
            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM Employees";

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                employees.Add(new Employee
                {
                    EmployeeID = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Department = reader.GetString(2),
                    Position = reader.GetString(3),
                    Email = reader.GetString(4)
                });
            }
            return employees;
        }

        // Add a new employee to the database
        public async Task AddEmployeeAsync(Employee employee)
        {
            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText = @"
                INSERT INTO Employees (Name, Department, Position, Email)
                VALUES ($name, $department, $position, $email)";
            command.Parameters.AddWithValue("$name", employee.Name);
            command.Parameters.AddWithValue("$department", employee.Department);
            command.Parameters.AddWithValue("$position", employee.Position);
            command.Parameters.AddWithValue("$email", employee.Email);

            await command.ExecuteNonQueryAsync();
        }

        // Update an existing employee's details
        public async Task UpdateEmployeeAsync(Employee employee)
        {
            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText = @"
                UPDATE Employees SET
                    Name = $name,
                    Department = $department,
                    Position = $position,
                    Email = $email
                WHERE EmployeeID = $id";
            command.Parameters.AddWithValue("$id", employee.EmployeeID);
            command.Parameters.AddWithValue("$name", employee.Name);
            command.Parameters.AddWithValue("$department", employee.Department);
            command.Parameters.AddWithValue("$position", employee.Position);
            command.Parameters.AddWithValue("$email", employee.Email);

            await command.ExecuteNonQueryAsync();
        }

        public async Task DeleteEmployeeAsync(int employeeId)
        {
            try
            {
                using var connection = new SqliteConnection(_connectionString);
                await connection.OpenAsync();

                var command = connection.CreateCommand();
                command.CommandText = "DELETE FROM Employees WHERE EmployeeID = $id";
                command.Parameters.AddWithValue("$id", employeeId);

                // Execute the command to delete the employee
                await command.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                // Handle exceptions (log, display error message, etc.)
                Console.WriteLine($"Error deleting employee: {ex.Message}");
            }
        }


        // --------------- Department Methods ---------------

        // Retrieve all departments from the database
        public async Task<List<Department>> GetAllDepartmentsAsync()
        {
            var departments = new List<Department>();
            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM Departments";

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                departments.Add(new Department
                {
                    DepartmentID = reader.GetInt32(0),
                    DepartmentName = reader.GetString(1)
                });
            }
            return departments;
        }

        // Add a new department to the database
        public async Task AddDepartmentAsync(Department department)
        {
            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText = @"
                INSERT INTO Departments (DepartmentName)
                VALUES ($name)";
            command.Parameters.AddWithValue("$name", department.DepartmentName);

            await command.ExecuteNonQueryAsync();
        }

        // Update an existing department's name
        public async Task UpdateDepartmentAsync(Department department)
        {
            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText = @"
                UPDATE Departments SET
                    DepartmentName = $name
                WHERE DepartmentID = $id";
            command.Parameters.AddWithValue("$id", department.DepartmentID);
            command.Parameters.AddWithValue("$name", department.DepartmentName);

            await command.ExecuteNonQueryAsync();
        }

        // Delete a department by ID
        public async Task DeleteDepartmentAsync(int departmentId)
        {
            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText = "DELETE FROM Departments WHERE DepartmentID = $id";
            command.Parameters.AddWithValue("$id", departmentId);

            await command.ExecuteNonQueryAsync();
        }

        // --------------- Leave Request Methods ---------------

        // Retrieve all leave requests from the database
        public async Task<List<LeaveRequest>> GetAllLeaveRequestsAsync()
        {
            var leaveRequests = new List<LeaveRequest>();
            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM LeaveRequests";

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                leaveRequests.Add(new LeaveRequest
                {
                    LeaveRequestID = reader.GetInt32(0),
                    EmployeeID = reader.GetInt32(1),
                    StartDate = reader.GetDateTime(2),
                    EndDate = reader.GetDateTime(3),
                    Reason = reader.GetString(4),
                    ApprovalStatus = reader.GetString(5)
                });
            }
            return leaveRequests;
        }

        // Add a new leave request to the database
        public async Task AddLeaveRequestAsync(LeaveRequest leaveRequest)
        {
            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText = @"
                INSERT INTO LeaveRequests (EmployeeID, StartDate, EndDate, Reason, ApprovalStatus)
                VALUES ($employeeId, $startDate, $endDate, $reason, $status)";
            command.Parameters.AddWithValue("$employeeId", leaveRequest.EmployeeID);
            command.Parameters.AddWithValue("$startDate", leaveRequest.StartDate);
            command.Parameters.AddWithValue("$endDate", leaveRequest.EndDate);
            command.Parameters.AddWithValue("$reason", leaveRequest.Reason);
            command.Parameters.AddWithValue("$status", leaveRequest.ApprovalStatus);

            await command.ExecuteNonQueryAsync();
        }

        // Update an existing leave request's details
        public async Task UpdateLeaveRequestAsync(LeaveRequest leaveRequest)
        {
            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText = @"
                UPDATE LeaveRequests SET
                    EmployeeID = $employeeId,
                    StartDate = $startDate,
                    EndDate = $endDate,
                    Reason = $reason,
                    ApprovalStatus = $status
                WHERE LeaveRequestID = $id";
            command.Parameters.AddWithValue("$id", leaveRequest.LeaveRequestID);
            command.Parameters.AddWithValue("$employeeId", leaveRequest.EmployeeID);
            command.Parameters.AddWithValue("$startDate", leaveRequest.StartDate);
            command.Parameters.AddWithValue("$endDate", leaveRequest.EndDate);
            command.Parameters.AddWithValue("$reason", leaveRequest.Reason);
            command.Parameters.AddWithValue("$status", leaveRequest.ApprovalStatus);

            await command.ExecuteNonQueryAsync();
        }

        // Delete a leave request by ID
        public async Task DeleteLeaveRequestAsync(int leaveRequestId)
        {
            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText = "DELETE FROM LeaveRequests WHERE LeaveRequestID = $id";
            command.Parameters.AddWithValue("$id", leaveRequestId);

            await command.ExecuteNonQueryAsync();
        }

    }
}
