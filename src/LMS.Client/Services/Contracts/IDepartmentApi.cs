namespace LMS.Client.Services.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using LMS.Shared.ViewModels.Departments;

    public interface IDepartmentApi
    {
        Task CreateDepartment(CreateDepartmentParameters createDepartmentParameters);

        Task<IEnumerable<KeyValuePair<string, string>>> GetAllDepartmentsAsKeyValuePairs();
    }
}
