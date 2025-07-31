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
        public async Task<IActionResult> GetCount([FromQuery] string n)
        {
            int totalTask = await _taskService.CountTaskByType(n);
            return Ok(totalTask);
        }

        /***[Authorize(Roles = "Admin")]
        [HttpGet("task-inf")]
        public async Task<IActionResult> GetTask(string n)
        {
            List<TaskInfs> totalTask = await _taskService.TaskInfByType(n);
            return Ok(totalTask);
        }**/

        //[Authorize(Roles = "Admin")]
        [HttpGet("list-id")]
        public async Task<IActionResult> GetListTask(string n)
        {
            List<Guid> totalTask = await _taskService.TaskIdByType(n);
            return Ok(totalTask);
        }

        //[Authorize]
        [HttpGet("task-by-id")]
        public async Task<IActionResult> GetTaskId(Guid n)
        {
            TaskInformation totalTask = await _taskService.TaskInfById(n);
            return Ok(totalTask);
        }

        //[Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateTask(CreateTask task)
        {
            TaskInfs CreateTask = await _taskService.CreateTask(task);
            return Ok(CreateTask);
        }

        //[Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateTask(CreateTask task, Guid id)
        {
            TaskInfs UpdateTask = await _taskService.UpdateTask(task,id);
            return Ok(UpdateTask);
        }

        //[Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public IActionResult DeleteTask(Guid id)
        {
            _taskService.DeleteTask(id);
            return Ok(new { message = "Task deleted successfully" });
        }
    }
}



