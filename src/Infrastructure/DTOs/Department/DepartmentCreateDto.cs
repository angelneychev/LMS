namespace Infrastructure.DTOs.Department
{
    using System.ComponentModel.DataAnnotations;

    public class DepartmentCreateDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public int CompanyId { get; set; }
    }
}
