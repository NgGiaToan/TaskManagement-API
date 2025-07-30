using TaskManagement.DbContexts;
using TaskManagement.Models;
namespace TaskManagement.Services
{
    public interface IAuthService
    {
        Task<string> SignInAsync(Signin model);
        Task<(bool Success, string Message, ApplicationAccount? User)> SignUpAsync(CreateAccount value);
    }
}
