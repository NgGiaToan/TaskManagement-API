using Microsoft.AspNetCore.Identity;

namespace TaskManagement.DbContexts
{
    public class ApplicationAccount : IdentityUser<Guid>
    {
        public string? FullName { get; set; }
        public string? Image { get; set; }

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
