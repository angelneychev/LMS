namespace Infrastructure.Repositories
{
    using AutoMapper;

    using Infrastructure.Data;
    using Infrastructure.DTOs.Department;
    using Infrastructure.Entities;
    using Infrastructure.Common;

    using Microsoft.EntityFrameworkCore;

    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class DepartmentService : IDepartmentService
    {
        private readonly AppDbContext dbContext;
        private readonly IMapper mapper;

        public DepartmentService(AppDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<DepartmentReadDto>> GetDepartmentsAsync()
        {
            var departments = await this.dbContext.Departments.ToListAsync();
            return this.mapper.Map<IEnumerable<DepartmentReadDto>>(departments);
        }

        public async Task<DepartmentReadDto> GetDepartmentByIdAsync(int id)
        {
            var department = await this.dbContext.Departments.FindAsync(id);
            return this.mapper.Map<DepartmentReadDto>(department);
        }

        public async Task<DepartmentReadDto> CreateDepartmentAsync(DepartmentCreateDto departmentCreateDto)
        {
            var department = this.mapper.Map<Department>(departmentCreateDto);
            this.dbContext.Departments.Add(department);
            await this.dbContext.SaveChangesAsync();
            return this.mapper.Map<DepartmentReadDto>(department);
        }

        public async Task UpdateDepartmentAsync(int id, DepartmentUpdateDto departmentUpdateDto)
        {
            var department = await this.dbContext.Departments.FindAsync(id);

            if (department == null)
            {
                throw new NotFoundException($"Department with ID {id} not found.");
            }

            this.mapper.Map(departmentUpdateDto, department);
            await this.dbContext.SaveChangesAsync();
        }

        public async Task DeleteDepartmentAsync(int id)
        {
            var department = await this.dbContext.Departments.FindAsync(id);

            if (department == null)
            {
                throw new NotFoundException($"Department with ID {id} not found.");
            }

            this.dbContext.Departments.Remove(department);
            await this.dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Department>> GetDepartmentsByCompanyAsync(int companyId)
        {
            var departments = await this.dbContext.Departments
                .Where(d => d.CompanyId == companyId)
                .ToListAsync();

            return departments;
        }
    }
}
