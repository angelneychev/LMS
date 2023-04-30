namespace LMS.Client.Services.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Json;
    using System.Threading.Tasks;

    using LMS.Client.Services.Contracts;
    using LMS.Shared.ViewModels.Companies;

    public class CompanyApi : ICompanyApi
    {
        private readonly HttpClient httpClient;

        public CompanyApi(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<HttpResponseMessage> CreateCompany(CreateCompanyParameters createCompanyParameters)
        {
            var result = await this.httpClient.PostAsJsonAsync("api/Company/CreateCompany", createCompanyParameters);

            if (result.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                throw new Exception(await result.Content.ReadAsStringAsync());
            }

            result.EnsureSuccessStatusCode();

            return result;
        }

        public async Task<IEnumerable<KeyValuePair<string, string>>> GetAllCompaniesAsKeyValuePairs()
            => await this.httpClient
            .GetFromJsonAsync<IEnumerable<KeyValuePair<string, string>>>("api/Company/GetAllCompaniesAsKeyValuePairs")
            ?? Enumerable.Empty<KeyValuePair<string, string>>();
    }
}
