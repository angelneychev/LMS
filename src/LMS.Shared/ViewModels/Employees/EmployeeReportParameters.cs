namespace LMS.Shared.ViewModels.Employees
{
    public class EmployeeReportParameters
    {
        public int Id { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string CompanyName { get; set; }

        public string Role { get; set; }

        public string Department { get; set; }

        public int? DepartmentId { get; set; }

        public bool IsRoleEditing { get; set; }

        public bool IsDepartmentEditing { get; set; }
    }
}
