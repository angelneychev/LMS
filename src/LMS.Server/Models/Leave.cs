namespace LMS.Server.Models
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Leave
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }

        public Employee Employee { get; set; }

        public int LeaveStatusId { get; set; }

        public LeaveStatus LeaveStatus { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedOn { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? ModifiedOn { get; set; }
    }
}
