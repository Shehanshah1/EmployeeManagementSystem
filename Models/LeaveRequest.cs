using SQLite;
using System;

namespace EmployeeManagementSystem.Models
{
    [Table("LeaveRequests")]
    public class LeaveRequest
    {
        [PrimaryKey, AutoIncrement]
        public int LeaveRequestID { get; set; }

        [NotNull]
        public int EmployeeID { get; set; }

        [NotNull]
        public DateTime StartDate { get; set; }

        [NotNull]
        public DateTime EndDate { get; set; }

        public int days { get; set; }

        public string EmployeeName { get; set; }

        public string Reason { get; set; }
        public string ApprovalStatus { get; set; } = "Pending";
    }
}
