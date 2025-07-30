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
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly UserManager<ApplicationAccount> _userManager;
<<<<<<< HEAD
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
=======
>>>>>>> a9562b585aa95a188f02a32cc14c17eee065d6c1
        public AccountService(AppDbContext dbContext, UserManager<ApplicationAccount> userManager, RoleManager<IdentityRole<Guid>> roleManager)
        {
            _userManager = userManager;
            _dbContext = dbContext;
            _roleManager = roleManager;
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

        public async Task<(bool Success, string Message, ApplicationAccount? User)> CreateUser(CreateAccount value)
        {
            try
            {
                // Kiểm tra trùng username
                var checkUsername = await _userManager.FindByNameAsync(value.Username);
                if (checkUsername != null)
                {
                    return (false, "Username already exists.", null);
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

                // Tạo user
                var result = await _userManager.CreateAsync(user, value.Password.Trim());

                if (!result.Succeeded)
                {
                    var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                    return (false, $"User creation failed: {errors}", null);
                }

                // Kiểm tra role trước khi add
                if (!await _roleManager.RoleExistsAsync("User"))
                {
                    var roleResult = await _roleManager.CreateAsync(new IdentityRole<Guid>("User"));
                    if (!roleResult.Succeeded)
                    {
                        var roleErrors = string.Join("; ", roleResult.Errors.Select(e => e.Description));
                        return (false, $"Role creation failed: {roleErrors}", null);
                    }
                }

                await _userManager.AddToRoleAsync(user, "User");

                return (true, "User created successfully.", user);
            }
            catch (Exception ex)
            {
                return (false, $"Exception occurred: {ex.Message}", null);
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
