namespace LMS.Client.Services.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Json;
    using System.Threading.Tasks;

    using LMS.Client.Services.Contracts;
    using LMS.Shared.ViewModels.Employees;
    using LMS.Shared.ViewModels.UsersInRoles;
    using Microsoft.AspNetCore.Components;
    using Microsoft.JSInterop;

    public class AuthorizeApi : IAuthorizeApi
    {
        private readonly HttpClient httpClient;

        public AuthorizeApi(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task Login(LoginParameters loginParameters)
        {
            var result = await this.httpClient.PostAsJsonAsync("api/Authorize/Login", loginParameters);

            if (result.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                throw new Exception(await result.Content.ReadAsStringAsync());
            }

            result.EnsureSuccessStatusCode();
        }

        public async Task<bool> IsFirstLogin()
        {
            var response = await this.httpClient.GetAsync("api/Authorize/IsFirstLogin");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<bool>();
            }

            throw new Exception("Failed to check IsFirstLogin status.");
        }


        public async Task Logout()
        {
            var result = await this.httpClient.PostAsync("api/Authorize/Logout", null);
            result.EnsureSuccessStatusCode();
        }

        public async Task<HttpResponseMessage> Register(RegisterParameters parameters)
            => await this.httpClient.PostAsJsonAsync("api/Authorize/Register", parameters);

        public async Task ChangePassword(ChangePasswordParameters parameters)
        {
            var result = await this.httpClient.PostAsJsonAsync("api/Authorize/ChangePassword", parameters);

            if (result.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                throw new Exception(await result.Content.ReadAsStringAsync());
            }

            result.EnsureSuccessStatusCode();
        }

        public async Task<UserInfo> GetUserInfo()
            => await this.httpClient.GetFromJsonAsync<UserInfo>("api/Authorize/UserInfo");

        public async Task CreateRole(CreateRoleParameters createRoleParameters)
        {
            var result = await this.httpClient.PostAsJsonAsync("api/Authorize/CreateRole", createRoleParameters);

            if (result.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                throw new Exception(await result.Content.ReadAsStringAsync());
            }

            result.EnsureSuccessStatusCode();
        }

        public async Task<HttpResponseMessage> RegisterUserAndEmployee(RegisterUserAndEmployeeParameters parameters)
            => await this.httpClient.PostAsJsonAsync("api/Authorize/RegisterUserAndEmployee", parameters);

        public async Task<IEnumerable<KeyValuePair<string, string>>> GetAllRolesAsKeyValuePairs()
            => await this.httpClient
            .GetFromJsonAsync<IEnumerable<KeyValuePair<string, string>>>("api/Authorize/GetAllRolesAsKeyValuePairs")
            ?? Enumerable.Empty<KeyValuePair<string, string>>();
 
    }
}
