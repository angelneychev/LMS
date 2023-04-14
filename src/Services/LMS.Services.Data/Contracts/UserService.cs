namespace LMS.Services.Data.Contracts
{
    using System.Net.Http;
    using System.Net.Http.Json;
    using System.Threading.Tasks;

    using LMS.Web.ViewModels.Contracts;

    public class UserService : IUserService
    {
        private readonly HttpClient httpClient;

        public UserService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<bool> RegisterUserAsync(RegisterViewModel model)
        {
            var response = await this.httpClient.PostAsJsonAsync("api/users/register", model);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateUserAsync(UserViewModel model)
        {
            var response = await this.httpClient.PutAsJsonAsync($"api/users/{model.Id}", model);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteUserAsync(string id)
        {
            var response = await this.httpClient.DeleteAsync($"api/users/{id}");

            return response.IsSuccessStatusCode;
        }
    }
}
