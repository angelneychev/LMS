using LMS.Server.Models;
using LMS.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace LMS.Server.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthorizeController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<ApplicationRole> roleManager;

        public AuthorizeController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<ApplicationRole> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
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
            //var user = await _userManager.GetUserAsync(HttpContext.User);
            return BuildUserInfo();
        }


        private UserInfo BuildUserInfo()
        {
            return new UserInfo
            {
                IsAuthenticated = User.Identity.IsAuthenticated,
                UserName = User.Identity.Name,
                ExposedClaims = User.Claims
                    //Optionally: filter the claims you want to expose to the client
                    //.Where(c => c.Type == "test-claim")
                    .ToDictionary(c => c.Type, c => c.Value)
            };
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleParameters parameters)
        {
            if (string.IsNullOrWhiteSpace(parameters.RoleName))
            {
                return BadRequest("Role name is required.");
            }

            var newRole = new ApplicationRole { Name = parameters.RoleName };

            var result = await roleManager.CreateAsync(newRole);

            if (result.Succeeded)
            {
                return Ok("Role created successfully!");
            }

            return BadRequest(result.Errors.FirstOrDefault()?.Description);
        }

        [HttpGet]
        public async Task<IEnumerable<RoleModel>> GetRolesAsync()
        {
            var roles = roleManager.Roles;
            var roleModels = new List<RoleModel>();

            foreach (var role in roles)
            {
                roleModels.Add(new RoleModel { Id = role.Id, Name = role.Name });
            }

            return roleModels;
        }

        [HttpGet]
        public IEnumerable<KeyValuePair<string, string>> GetAllAsKeyValuePairs()
        {
            return this.roleManager.Roles
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                })
                .OrderBy(x => x.Name)
                .ToList()
                .Select(x => new KeyValuePair<string, string>(x.Id.ToString(), x.Name));
        }
    }
}
