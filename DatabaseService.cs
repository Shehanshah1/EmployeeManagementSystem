
using EmployeeManagementSystem.Models;
using Microsoft.Data.Sqlite;
using Microsoft.Maui.Controls;
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
                    EmployeeID INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL,
                    Department TEXT,
                    Position TEXT,
                    Email TEXT
                );

                CREATE TABLE IF NOT EXISTS Departments (
                    DepartmentID INTEGER PRIMARY KEY AUTOINCREMENT,
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
    }
}
