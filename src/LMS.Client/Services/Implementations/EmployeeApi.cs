namespace LMS.Client.Services.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Json;
    using System.Threading.Tasks;

    using LMS.Client.Services.Contracts;
    using LMS.Shared.ViewModels.Employees;

    public class EmployeeApi : IEmployeeApi
    {
        private readonly HttpClient httpClient;

        public EmployeeApi(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<IEnumerable<EmployeeReportParameters>> GetEmployeesByCompany()
        {
            return await this.httpClient.GetFromJsonAsync<IEnumerable<EmployeeReportParameters>>("api/Employee/GetEmployeesByCompany");
        }

    }
}
