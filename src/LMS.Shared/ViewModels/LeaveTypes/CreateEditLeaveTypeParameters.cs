namespace LMS.Shared.ViewModels.LeaveTypes
{
    using System.ComponentModel.DataAnnotations;

    public class CreateEditLeaveTypeParameters
    {
        [Required]
        public string Name { get; set; }
    }
}
