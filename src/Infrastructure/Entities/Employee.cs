namespace Infrastructure.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Employee
    {
        public Employee()
        {
            this.Leaves = new HashSet<Leave>();
        }

        public int Id { get; set; }

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public int CompanyId { get; set; }

        public Company Company { get; set; }

        public int DepartmentId { get; set; }

        public Department Department { get; set; }

        public string Position { get; set; }

        public DateTime HiredDate { get; set; }

        public DateTime? TerminatedDate { get; set; }

        public ICollection<Leave> Leaves { get; set; }
    }
}
