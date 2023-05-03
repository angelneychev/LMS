namespace LMS.Client.Services.Contracts
{
    using LMS.Shared.ViewModels.LeaveStatuses;
    using System.Threading.Tasks;

    public interface ILeaveStatusApi
    {
        Task CreateLeaveStatus(CreateEditLeaveStatusParameters createLeaveStatusParameters);
        Task UpdateLeaveStatus(int id, CreateEditLeaveStatusParameters updateLeaveStatusParameters);
    }
}
