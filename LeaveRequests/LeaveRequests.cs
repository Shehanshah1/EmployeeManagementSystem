using System;

namespace EmployeeManagementSystem
{
    public class LeaveRequest
    {
        public int LeaveRequestID { get; set; }
        public int EmployeeID { get; set; }
        public DateTime SubmissionDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string LeaveType { get; set; }
        public string ApprovalStatus { get; set; } // e.g., "Pending", "Approved", "Rejected"
    }
}
