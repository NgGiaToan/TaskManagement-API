using TaskManagement.DbContexts;

namespace TaskManagement.Models
{
    public class TaskInformation
    {
        public Guid Id { get; set; }
        public string TaskName { get; set; }
        public string TaskTitle { get; set; }
        public string TaskDetail { get; set; }
        public string TaskTime { get; set; }
        public string TaskStatus { get; set; }
        public string TaskType { get; set; }

        public List<ApplicationAccount> Accounts { get; set; }
    }
}
