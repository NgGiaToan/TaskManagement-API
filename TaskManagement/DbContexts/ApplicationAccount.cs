using Microsoft.AspNetCore.Identity;

namespace TaskManagement.DbContexts
{
    public enum AccountStatus { Active, Inactive }
    public class ApplicationAccount : IdentityUser<Guid>
    {
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? Contact { get; set; }
        public string? Address { get; set; }
        public DateTime Createddate { get; set; } = DateTime.Now;
        public AccountStatus Status { get; set; } = AccountStatus.Active;
        public string? Note { get; set; }

        public ICollection<UserTasks> UserTasks { get; set; }
        public ApplicationAccount()
        {
            UserTasks = new List<UserTasks>();
        }

        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
        public DateTime? LastLogin { get; set; }
    }
}
