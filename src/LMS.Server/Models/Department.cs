namespace LMS.Server.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using static LMS.Server.Models.Common.Validation.DepartmentValidation;

    public class Department
    {
        public int Id { get; set; }

        [Required]
        [MinLength(MinLengthDepartmentName)]
        [MaxLength(MaxLengthDepartmentName)]
        public string Name { get; set; }

        public int CompanyId { get; set; }

        public Company Company { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedOn { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? ModifiedOn { get; set; }
    }
}
