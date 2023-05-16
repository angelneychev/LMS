namespace LMS.Client.Services.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;

    using LMS.Shared.ViewModels.Employees;
    using LMS.Shared.ViewModels.UsersInRoles;

    public interface IEmployeeApi
    {
        Task<HttpResponseMessage> EmployeeRegister(EmployeeRegisterParameters parameters);

        Task<IEnumerable<EmployeeReportParameters>> GetEmployeesByCompany();

        Task UpdateEmployee(EmployeeUpdateParameters employeeUpdateParameters);

        Task DeleteEmployeeRole(int id);

        Task DeleteEmployeeDepartment(int id);
    }
}
