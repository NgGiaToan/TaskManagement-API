using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.DbContexts;
using TaskManagement.Models;
using TaskManagement.Services;

namespace TaskManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserTaskController : ControllerBase
    {
        private readonly IUserTaskService _userTaskService;
        public UserTaskController(IUserTaskService bookService)
        {
            _userTaskService = bookService;
        }

        [Authorize]
        [HttpGet]
        public async Task<List<UserTasks>> GetUserTasks(Guid? taskId, Guid? userId)
        {
            var userTasks =  await _userTaskService.GetUserTasks(taskId, userId);
            return userTasks;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateTask(Guid taskId, Guid userId)
        {
            UserTasks CreateTask = await _userTaskService.CreateUserTask(taskId, userId);
            return Ok(CreateTask);
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateUserTask(Guid taskId, Guid userId, string status)
        {
            var updateUserTask = await _userTaskService.UpdateUserTask(taskId, userId, status);
            return Ok(updateUserTask);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("delete-by-taskId")]
        public async Task<IActionResult> DeleteByTaskId(Guid taskId)
        {
            var task = await _userTaskService.deleteByTaskId(taskId);
            return Ok(task);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("delete-by-accountId")]
        public async Task<IActionResult> DeleteByAccountId(Guid accountId)
        {
            var task = await _userTaskService.deleteByAccountId(accountId);
            return Ok(task);
        }
    }
}
