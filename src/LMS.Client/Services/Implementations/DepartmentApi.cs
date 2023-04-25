using LMS.Client.Services.Contracts;
using LMS.Shared.ViewModels.Departments;
using System.Net.Http.Json;

namespace LMS.Client.Services.Implementations
{
    public class DepartmentApi : IDepartmentApi
    {
        private readonly HttpClient httpClient;

        public DepartmentApi(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task CreateDepartment(CreateDepartmentParameters createDepartmentParameters)
        {
            var result = await httpClient.PostAsJsonAsync("api/Department/CreateDepartment", createDepartmentParameters);

            if (result.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                throw new Exception(await result.Content.ReadAsStringAsync());
            }
            result.EnsureSuccessStatusCode();
        }
    }
}
