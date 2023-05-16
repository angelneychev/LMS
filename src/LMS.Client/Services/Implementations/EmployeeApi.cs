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
            => await this.httpClient.GetFromJsonAsync<IEnumerable<EmployeeReportParameters>>("api/Employee/GetEmployeesByCompany");

        public async Task<HttpResponseMessage> EmployeeRegister(EmployeeRegisterParameters parameters)
           => await this.httpClient.PostAsJsonAsync("api/Employee/EmployeeRegister", parameters);

        public async Task UpdateEmployee(EmployeeUpdateParameters employeeUpdateParameters)
            => await this.httpClient.PutAsJsonAsync("api/Employee/UpdateEmployee", employeeUpdateParameters);

        public async Task DeleteEmployeeRole(int id)
            => await httpClient.DeleteAsync($"api/Employee/DeleteEmployeeRole/{id}");

        public async Task DeleteEmployeeDepartment(int id)
            => await httpClient.DeleteAsync($"api/Employee/DeleteEmployeeDepartment/{id}");
    }
}
