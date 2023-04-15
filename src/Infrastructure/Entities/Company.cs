namespace Infrastructure.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static Infrastructure.Common.Validation.CompanyValidation;

    public class Company
    {
        public Company()
        {
            this.Departments = new HashSet<Department>();
        }

        public int Id { get; set; }

        [Required]
        [MaxLength(MaxLengthCompanyName)]
        public string Name { get; set; }

        [Required]
        [MaxLength(MaxLengthCompanyAddress)]
        public string Address { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string TaxIdentificationNumber { get; set; }

        public ICollection<Department> Departments { get; set; }

    }
}
