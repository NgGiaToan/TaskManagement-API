using Microsoft.AspNetCore.Identity;
using Microsoft.Azure.Management.Graph.RBAC.Fluent.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TaskManagement.DbContexts;
using TaskManagement.Models;

namespace TaskManagement.Services
{
    public class AccountService : IAccountService
    {
        private readonly AppDbContext _dbContext;
        private readonly UserManager<ApplicationAccount> _userManager;
        public AccountService(AppDbContext dbContext, UserManager<ApplicationAccount> userManager)
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }
        public async Task<int> TotalUsers()
        {
            return (await _userManager.GetUsersInRoleAsync("User")).Count();
        }

        public async Task<ApplicationAccount> ViewUser(Guid id)
        {
            /**var user = await _userManager.Users
                .Where(u => u.Id == id)
                .Select(u => new ApplicationAccount
                {
                    Id = u.Id,
                }).FirstOrDefaultAsync();
            return user;**/
            return null;
        }

        public async Task<ApplicationAccount> CreateUser(CreateAccount value)
        {
            try
            {
                var checkUsername = await _userManager.FindByNameAsync(value.Username);
                if (checkUsername != null)
                {
                    return null;
                }

                var user = new ApplicationAccount
                {
                    Id = Guid.NewGuid(),
                    UserName = value.Username.Trim(),
                    FullName = value.FullName.Trim(),
                    RefreshToken = null,
                    RefreshTokenExpiryTime = null,
                    LastLogin = null,
                    Image = value.Avatar
                };

                var result = await _userManager.CreateAsync(user, value.Password.Trim());

                if (!result.Succeeded)
                {
                    var errors = string.Join("; ", result.Errors.Select(e => e.Description));

                    return null;
                }
                else
                {
                    await _userManager.AddToRoleAsync(user, "User");
                    return user;
                }


            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<ApplicationAccount> UpdateUser(CreateAccount value, Guid id)
        {
            try
            {
                var account = await _userManager.FindByIdAsync(id.ToString());
                if (account == null)
                {
                    return null;
                }

                var username = await _userManager.FindByNameAsync(value.Username);
                if (username != null)
                {
                    return null;
                }

                account.FullName = value.FullName.Trim();
                account.Image = value.Avatar.Trim();
                account.UserName = value.Username.Trim();

                var resetToken = await _userManager.GeneratePasswordResetTokenAsync(account);
                var passwordResult = await _userManager.ResetPasswordAsync(account, resetToken, value.Password.Trim());

                if (!passwordResult.Succeeded)
                {
                    var errors = string.Join("; ", passwordResult.Errors.Select(e => e.Description));
                    return null;
                }

                var updateResult = await _userManager.UpdateAsync(account);
                if (!updateResult.Succeeded)
                {
                    var errors = string.Join("; ", updateResult.Errors.Select(e => e.Description));
                    return null;
                }

                return account;
            }

            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> DeleteUser(Guid id)
        {
            try
            {
                var account = await _userManager.FindByIdAsync(id.ToString());
                if (account == null)
                {
                    return false;

                }

                await _userManager.DeleteAsync(account);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
