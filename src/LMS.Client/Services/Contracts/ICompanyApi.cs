namespace LMS.Client.Services.Contracts
{
    using LMS.Shared.ViewModels.Companies;
    using System.Threading.Tasks;

    public interface ICompanyApi
    {
        Task<HttpResponseMessage> CreateCompany(CreateCompanyParameters createCompanyParameters);

        Task<IEnumerable<KeyValuePair<string, string>>> GetAllCompaniesAsKeyValuePairs();
    }
}
