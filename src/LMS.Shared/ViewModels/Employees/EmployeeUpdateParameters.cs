namespace LMS.Shared.ViewModels.Employees
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class EmployeeUpdateParameters
    {
        public int Id { get; set; }

        public string Role { get; set; }

        public string? RoleId { get; set; }

        public bool RemoveRole { get; set; }

        public int? DepartmentId { get; set; }

        public bool RemoveDepartment { get; set; }
    }
}
