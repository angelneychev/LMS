namespace Infrastructure.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static Infrastructure.Common.Validation.DepartmentValidation;
    public class Department
    {

        public int Id { get; set; }

        [Required]
        [MaxLength(MaxLengthDepartmentName)]
        public string Name { get; set; }

        public int CompanyId { get; set; }

        public Company Company { get; set; }
    }
}
