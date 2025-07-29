using TaskManagement.DbContexts;

namespace TaskManagement.Services
{
    public interface IUserTaskService
    {
        Task<UserTasks> CreateUserTask(Guid taskId, Guid userId);
    }
}
