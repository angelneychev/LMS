namespace Infrastructure.Repositories
{
    using Infrastructure.Data;
    using Infrastructure.DTOs.User;
    using Infrastructure.Entities;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly AppDbContext dbContext;

        public UserService(UserManager<ApplicationUser> userManager, AppDbContext dbContext)
        {
            this.userManager = userManager;
            this.dbContext = dbContext;
        }

        public async Task<IdentityResult> RegisterUserAsync(ApplicationUser currentUser, RegisterUserDto registerUserDto)
        {
            var currentUserRoles = await this.userManager.GetRolesAsync(currentUser);

            var isHr = currentUserRoles.Contains("HR");
            var isAdmin = currentUserRoles.Contains("Administrator");

            if (isHr)
            {
                var hrEmployee = await this.dbContext.Employees.FirstOrDefaultAsync(e => e.UserId == currentUser.Id);
                registerUserDto.CompanyId = hrEmployee.CompanyId;
            }
            else if (isAdmin)
            {
                if (!registerUserDto.CompanyId.HasValue || !registerUserDto.DepartmentId.HasValue)
                {
                    return IdentityResult.Failed(new IdentityError { Description = "Company and department are required for administrator registration." });
                }
            }

            var user = new ApplicationUser
            {
                Email = registerUserDto.Email,
                UserName = registerUserDto.Email,
                FullName = registerUserDto.FullName,
                UniqueCitizenshipNumber = registerUserDto.UniqueCitizenshipNumber,
                IsFirstLogin = false
            };

            var createUserResult = await this.userManager.CreateAsync(user, registerUserDto.Password);
            if (!createUserResult.Succeeded)
            {
                return createUserResult;
            }

            var addToRolesResult = await this.userManager.AddToRolesAsync(user, registerUserDto.Roles);
            if (!addToRolesResult.Succeeded)
            {
                return addToRolesResult;
            }

            if (!registerUserDto.DepartmentId.HasValue && registerUserDto.CreateNewDepartment)
            {
                var newDepartment = new Department
                {
                    Name = registerUserDto.NewDepartmentName,
                    CompanyId = registerUserDto.CompanyId.Value
                };

                this.dbContext.Departments.Add(newDepartment);
                await this.dbContext.SaveChangesAsync();
                registerUserDto.DepartmentId = newDepartment.Id;
            }

            var employee = new Employee
            {
                UserId = user.Id,
                CompanyId = registerUserDto.CompanyId.Value,
                DepartmentId = registerUserDto.DepartmentId.Value
            };

            this.dbContext.Employees.Add(employee);
            await this.dbContext.SaveChangesAsync();

            return IdentityResult.Success;
        }
    }
}
