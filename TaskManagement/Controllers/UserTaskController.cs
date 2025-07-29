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

        [HttpPost]
        public async Task<IActionResult> CreateTask(Guid taskId, Guid userId)
        {
            UserTasks CreateTask = await _userTaskService.CreateUserTask(taskId, userId);
            return Ok(CreateTask);
        }
    }
}
