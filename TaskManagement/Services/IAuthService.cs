using TaskManagement.Models;
namespace TaskManagement.Services
{
    public interface IAuthService
    {
        Task<string> SignInAsync(Signin model);
        Task<bool> SignUpAsync(Signup model);
    }
}
