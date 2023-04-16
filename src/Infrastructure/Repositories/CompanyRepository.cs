namespace Infrastructure.Repositories
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Infrastructure.Entities;
    using Microsoft.EntityFrameworkCore;
    using Infrastructure.Data;
    using Infrastructure.DTOs.Company;
    using AutoMapper;

    public class CompanyRepository : ICompanyRepository
    {
        private readonly AppDbContext dbContext;
        private readonly IMapper mapper;

        public CompanyRepository(AppDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<Company> CreateCompanyAsync(CompanyCreateDto companyDto)
        {
            var company = this.mapper.Map<Company>(companyDto);
            await this.dbContext.Companies.AddAsync(company);
            await this.dbContext.SaveChangesAsync();

            return company;
        }

        public async Task<CompanyReadDto> GetCompanyByIdAsync(int id)
        {
            var company = await this.dbContext.Companies.FindAsync(id);
            return this.mapper.Map<CompanyReadDto>(company);
        }

        public async Task<IEnumerable<CompanyReadDto>> GetAllCompaniesAsync()
        {
            var companies = await this.dbContext.Companies.ToListAsync();
            return this.mapper.Map<IEnumerable<CompanyReadDto>>(companies);
        }

        public async Task<bool> UpdateCompanyAsync(CompanyUpdateDto companyDto)
        {
            var company = await this.dbContext.Companies.FindAsync(companyDto.Id);
            if (company == null)
            {
                return false;
            }

            this.mapper.Map(companyDto, company);
            this.dbContext.Entry(company).State = EntityState.Modified;
            await this.dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteCompanyAsync(int id)
        {
            var company = await this.dbContext.Companies.FindAsync(id);
            if (company == null)
            {
                return false;
            }

            this.dbContext.Companies.Remove(company);
            await this.dbContext.SaveChangesAsync();

            return true;
        }
    }
}
