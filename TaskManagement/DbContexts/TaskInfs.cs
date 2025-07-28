namespace TaskManagement.DbContexts
{
    public class TaskInfs
    {
        public Guid Id { get; set; }
        public string Title { get; set; }

        public ICollection<UserTasks> UserTasks { get; set; }

        public TaskInfs()
        {
            UserTasks = new List<UserTasks>();
        }
    }
}
