namespace LMS.Server.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using LMS.Server.Data;
    using LMS.Server.Models;
    using LMS.Shared.ViewModels.Employees;
    using LMS.Shared.ViewModels.UsersInRoles;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthorizeController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly ApplicationDbContext dbContext;

        public AuthorizeController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            RoleManager<ApplicationRole> roleManager, ApplicationDbContext dbContext)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            this.dbContext = dbContext;
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginParameters parameters)
        {
            var user = await this.userManager.FindByNameAsync(parameters.UserName);

            if (user == null)
            {
                return this.BadRequest("User does not exist");
            }

            var singInResult = await this.signInManager.CheckPasswordSignInAsync(user, parameters.Password, false);

            if (!singInResult.Succeeded)
            {
                return this.BadRequest("Invalid password");
            }

            await this.signInManager.SignInAsync(user, parameters.RememberMe);

            return this.Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterParameters parameters)
        {
            var user = new ApplicationUser
            {
                UserName = parameters.UserName,
                Email = parameters.UserName,
                UniqueCitizenshipNumber = parameters.UniqueCitizenshipNumber,
                FullName = parameters.FullName,
                CreatedOn = DateTime.UtcNow,
                IsFirstLogin = false,
            };

            var result = await this.userManager.CreateAsync(user, parameters.Password);

            if (result.Succeeded)
            {
                // Add this code before creating the Employee object
                if (parameters.CompanyId <= 0)
                {
                    return this.BadRequest("Company not selected or invalid.");
                }

                // Create the Employee object
                var employee = new Employee
                {
                    UserId = user.Id,
                    CompanyId = parameters.CompanyId,
                    HiredDate = DateTime.UtcNow,
                    CreatedOn = DateTime.UtcNow,
                    Position = string.Empty,
                };

                // Add the Employee object to the database
                this.dbContext.Employees.Add(employee);
                await this.dbContext.SaveChangesAsync();

                if (!string.IsNullOrEmpty(parameters.Role))
                {
                    var role = await this.roleManager.FindByIdAsync(parameters.Role);

                    if (role != null && role.Name != "Administrator")
                    {
                        var roleResult = await this.userManager.AddToRoleAsync(user, role.Name);
                        if (!roleResult.Succeeded)
                        {
                            return this.BadRequest(roleResult.Errors.FirstOrDefault()?.Description);
                        }
                    }
                    else
                    {
                        return this.BadRequest("Role does not exist or is not allowed.");
                    }
                }

                await this.signInManager.SignInAsync(user, isPersistent: false);

                return this.Ok("User registered successfully.");
            }

            return this.BadRequest(result.Errors.FirstOrDefault()?.Description);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await this.signInManager.SignOutAsync();

            return this.Ok();
        }

        [HttpGet]
        public UserInfo UserInfo()
        {
            return this.BuildUserInfo();
        }

        private UserInfo BuildUserInfo()
        {
            return new UserInfo
            {
                IsAuthenticated = this.User.Identity.IsAuthenticated,
                UserName = this.User.Identity.Name,
                ExposedClaims = this.User.Claims.ToDictionary(c => c.Type, c => c.Value),
            };
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleParameters parameters)
        {
            if (string.IsNullOrWhiteSpace(parameters.RoleName))
            {
                return this.BadRequest("Role name is required.");
            }

            var role = new ApplicationRole { Name = parameters.RoleName };

            var result = await this.roleManager.CreateAsync(role);

            if (result.Succeeded)
            {
                return this.Ok("Role created successfully!");
            }

            return this.BadRequest(result.Errors.FirstOrDefault()?.Description);
        }

        [HttpGet]
        public IEnumerable<KeyValuePair<string, string>> GetAllRolesAsKeyValuePairs()
            => this.roleManager.Roles
               .OrderBy(x => x.Name)
               .Select(x => new KeyValuePair<string, string>(x.Id.ToString(), x.Name ?? string.Empty))
               .ToList();

        [HttpPost]
        public async Task<IActionResult> RegisterUserAndEmployee(RegisterUserAndEmployeeParameters parameters)
        {
            // Retrieve the currently logged-in user
            var loggedInUserId = this.userManager.GetUserId(this.User);
            var loggedInUser = await this.userManager.FindByIdAsync(loggedInUserId);

            // Check if the current user has the "Human Resources" role
            if (loggedInUser == null || !await this.userManager.IsInRoleAsync(loggedInUser, "Human Resources"))
            {
                return this.BadRequest("HR user not found or invalid.");
            }

            var user = new ApplicationUser
            {
                UserName = parameters.UserName,
                Email = parameters.UserName,
                UniqueCitizenshipNumber = parameters.UniqueCitizenshipNumber,
                FullName = parameters.FullName,
                PhoneNumber = parameters.PhoneNumber,
                CreatedOn = DateTime.UtcNow,
                IsFirstLogin = false,
            };

            var result = await this.userManager.CreateAsync(user, parameters.Password);

            if (result.Succeeded)
            {
                if (parameters.CompanyId <= 0)
                {
                    return this.BadRequest("Company not selected or invalid.");
                }

                if (!string.IsNullOrEmpty(parameters.Role))
                {
                    var role = await this.roleManager.FindByIdAsync(parameters.Role);

                    if (role != null && role.Name != "Administrator")
                    {
                        var roleResult = await this.userManager.AddToRoleAsync(user, role.Name);

                        if (!roleResult.Succeeded)
                        {
                            return this.BadRequest(roleResult.Errors.FirstOrDefault()?.Description);
                        }
                    }
                    else
                    {
                        return this.BadRequest("Role does not exist or is not allowed.");
                    }
                }

                var employee = new Employee
                {
                    UserId = user.Id,
                    CompanyId = parameters.CompanyId,
                    DepartmentId = parameters.DepartmentId,
                    Position = parameters.Position,
                    HiredDate = parameters.HiredDate,
                    CreatedOn = DateTime.UtcNow,
                };

                this.dbContext.Employees.Add(employee);
                await this.dbContext.SaveChangesAsync();

                await this.signInManager.SignInAsync(user, isPersistent: false);

                return this.Ok("User and employee registered successfully.");
            }

            return this.BadRequest(result.Errors.FirstOrDefault()?.Description);
        }
    }
}
