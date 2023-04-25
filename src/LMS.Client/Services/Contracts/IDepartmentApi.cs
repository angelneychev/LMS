using LMS.Shared.ViewModels.Departments;

namespace LMS.Client.Services.Contracts
{
    public interface IDepartmentApi
    {
        Task CreateDepartment(CreateDepartmentParameters createDepartmentParameters);
    }
}
