using TaskManagement.DbContexts;

namespace TaskManagement.Models
{
    public class CreateTask
    {
        public string TaskName { get; set; }
        public string TaskTitle { get; set; }
        public string TaskDetail { get; set; }
        public string TaskStatus { get; set; }
        public string TaskType { get; set; }
    }
}
