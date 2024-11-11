using SQLite;

namespace EmployeeManagementSystem.Models
{
    [Table("UserAccounts")]
    public class UserAccount
    {
        [PrimaryKey, AutoIncrement]
        public int UserID { get; set; }

        [Unique, NotNull]
        public string? Email { get; set; } = "";

        [NotNull]
        public string? PasswordHash { get; set; } = "";

        public string Role { get; set; } = "User";
    }
}
