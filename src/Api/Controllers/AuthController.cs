namespace Api.Controllers
{
    using Infrastructure.DTOs.User;
    using Infrastructure.DTOs;
    using Infrastructure.Entities;
    using Infrastructure.Repositories;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IUserService userService;

        public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IUserService userService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.userService = userService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var user = await this.userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
            {
                return BadRequest("Invalid email or password.");
            }

            var signInResult = await this.signInManager.PasswordSignInAsync(user, loginDto.Password, false, false);

            if (signInResult.Succeeded)
            {
                if (user.IsFirstLogin)
                {
                    user.IsFirstLogin = false;
                    await this.userManager.UpdateAsync(user);
                    return StatusCode(210, "First login, password change required.");
                }
                else
                {
                    return Ok("Logged in successfully.");
                }
            }
            else
            {
                return BadRequest("Invalid email or password.");
            }
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await this.signInManager.SignOutAsync();
            return Ok("Logged out successfully.");
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
        {
            var user = await this.userManager.FindByEmailAsync(changePasswordDto.Email);
            if (user == null)
            {
                return BadRequest("User not found.");
            }

            var changePasswordResult = await this.userManager.ChangePasswordAsync(user, changePasswordDto.OldPassword, changePasswordDto.NewPassword);
            if (changePasswordResult.Succeeded)
            {
                return Ok("Password changed successfully.");
            }
            else
            {
                return BadRequest("Error changing password.");
            }
        }

        [Authorize(Roles = "Administrator,HR")]
        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserDto registerUserDto)
        {
            var currentUser = await this.userManager.GetUserAsync(User);

            var registerUserResult = await this.userService.RegisterUserAsync(currentUser, registerUserDto);

            if (registerUserResult.Succeeded)
            {
                return Ok("User registered successfully.");
            }
            else
            {
                return BadRequest(registerUserResult.Errors);
            }
        }
    }
}
