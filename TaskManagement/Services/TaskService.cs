using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Security.Claims;
using TaskManagement.DbContexts;
using TaskManagement.Models;

namespace TaskManagement.Services
{
    public class TaskService : ITaskService
    {
        private readonly AppDbContext _dbContext;
        private readonly UserManager<ApplicationAccount> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public TaskService(AppDbContext myDbContext, UserManager<ApplicationAccount> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = myDbContext;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<int> CountTaskByType(string n)
        {
            int totalTask;

            totalTask = await _dbContext.TaskInfs.Where(t => t.TaskType == n).CountAsync();
            

            return totalTask;
        }

        public async Task<TaskInformation> TaskInfById(Guid id)
        {
            var task = await _dbContext.TaskInfs
            .Where(t => t.Id == id)
            .FirstOrDefaultAsync();

            if (task == null)
                return null;

            var userIds = await _dbContext.UserTasks
                .Where(ut => ut.TaskId == id)
                .Select(ut => ut.UserId)
                .ToListAsync();

            var accounts = new List<ApplicationAccount>();
            foreach (var userId in userIds)
            {
                var user = await _userManager.FindByIdAsync(userId.ToString());
                if (user != null)
                {
                    accounts.Add(new ApplicationAccount
                    {
                        Id = user.Id,
                        Image = user.Image
                    });
                }
            }
            TaskInformation taskDetail = await _dbContext.TaskInfs
                .Where(t => t.Id == id)
                .Select(t => new TaskInformation
                {
                    Id = t.Id,
                    TaskName = t.TaskName,
                    TaskTitle = t.TaskTitle,
                    TaskDetail = t.TaskDetail,
                    TaskTime = t.TaskTime,
                    TaskStatus = t.TaskStatus,
                    TaskType = t.TaskType,
                    Accounts = accounts,
                })
                .FirstOrDefaultAsync();
            return taskDetail;
        }

        public async Task<List<TaskInfs>> TaskInfByType(string type)
        {
            List<TaskInfs> taskInfs = await _dbContext.TaskInfs.Where(t => t.TaskType == type).ToListAsync();
            return taskInfs;
        }

        public async Task<List<Guid>> TaskIdByType(string type)
        {
            List<Guid> listId = await _dbContext.TaskInfs.Where(t => t.TaskType == type).Select(t=> t.Id).ToListAsync();
            return listId;
        }

        public async Task<TaskInfs> CreateTask(CreateTask taskInf)
        {
            try
            {
                var taskId = Guid.NewGuid();
                var task = new TaskInfs
                {
                    Id = taskId,
                    TaskName = taskInf.TaskName,
                    TaskTitle = taskInf.TaskTitle,
                    TaskDetail = taskInf.TaskDetail,
                    TaskTime = taskInf.TaskTime,
                    TaskStatus = "",
                    TaskType = "To do",
                };

                _dbContext.TaskInfs.Add(task);
                await _dbContext.SaveChangesAsync();

                return task;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<TaskInfs> UpdateTask(CreateTask taskInf, Guid id)
        {
            try
            {
                
                var task = await _dbContext.TaskInfs.FindAsync(id);
                if (task == null)
                    return null;

                task.TaskName = taskInf.TaskName;
                task.TaskTitle = taskInf.TaskTitle;
                task.TaskDetail = taskInf.TaskDetail;
                task.TaskTime = taskInf.TaskTime;
                task.TaskStatus = taskInf.TaskStatus;
                task.TaskType = taskInf.TaskType;

                await _dbContext.SaveChangesAsync();

                return task;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async void DeleteTask(Guid id)
        {
            try
            {
                var task = await _dbContext.TaskInfs.FindAsync(id);
                if (task != null)
                {
                    _dbContext.TaskInfs.Remove(task);
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting task: {ex.Message}");
            }
        }
    }
}
