namespace LMS.Server.Controllers
{
    using LMS.Server.Data;
    using LMS.Server.Models;
    using LMS.Shared.ViewModels.Companies;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;

    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext dbContext;

        public CompanyController(UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext)
        {
            this.userManager = userManager;
            this.dbContext = dbContext;
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> CreateCompany([FromBody] CreateCompanyParameters parameters)
        {
            var company = new Company
            {
                Name = parameters.Name,
                Address = parameters.Address,
                PhoneNumber = parameters.PhoneNumber,
                Email = parameters.Email,
                TaxIdentificationNumber = parameters.TaxIdentificationNumber,
                CreatedOn = DateTime.UtcNow
            };

            dbContext.Companies.Add(company);
            await dbContext.SaveChangesAsync();

            return Ok("Company created successfully.");
        }

        [HttpGet]
        public IEnumerable<KeyValuePair<string, string>> GetAllCompaniesAsKeyValuePairs() 
            => dbContext.Companies
                .OrderBy(x => x.Name)
                .Select(x => new KeyValuePair<string, string>(x.Id.ToString(), x.Name))
                .ToList();
    }
}
