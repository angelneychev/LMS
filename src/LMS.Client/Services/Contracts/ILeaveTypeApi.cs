namespace LMS.Client.Services.Contracts
{
    using System.Threading.Tasks;

    using LMS.Shared.ViewModels.LeaveTypes;
    public interface ILeaveTypeApi
    {
        Task CreateLeaveType(CreateEditLeaveTypeParameters createLeaveTypeParameters);
        Task UpdateLeaveType(int id, CreateEditLeaveTypeParameters updateLeaveTypeParameters);
    }
}
