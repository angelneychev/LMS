namespace LMS.Server.Controllers
{
    using LMS.Server.Data;
    using LMS.Server.Models;
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

            if (user == null) return BadRequest("User does not exist");

            var singInResult = await this.signInManager.CheckPasswordSignInAsync(user, parameters.Password, false);
            
            if (!singInResult.Succeeded) return BadRequest("Invalid password");

            await this.signInManager.SignInAsync(user, parameters.RememberMe);

            return Ok();
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
                IsFirstLogin = false
            };

            var result = await this.userManager.CreateAsync(user, parameters.Password);

            if (result.Succeeded)
            {

                // Add this code before creating the Employee object
                if (parameters.CompanyId <= 0)
                {
                    return BadRequest("Company not selected or invalid.");
                }

                // Create the Employee object
                var employee = new Employee
                {
                    UserId = user.Id,
                    CompanyId = parameters.CompanyId,
                    HiredDate = DateTime.UtcNow,
                    CreatedOn = DateTime.UtcNow,
                    Position = string.Empty
                };

                // Add the Employee object to the database
                dbContext.Employees.Add(employee);
                await dbContext.SaveChangesAsync();

                if (!string.IsNullOrEmpty(parameters.Role))
                {
                    var role = await roleManager.FindByIdAsync(parameters.Role);

                    if (role != null && role.Name != "Administrator")
                    {
                        var roleResult = await userManager.AddToRoleAsync(user, role.Name);
                        if (!roleResult.Succeeded)
                        {
                            return BadRequest(roleResult.Errors.FirstOrDefault()?.Description);
                        }
                    }
                    else
                    {
                        return BadRequest("Role does not exist or is not allowed.");
                    }
                }

                await signInManager.SignInAsync(user, isPersistent: false);

                return Ok("User registered successfully.");
            }

            return BadRequest(result.Errors.FirstOrDefault()?.Description);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await this.signInManager.SignOutAsync();

            return Ok();
        }

        [HttpGet]
        public UserInfo UserInfo()
        {
            return BuildUserInfo();
        }


        private UserInfo BuildUserInfo()
        {
            return new UserInfo
            {
                IsAuthenticated = User.Identity.IsAuthenticated,
                UserName = User.Identity.Name,
                ExposedClaims = User.Claims.ToDictionary(c => c.Type, c => c.Value)
            };
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleParameters parameters)
        {
            if (string.IsNullOrWhiteSpace(parameters.RoleName))
            {
                return BadRequest("Role name is required.");
            }

            var role = new ApplicationRole { Name = parameters.RoleName };

            var result = await roleManager.CreateAsync(role);

            if (result.Succeeded)
            {
                return Ok("Role created successfully!");
            }

            return BadRequest(result.Errors.FirstOrDefault()?.Description);
        }

        [HttpGet]
        public IEnumerable<KeyValuePair<string, string>> GetAllRolesAsKeyValuePairs() 
            => this.roleManager.Roles
               .OrderBy(x => x.Name)
               .Select(x => new KeyValuePair<string, string>(x.Id.ToString(), x.Name ?? string.Empty))
               .ToList();
    }
}
