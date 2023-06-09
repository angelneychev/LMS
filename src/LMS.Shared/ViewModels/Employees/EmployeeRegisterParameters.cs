﻿namespace LMS.Shared.ViewModels.Employees
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class EmployeeRegisterParameters
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
        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        public string Role { get; set; }

        [Required]
        public int CompanyId { get; set; }

        public int? DepartmentId { get; set; }

        public string Position { get; set; }

        [Required]
        public DateTime HiredDate { get; set; }

        public string HRManagement { get; set; }
    }
}
