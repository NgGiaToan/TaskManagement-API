namespace TaskManagement.DbContexts
{
    public class UserTasks
    {
        public Guid UserId { get; set; }
        public ApplicationAccount User { get; set; }

        public Guid TaskId { get; set; }
        public TaskInfs Task { get; set; }

        public string Status { get; set; }
    }
}
