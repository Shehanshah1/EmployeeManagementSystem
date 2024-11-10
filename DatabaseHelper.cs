using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace EmployeeManagementSystem
{
    public class DatabaseHelper
    {
        private readonly string connectionString;

        public DatabaseHelper(string dbPath)
        {
            connectionString = $"Data Source={dbPath};Version=3;";
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string createEmployeesTable = @"CREATE TABLE IF NOT EXISTS Employees (
                                                    EmployeeID INTEGER PRIMARY KEY AUTOINCREMENT,
                                                    Name TEXT NOT NULL,
                                                    DepartmentID INTEGER NOT NULL,
                                                    Position TEXT,
                                                    DateJoined TEXT);";

                string createDepartmentsTable = @"CREATE TABLE IF NOT EXISTS Departments (
                                                      DepartmentID INTEGER PRIMARY KEY AUTOINCREMENT,
                                                      DepartmentName TEXT NOT NULL);";

                string createLeaveRequestsTable = @"CREATE TABLE IF NOT EXISTS LeaveRequests (
                                                        LeaveRequestID INTEGER PRIMARY KEY AUTOINCREMENT,
                                                        EmployeeID INTEGER NOT NULL,
                                                        SubmissionDate TEXT,
                                                        StartDate TEXT,
                                                        EndDate TEXT,
                                                        LeaveType TEXT,
                                                        ApprovalStatus TEXT,
                                                        FOREIGN KEY(EmployeeID) REFERENCES Employees(EmployeeID));";

                using (var command = new SQLiteCommand(createEmployeesTable, connection))
                {
                    command.ExecuteNonQuery();
                }
                using (var command = new SQLiteCommand(createDepartmentsTable, connection))
                {
                    command.ExecuteNonQuery();
                }
                using (var command = new SQLiteCommand(createLeaveRequestsTable, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        // Employee management methods (Add, Search, etc.)
        public void AddEmployee(Employee employee)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                var command = new SQLiteCommand("INSERT INTO Employees (Name, DepartmentID, Position, DateJoined) VALUES (@Name, @DepartmentID, @Position, @DateJoined)", connection);
                command.Parameters.AddWithValue("@Name", employee.Name);
                command.Parameters.AddWithValue("@DepartmentID", employee.DepartmentID);
                command.Parameters.AddWithValue("@Position", employee.Position);
                command.Parameters.AddWithValue("@DateJoined", employee.DateJoined.ToString("yyyy-MM-dd"));
                command.ExecuteNonQuery();
            }
        }

        public List<Employee> SearchEmployees(string name = null, int? departmentId = null)
        {
            var employees = new List<Employee>();
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                var query = "SELECT * FROM Employees WHERE 1=1";
                if (!string.IsNullOrEmpty(name))
                    query += " AND Name LIKE @Name";
                if (departmentId.HasValue)
                    query += " AND DepartmentID = @DepartmentID";

                var command = new SQLiteCommand(query, connection);
                if (!string.IsNullOrEmpty(name))
                    command.Parameters.AddWithValue("@Name", "%" + name + "%");
                if (departmentId.HasValue)
                    command.Parameters.AddWithValue("@DepartmentID", departmentId);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        employees.Add(new Employee
                        {
                            EmployeeID = Convert.ToInt32(reader["EmployeeID"]),
                            Name = reader["Name"].ToString(),
                            DepartmentID = Convert.ToInt32(reader["DepartmentID"]),
                            Position = reader["Position"].ToString(),
                            DateJoined = DateTime.Parse(reader["DateJoined"].ToString())
                        });
                    }
                }
            }
            return employees;
        }

        // Leave request management methods (Add, Get, Update status)
        public void AddLeaveRequest(LeaveRequest request)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                var command = new SQLiteCommand("INSERT INTO LeaveRequests (EmployeeID, SubmissionDate, StartDate, EndDate, LeaveType, ApprovalStatus) VALUES (@EmployeeID, @SubmissionDate, @StartDate, @EndDate, @LeaveType, @ApprovalStatus)", connection);
                command.Parameters.AddWithValue("@EmployeeID", request.EmployeeID);
                command.Parameters.AddWithValue("@SubmissionDate", request.SubmissionDate.ToString("yyyy-MM-dd"));
                command.Parameters.AddWithValue("@StartDate", request.StartDate.ToString("yyyy-MM-dd"));
                command.Parameters.AddWithValue("@EndDate", request.EndDate.ToString("yyyy-MM-dd"));
                command.Parameters.AddWithValue("@LeaveType", request.LeaveType);
                command.Parameters.AddWithValue("@ApprovalStatus", request.ApprovalStatus);
                command.ExecuteNonQuery();
            }
        }

        public List<LeaveRequest> GetLeaveRequests(int employeeId)
        {
            var leaveRequests = new List<LeaveRequest>();
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                var command = new SQLiteCommand("SELECT * FROM LeaveRequests WHERE EmployeeID = @EmployeeID", connection);
                command.Parameters.AddWithValue("@EmployeeID", employeeId);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        leaveRequests.Add(new LeaveRequest
                        {
                            LeaveRequestID = Convert.ToInt32(reader["LeaveRequestID"]),
                            EmployeeID = Convert.ToInt32(reader["EmployeeID"]),
                            SubmissionDate = DateTime.Parse(reader["SubmissionDate"].ToString()),
                            StartDate = DateTime.Parse(reader["StartDate"].ToString()),
                            EndDate = DateTime.Parse(reader["EndDate"].ToString()),
                            LeaveType = reader["LeaveType"].ToString(),
                            ApprovalStatus = reader["ApprovalStatus"].ToString()
                        });
                    }
                }
            }
            return leaveRequests;
        }

        public void UpdateLeaveRequestStatus(int leaveRequestId, string newStatus)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                var command = new SQLiteCommand("UPDATE LeaveRequests SET ApprovalStatus = @ApprovalStatus WHERE LeaveRequestID = @LeaveRequestID", connection);
                command.Parameters.AddWithValue("@ApprovalStatus", newStatus);
                command.Parameters.AddWithValue("@LeaveRequestID", leaveRequestId);
                command.ExecuteNonQuery();
            }
        }
    }
}

