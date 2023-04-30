namespace LMS.Shared.ViewModels.Companies
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class CreateCompanyParameters
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Address { get; set; }

        [Phone]
        [Required]
        public string PhoneNumber { get; set; }

        [EmailAddress]
        [Required]
        public string Email { get; set; }

        [Required]
        public string TaxIdentificationNumber { get; set; }
    }
}
