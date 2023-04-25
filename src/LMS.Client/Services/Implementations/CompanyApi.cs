namespace LMS.Client.Services.Implementations
{
    using LMS.Client.Services.Contracts;
    using LMS.Shared.ViewModels.Companies;
    using System.Net.Http;
    using System.Net.Http.Json;
    using System.Threading.Tasks;

    public class CompanyApi : ICompanyApi
    {
        private readonly HttpClient httpClient;

        public CompanyApi(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<HttpResponseMessage> CreateCompany(CreateCompanyParameters createCompanyParameters)
        {
            var result = await httpClient.PostAsJsonAsync("api/Company/CreateCompany", createCompanyParameters);

            if (result.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                throw new Exception(await result.Content.ReadAsStringAsync());
            }
            result.EnsureSuccessStatusCode();

            return result;
        }

        public async Task<IEnumerable<KeyValuePair<string, string>>> GetAllCompaniesAsKeyValuePairs()
            => await httpClient
            .GetFromJsonAsync<IEnumerable<KeyValuePair<string, string>>>("api/Company/GetAllCompaniesAsKeyValuePairs") 
            ?? Enumerable.Empty<KeyValuePair<string, string>>();
    }
}