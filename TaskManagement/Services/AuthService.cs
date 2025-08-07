using Microsoft.AspNetCore.Identity;
using Microsoft.Azure.Management.Graph.RBAC.Fluent.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskManagement.DbContexts;
using TaskManagement.Models;

namespace TaskManagement.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _dbContext;
        private readonly SignInManager<ApplicationAccount> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationAccount> _userManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        public AuthService(AppDbContext dbContext,
            IConfiguration configuration,
            SignInManager<ApplicationAccount> signInManager,
            UserManager<ApplicationAccount> userManager,
            RoleManager<IdentityRole<Guid>> roleManager
            )
        {
            _signInManager = signInManager;
            _dbContext = dbContext;
            _configuration = configuration;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<string> SignInAsync(Signin model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            var passwordValid = await _userManager.CheckPasswordAsync(user, model.Password);

            if (user == null || !passwordValid)
            {
                return null;
            }

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, model.Username),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role.ToString()));
            }

            var authenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                expires: DateTime.Now.AddMinutes(60),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authenKey, SecurityAlgorithms.HmacSha512Signature)
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<(bool Success, string Message, ApplicationAccount? User)> SignUpAsync(CreateAccount value)
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
                    return (false, $"{errors}", null);
                }

                // Kiểm tra role trước khi add
                if (!await _roleManager.RoleExistsAsync("User"))
                {
                    var roleResult = await _roleManager.CreateAsync(new IdentityRole<Guid>("User"));
                    if (!roleResult.Succeeded)
                    {
                        var roleErrors = string.Join("; ", roleResult.Errors.Select(e => e.Description));
                        return (false, $"{roleErrors}", null);
                    }
                }

                await _userManager.AddToRoleAsync(user, "User");

                return (true, "User created successfully.", user);
            }
            catch (Exception ex)
            {
                return (false, $"{ex.Message}", null);
            }
        }
    }
}