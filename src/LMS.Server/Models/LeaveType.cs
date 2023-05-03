namespace LMS.Server.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using static LMS.Server.Models.Common.Validation.LeaveTypeValidation;

    public class LeaveType
    {
        public LeaveType()
        {
            this.Leaves = new HashSet<Leave>();
        }

        public int Id { get; set; }

        [Required]
        [MinLength(MinLengthLeaveTypeName)]
        [MaxLength(MaxLengthLeaveTypeName)]
        public string Name { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedOn { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? ModifiedOn { get; set; }

        public ICollection<Leave> Leaves { get; set; }
    }
}
