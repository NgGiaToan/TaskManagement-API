using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskManagement.DbContexts;
using TaskManagement.Models;

namespace TaskManagement.Services
{
    public class UserTaskService : IUserTaskService
    {
        private readonly AppDbContext _dbContext;
        private readonly UserManager<ApplicationAccount> _userManager;

        public UserTaskService(AppDbContext myDbContext, UserManager<ApplicationAccount> userManager)
        {
            _dbContext = myDbContext;
            _userManager = userManager;
        }
        public async Task<UserTasks> CreateUserTask(Guid taskId, Guid userId)
        {
            try
            {
                var userTask = new UserTasks
                {
                    UserId = userId,
                    TaskId = taskId,
                    Status = "To do",
                };

                _dbContext.UserTasks.Add(userTask);
                await _dbContext.SaveChangesAsync();

                return userTask;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
