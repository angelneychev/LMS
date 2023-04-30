namespace LMS.Client.Services.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Json;
    using System.Threading.Tasks;

    using LMS.Client.Services.Contracts;
    using LMS.Shared.ViewModels.Departments;

    public class DepartmentApi : IDepartmentApi
    {
        private readonly HttpClient httpClient;

        public DepartmentApi(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task CreateDepartment(CreateDepartmentParameters createDepartmentParameters)
        {
            var result = await this.httpClient.PostAsJsonAsync("api/Department/CreateDepartment", createDepartmentParameters);

            if (result.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                throw new Exception(await result.Content.ReadAsStringAsync());
            }

            result.EnsureSuccessStatusCode();
        }

        public async Task<IEnumerable<KeyValuePair<string, string>>> GetAllDepartmentsAsKeyValuePairs()
               => await this.httpClient
                  .GetFromJsonAsync<IEnumerable<KeyValuePair<string, string>>>("api/Department/GetAllDepartmentsAsKeyValuePairs")
                  ?? Enumerable.Empty<KeyValuePair<string, string>>();
    }
}
