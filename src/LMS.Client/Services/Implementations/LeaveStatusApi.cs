namespace LMS.Client.Services.Implementations
{
    using System.Net.Http;
    using System.Net.Http.Json;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Components;

    using LMS.Client.Services.Contracts;
    using LMS.Shared.ViewModels.LeaveStatuses;

    public class LeaveStatusApi : ILeaveStatusApi
    {
        private readonly HttpClient httpClient;

        public LeaveStatusApi(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task CreateLeaveStatus(CreateEditLeaveStatusParameters createLeaveStatusParameters)
        {
            await httpClient.PostAsJsonAsync("api/LeaveStatus/CreateLeaveStatus", createLeaveStatusParameters);
        }

        public async Task UpdateLeaveStatus(int id, CreateEditLeaveStatusParameters updateLeaveStatusParameters)
        {
            await httpClient.PutAsJsonAsync($"api/LeaveStatus/UpdateLeaveStatus/{id}", updateLeaveStatusParameters);
        }
    }
}
