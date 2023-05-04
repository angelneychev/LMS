namespace LMS.Client.Services.Implementations
{

    using System.Net.Http;
    using System.Net.Http.Json;
    using System.Threading.Tasks;

    using LMS.Client.Services.Contracts;
    using LMS.Shared.ViewModels.LeaveTypes;

    public class LeaveTypeApi : ILeaveTypeApi
    {
        private readonly HttpClient httpClient;

        public LeaveTypeApi(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task CreateLeaveType(CreateEditLeaveTypeParameters createLeaveTypeParameters)
        {
            await httpClient.PostAsJsonAsync("api/LeaveType/CreateLeaveType", createLeaveTypeParameters);
        }

        public async Task UpdateLeaveType(int id, CreateEditLeaveTypeParameters updateLeaveTypeParameters)
        {
            await httpClient.PutAsJsonAsync($"api/LeaveType/UpdateLeaveType/{id}", updateLeaveTypeParameters);
        }
    }
}

