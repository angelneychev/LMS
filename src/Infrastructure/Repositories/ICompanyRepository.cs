namespace Infrastructure.Repositories
{
    using Infrastructure.Entities;
    using Infrastructure.DTOs.Company;

    public interface ICompanyRepository
    {
        Task<Company> CreateCompanyAsync(CompanyCreateDto companyDto);

        Task<CompanyReadDto> GetCompanyByIdAsync(int id);

        Task<IEnumerable<CompanyReadDto>> GetAllCompaniesAsync();

        Task<bool> UpdateCompanyAsync(CompanyUpdateDto companyDto);

        Task<bool> DeleteCompanyAsync(int id);
    }
}
