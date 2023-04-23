﻿using System.ComponentModel.DataAnnotations.Schema;

namespace LMS.Server.Models
{
    public class LeaveStatus
    {
        public LeaveStatus()
        {
            this.Leaves = new HashSet<Leave>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedOn { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? ModifiedOn { get; set; }

        public ICollection<Leave> Leaves { get; set; }
    }
}