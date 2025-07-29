using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        [HttpPost]
        public async Task<ApplicationAccount> CreateUser(CreateAccount value)
        {
            ApplicationAccount account = await _accountService.CreateUser(value);
            return account;
        }

        [HttpPut]
        public async Task<ApplicationAccount> UpdateUser(CreateAccount value, Guid id)
        {
            ApplicationAccount account = await _accountService.UpdateUser(value,id);
            return account;
        }

        [HttpDelete]
        public async Task<bool> DeleteUser(Guid id)
        {
            var account = await _accountService.DeleteUser(id);
            return true;
        }
    }
}
