namespace Api.Controllers
{
    using Infrastructure.Entities;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using System.Threading.Tasks;

    [ApiController]
    [Route("[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly RoleManager<ApplicationRole> roleManager;

        public RoleController(RoleManager<ApplicationRole> roleManager)
        {
            this.roleManager = roleManager;
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] string roleName)
        {
            if (string.IsNullOrEmpty(roleName))
            {
                return BadRequest("Role name is required.");
            }

            var role = new ApplicationRole { Name = roleName };
            var result = await this.roleManager.CreateAsync(role);

            if (result.Succeeded)
            {
                return Ok("Role created successfully.");
            }

            return BadRequest("Error creating role.");
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await this.roleManager.Roles.ToListAsync();
            return Ok(roles);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoleById(string id)
        {
            var role = await this.roleManager.FindByIdAsync(id);

            if (role == null)
            {
                return NotFound("Role not found.");
            }

            return Ok(role);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRole(string id, [FromBody] string newRoleName)
        {
            if (string.IsNullOrEmpty(newRoleName))
            {
                return BadRequest("Role name is required.");
            }

            var role = await this.roleManager.FindByIdAsync(id);

            if (role == null)
            {
                return NotFound("Role not found.");
            }

            role.Name = newRoleName;
            var result = await this.roleManager.UpdateAsync(role);

            if (result.Succeeded)
            {
                return Ok("Role updated successfully.");
            }

            return BadRequest("Error updating role.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await this.roleManager.FindByIdAsync(id);

            if (role == null)
            {
                return NotFound("Role not found.");
            }

            var result = await this.roleManager.DeleteAsync(role);

            if (result.Succeeded)
            {
                return Ok("Role deleted successfully.");
            }

            return BadRequest("Error deleting role.");
        }
    }
}
