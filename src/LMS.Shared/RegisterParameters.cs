using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LMS.Shared
{
    public class RegisterParameters
    {
        [Required]
        [EmailAddress]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Compare(nameof(Password), ErrorMessage = "Passwords do not match")]
        public string PasswordConfirm { get; set; }

        [Required]
        public string UniqueCitizenshipNumber { get; set; }

        [Required]
        public string FullName { get; set; }
        
        [Required]
        public string Role { get; set; }
    }
}
