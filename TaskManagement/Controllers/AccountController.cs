using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TaskManagement.DbContexts;
using TaskManagement.Models;
using TaskManagement.Services;

namespace TaskManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        //[Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateAccount value)
        {
            var result = await _accountService.CreateUser(value);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.User);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var accounts = await _accountService.UserList();
            return Ok(accounts);
        }

        /***[Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateUser(CreateAccount value, Guid id)
        {
            ApplicationAccount account = await _accountService.UpdateUser(value,id);
            return Ok(account);
        }***/


        [Authorize(Roles = "Admin")] 
        [HttpDelete]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var account = await _accountService.DeleteUser(id);
            return Ok();
        }
    }
}
