using TaskManagement.DbContexts;
using TaskManagement.Models;

namespace TaskManagement.Services
{
    public interface ITaskService
    {
        Task<int> CountTaskByType(string n);
        Task<List<TaskInformation>> GetAllTasks(Guid? userId, string? type, string? sortBy, string? search);
        Task<TaskInfs> CreateTask(CreateTask taskInf);
        Task<TaskInfs> UpdateTask(CreateTask taskInf,Guid id);
        Task<List<TaskInfs>> TaskInfByType(string type);
        //Task<List<Guid>> TaskIdByType(string type);
        Task<TaskInformation> TaskInfById(Guid id);
        Task<bool> DeleteTask(Guid id);
    }
}
