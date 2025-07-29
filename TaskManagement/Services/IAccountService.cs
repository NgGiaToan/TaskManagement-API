using TaskManagement.DbContexts;
using TaskManagement.Models;

namespace TaskManagement.Services
{
    public interface IAccountService
    {
        Task<ApplicationAccount> CreateUser(CreateAccount value);
        Task<ApplicationAccount> UpdateUser(CreateAccount value, Guid id);
        Task<bool> DeleteUser(Guid id);
    }
}
