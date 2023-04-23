namespace LMS.Server.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using static LMS.Server.Models.Common.Validation.CompanyValidation;

    public class Company
    {
        public Company()
        {
            this.Departments = new HashSet<Department>();
        }

        public int Id { get; set; }

        [Required]
        [MinLength(MinLengthCompanyName)]
        [MaxLength(MaxLengthCompanyName)]
        public string Name { get; set; }

        [Required]
        [MaxLength(MaxLengthCompanyAddress)]
        public string Address { get; set; }

        [Phone]
        [Required]
        public string PhoneNumber { get; set; }

        [EmailAddress]
        [Required]
        public string Email { get; set; }

        [Required]
        [MinLength(MinLengthTaxIdentificationNumber)]
        [MaxLength(MaxLengthTaxIdentificationNumber)]
        public string TaxIdentificationNumber { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedOn { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? ModifiedOn { get; set; }

        public ICollection<Department> Departments { get; set; }
    }
}