namespace LMS.Server.Controllers
{
    using LMS.Server.Data;
    using LMS.Server.Models;
    using LMS.Shared.ViewModels.Departments;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;

        public DepartmentController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> CreateDepartment([FromBody] CreateDepartmentParameters parameters)
        {
            var department = new Department
            {
                Name = parameters.Name,
                CompanyId = (int)parameters.CompanyId,
                CreatedOn = DateTime.UtcNow
            };

            dbContext.Departments.Add(department);
            await dbContext.SaveChangesAsync();

            return Ok("Department created successfully.");
        }
    }
}
