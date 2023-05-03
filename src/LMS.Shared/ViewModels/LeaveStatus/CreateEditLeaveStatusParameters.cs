namespace LMS.Shared.ViewModels.LeaveStatuses
{
    using System.ComponentModel.DataAnnotations;

    public class CreateEditLeaveStatusParameters
    {
        [Required]
        public string Name { get; set; }
    }
}

