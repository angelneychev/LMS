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

    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly ApplicationDbContext dbContext;

        public EmployeeController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            RoleManager<ApplicationRole> roleManager, ApplicationDbContext dbContext)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            this.dbContext = dbContext;
        }

        [HttpGet]
        [Authorize(Roles = "Human Resources")]
        public async Task<IEnumerable<EmployeeReportParameters>> GetEmployeesByCompany()
        {
            var loggedInUserId = userManager.GetUserId(this.User);
            var loggedInUser = await userManager.FindByIdAsync(loggedInUserId);

            if (loggedInUser == null)
            {
                return Enumerable.Empty<EmployeeReportParameters>();
            }

            var loggedInEmployee = await dbContext.Employees
                .Include(e => e.User)
                .Include(e => e.Department)
                .FirstOrDefaultAsync(e => e.UserId == loggedInUserId);

            if (loggedInEmployee == null || loggedInEmployee.CompanyId <= 0)
            {
                return Enumerable.Empty<EmployeeReportParameters>();
            }

            var employees = await dbContext.Employees
                .Include(e => e.User)
                .Include(e => e.Department)
                .Where(e => e.CompanyId == loggedInEmployee.CompanyId)
                .ToListAsync();

            var employeeReportItems = new List<EmployeeReportParameters>();

            foreach (var employee in employees)
            {
                var roles = await userManager.GetRolesAsync(employee.User);
                employeeReportItems.Add(new EmployeeReportParameters
                {
                    FullName = employee.User.FullName,
                    Email = employee.User.Email,
                    CompanyName = dbContext.Companies.FirstOrDefault(c => c.Id == loggedInEmployee.CompanyId)?.Name,
                    Role = string.Join(", ", roles),
                    Department = employee.Department?.Name ?? "N/A"
                });
            }

            return employeeReportItems;
        }
    }
}
