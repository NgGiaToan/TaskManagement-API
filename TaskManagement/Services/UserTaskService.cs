using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
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

        public async Task<List<UserTasks>> GetUserTasks(Guid? taskId, Guid? userId)
        {
            try
            {
                var userTasks = await _dbContext.UserTasks.ToListAsync();
                
                if (taskId.ToString().Length >0)
                {
                    userTasks = userTasks.Where(u => u.TaskId == taskId).ToList();
                }

                if (userId.ToString().Length > 0)
                {
                    userTasks = userTasks.Where(u => u.UserId == userId).ToList();
                }

                return userTasks;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<UserTasks> CreateUserTask(Guid taskId, Guid userId)
        {
            try
            {
                var userTask = new UserTasks
                {
                    UserId = userId,
                    TaskId = taskId,
                    Status = "InProgress",
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
        public async Task<UserTasks> UpdateUserTask(Guid taskId, Guid userId, string status)
        {
            try
            {

                var userTask = await _dbContext.UserTasks.FirstAsync(t=> t.UserId == userId && t.TaskId == taskId);
                if (userTask == null)
                    return null;

                userTask.UserId = userId;
                userTask.TaskId = taskId;
                userTask.Status = status;

                await _dbContext.SaveChangesAsync();

                return userTask;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> deleteByTaskId(Guid taskId)
        {
            try
            {
                var userTasks = await _dbContext.UserTasks
                    .Where(ut => ut.TaskId == taskId)
                    .ToListAsync();
                
                if (userTasks.Any())
                {
                    foreach (var userTask in userTasks)
                    {
                        _dbContext.UserTasks.RemoveRange(userTask);
                        await _dbContext.SaveChangesAsync();
                    }

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> deleteByAccountId(Guid accountId)
        {
            try
            {
                var userTasks = await _dbContext.UserTasks
                    .Where(ut => ut.UserId == accountId)
                    .ToListAsync();

                if (userTasks.Any())
                {
                    foreach (var userTask in userTasks) {
                        _dbContext.UserTasks.RemoveRange(userTasks);
                        await _dbContext.SaveChangesAsync();
                    }

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
