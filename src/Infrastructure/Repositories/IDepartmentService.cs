namespace Infrastructure.Repositories
{
    using Infrastructure.DTOs.Department;
    using Infrastructure.Entities;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IDepartmentService
    {
        Task<IEnumerable<DepartmentReadDto>> GetDepartmentsAsync();

        Task<DepartmentReadDto> GetDepartmentByIdAsync(int id);

        Task<DepartmentReadDto> CreateDepartmentAsync(DepartmentCreateDto departmentCreateDto);

        Task UpdateDepartmentAsync(int id, DepartmentUpdateDto departmentUpdateDto);

        Task DeleteDepartmentAsync(int id);

        Task<IEnumerable<Department>> GetDepartmentsByCompanyAsync(int companyId);
    }
}
