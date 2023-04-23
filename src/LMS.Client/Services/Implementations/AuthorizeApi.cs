using LMS.Client.Services.Contracts;
using LMS.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LMS.Client.Services.Implementations
{
    public class AuthorizeApi : IAuthorizeApi
    {
        private readonly HttpClient httpClient;

        public AuthorizeApi(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task Login(LoginParameters loginParameters)
        {
            //var stringContent = new StringContent(JsonSerializer.Serialize(loginParameters), Encoding.UTF8, "application/json");
            var result = await this.httpClient.PostAsJsonAsync("api/Authorize/Login", loginParameters);

            if (result.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                throw new Exception(await result.Content.ReadAsStringAsync());
            }

            result.EnsureSuccessStatusCode();
        }

        public async Task Logout()
        {
            var result = await this.httpClient.PostAsync("api/Authorize/Logout", null);
            result.EnsureSuccessStatusCode();
        }

        public async Task<HttpResponseMessage> Register(RegisterParameters parameters)
        {
            var response = await httpClient.PostAsJsonAsync("api/Authorize/Register", parameters);
            return response;
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

        public async Task<List<RoleModel>> GetRolesAsync()
        {
            return await httpClient.GetFromJsonAsync<List<RoleModel>>("api/Authorize/Roles");
        }

        public async Task<IEnumerable<KeyValuePair<string, string>>> GetAllAsKeyValuePairs()
        {
            return await httpClient.GetFromJsonAsync<IEnumerable<KeyValuePair<string, string>>>("api/Authorize/GetAllAsKeyValuePairs");
        }

    }
}
