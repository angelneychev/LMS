using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DTOs.Department
{
    public class DepartmentUpdateDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public int CompanyId { get; set; }
    }
}
