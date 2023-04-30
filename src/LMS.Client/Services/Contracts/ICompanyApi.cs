namespace LMS.Client.Services.Contracts
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;

    using LMS.Shared.ViewModels.Companies;

    public interface ICompanyApi
    {
        Task<HttpResponseMessage> CreateCompany(CreateCompanyParameters createCompanyParameters);

        Task<IEnumerable<KeyValuePair<string, string>>> GetAllCompaniesAsKeyValuePairs();
    }
}
