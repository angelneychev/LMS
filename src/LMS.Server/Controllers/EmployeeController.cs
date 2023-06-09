﻿namespace LMS.Server.Controllers
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
                    Id = employee.Id,
                    FullName = employee.User.FullName,
                    Email = employee.User.Email,
                    CompanyName = dbContext.Companies.FirstOrDefault(c => c.Id == loggedInEmployee.CompanyId)?.Name,
                    Role = string.Join(", ", roles),
                    DepartmentId = employee.DepartmentId,
                    Department = employee.Department?.Name ?? "N/A"
                });
            }

            return employeeReportItems;
        }

        [HttpPost]
        [Authorize(Roles = "Human Resources")]
        public async Task<IActionResult> EmployeeRegister(EmployeeRegisterParameters parameters)
        {
            // Retrieve the currently logged-in user
            var loggedInUserId = this.userManager.GetUserId(this.User);
            var loggedInUser = await this.userManager.FindByIdAsync(loggedInUserId);

            // Check if the current user has the "Human Resources" role
            if (loggedInUser == null || !await this.userManager.IsInRoleAsync(loggedInUser, "Human Resources"))
            {
                return this.BadRequest("HR user not found or invalid.");
            }
            // Start a transaction
            using (var transaction = await this.dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    var user = new ApplicationUser
                    {
                        UserName = parameters.UserName,
                        Email = parameters.UserName,
                        UniqueCitizenshipNumber = parameters.UniqueCitizenshipNumber,
                        FullName = parameters.FullName,
                        PhoneNumber = parameters.PhoneNumber,
                        CreatedOn = DateTime.UtcNow,
                        IsFirstLogin = true,
                    };

                    var result = await this.userManager.CreateAsync(user, parameters.Password);

                    if (!result.Succeeded)
                    {
                        return this.BadRequest(result.Errors.FirstOrDefault()?.Description);
                    }

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
                    await transaction.CommitAsync();

                    await this.signInManager.SignInAsync(user, isPersistent: false);

                    return this.Ok("User and employee registered successfully.");
                }
                catch (Exception ex)
                {
                    // If an error occurs, rollback the transaction
                    await transaction.RollbackAsync();

                    return this.BadRequest($"Error occurred while registering user and employee: {ex.Message}");
                }
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateEmployee(EmployeeUpdateParameters employeeUpdateParameters)
        {
            var employee = await this.dbContext.Employees.FindAsync(employeeUpdateParameters.Id);
            if (employee == null)
            {
                return NotFound();
            }

            if (employeeUpdateParameters.RoleId != null)
            {
                var role = await this.roleManager.FindByIdAsync(employeeUpdateParameters.RoleId);
                if (role == null)
                {
                    return BadRequest("Role does not exist.");
                }

                var user = await this.userManager.FindByIdAsync(employee.UserId);
                if (user == null)
                {
                    return BadRequest("User not found.");
                }

                if (role.Name == "Administrator")
                {
                    return BadRequest("Role 'Administrator' cannot be added or removed.");
                }

                var userHasRole = await this.userManager.IsInRoleAsync(user, role.Name);

                if (employeeUpdateParameters.RemoveRole && userHasRole)
                {
                    var roleResult = await this.userManager.RemoveFromRoleAsync(user, role.Name);
                    if (!roleResult.Succeeded)
                    {
                        return BadRequest(roleResult.Errors.FirstOrDefault()?.Description);
                    }
                }
                else if (!employeeUpdateParameters.RemoveRole && !userHasRole)
                {
                    var roleResult = await this.userManager.AddToRoleAsync(user, role.Name);
                    if (!roleResult.Succeeded)
                    {
                        return BadRequest(roleResult.Errors.FirstOrDefault()?.Description);
                    }
                }
            }

            if (employeeUpdateParameters.DepartmentId > 0)
            {
                employee.DepartmentId = employeeUpdateParameters.DepartmentId;
                this.dbContext.Entry(employee).State = EntityState.Modified;
            }
            else
            {
                employee.DepartmentId = null;
                this.dbContext.Entry(employee).State = EntityState.Modified;
            }

            // Add updates for other properties here when needed

            await this.dbContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployeeRole(int id)
        {
            var employee = await dbContext.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            var user = await userManager.FindByIdAsync(employee.UserId);
            if (user == null)
            {
                return BadRequest("User not found.");
            }

            var roles = await userManager.GetRolesAsync(user);
            if (roles.Contains("Administrator"))
            {
                return BadRequest("Role 'Administrator' cannot be removed.");
            }

            foreach (var role in roles)
            {
                var result = await userManager.RemoveFromRoleAsync(user, role);
                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors.FirstOrDefault()?.Description);
                }
            }

            await dbContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployeeDepartment(int id)
        {
            var employee = await dbContext.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            if (employee.DepartmentId == null)
            {
                return BadRequest("Employee is not part of any department.");
            }

            employee.DepartmentId = null;
            dbContext.Entry(employee).State = EntityState.Modified;

            await dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
