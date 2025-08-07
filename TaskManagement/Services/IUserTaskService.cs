using TaskManagement.DbContexts;

namespace TaskManagement.Services
{
    public interface IUserTaskService
    {
        Task<List<UserTasks>> GetUserTasks(Guid? taskId, Guid? userId);
        Task<UserTasks> CreateUserTask(Guid taskId, Guid userId);
        Task<UserTasks> UpdateUserTask(Guid taskId, Guid userId, string status);
        Task<bool> deleteByTaskId(Guid taskId);
        Task<bool> deleteByAccountId(Guid accountId);
    }
}
