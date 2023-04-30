namespace LMS.Server.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

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
                CreatedOn = DateTime.UtcNow,
            };

            this.dbContext.Departments.Add(department);
            await this.dbContext.SaveChangesAsync();

            return this.Ok("Department created successfully.");
        }

        [HttpGet]
        public IEnumerable<KeyValuePair<string, string>> GetAllDepartmentsAsKeyValuePairs()
            => this.dbContext.Departments
                 .OrderBy(x => x.Name)
                 .Select(x => new KeyValuePair<string, string>(x.Id.ToString(), x.Name))
                 .ToList();
    }
}
