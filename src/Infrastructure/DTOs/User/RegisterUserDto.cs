using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DTOs.User
{
    public class RegisterUserDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        public string Password { get; set; }

        [Required]
        [MaxLength(200)]
        public string FullName { get; set; }

        [Required]
        [MaxLength(11)]
        [MinLength(11)]
        public string UniqueCitizenshipNumber { get; set; }

        public ICollection<string> Roles { get; set; }

        public int? CompanyId { get; set; }

        public int? DepartmentId { get; set; }

        public bool CreateNewDepartment { get; set; }

        [MaxLength(100)]
        public string NewDepartmentName { get; set; }
    }

}
