using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.DbContexts;
using TaskManagement.Models;
using TaskManagement.Services;

namespace TaskManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;
        public TaskController(ITaskService bookService)
        {
            _taskService = bookService;
        }

        //[Authorize]
        [HttpGet("count-by-type")]
        public async Task<IActionResult> GetCount([FromQuery] string type)
        {
            int totalTask = await _taskService.CountTaskByType(type);
            return Ok(totalTask);
        }

        [Authorize]
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllTask(Guid? userId, string? type, string? sortBy, string? search)
        {
            List<TaskInformation> totalTask = await _taskService.GetAllTasks(userId, type, sortBy, search);
            return Ok(totalTask);
        }

        //[Authorize(Roles = "Admin")]
        //[HttpGet("list-id")]
        //public async Task<IActionResult> GetListTask(string n)
        //{
            //List<Guid> totalTask = await _taskService.TaskIdByType(n);
            //return Ok(totalTask);
        //}

        [Authorize]
        [HttpGet("task-by-id")]
        public async Task<IActionResult> GetTaskId(Guid id)
        {
            TaskInformation totalTask = await _taskService.TaskInfById(id);
            return Ok(totalTask);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<TaskInfs> CreateTask(CreateTask task)
        {
            TaskInfs CreateTask = await _taskService.CreateTask(task);
            return CreateTask;
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateTask(CreateTask task, Guid id)
        {
            TaskInfs UpdateTask = await _taskService.UpdateTask(task,id);
            return Ok(UpdateTask);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(Guid id)
        {
            var isDeleted = await _taskService.DeleteTask(id);
            
            if (!isDeleted)
            {
                return Ok(new { message = "Task deleted unsuccessfully" });
            }

            return Ok(new { message = "Task deleted successfully" });
        }
    }
}



