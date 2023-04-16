namespace Api.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Infrastructure.DTOs.Company;
    using Infrastructure.Repositories;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("[controller]")]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyRepository companyRepository;

        public CompanyController(ICompanyRepository companyRepository)
        {
            this.companyRepository = companyRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCompany(CompanyCreateDto companyDto)
        {
            var company = await companyRepository.CreateCompanyAsync(companyDto);
            return CreatedAtAction(nameof(GetCompanyById), new { id = company.Id }, company);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCompanyById(int id)
        {
            var company = await companyRepository.GetCompanyByIdAsync(id);
            if (company == null) return NotFound();
            return Ok(company);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCompanies()
        {
            var companies = await companyRepository.GetAllCompaniesAsync();
            return Ok(companies);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCompany(int id, CompanyUpdateDto companyDto)
        {
            if (id != companyDto.Id) return BadRequest();
            var result = await companyRepository.UpdateCompanyAsync(companyDto);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompany(int id)
        {
            var result = await companyRepository.DeleteCompanyAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
