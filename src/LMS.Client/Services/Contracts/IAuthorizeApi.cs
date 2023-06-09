﻿namespace LMS.Client.Services.Contracts
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;

    using LMS.Shared.ViewModels.Employees;
    using LMS.Shared.ViewModels.UsersInRoles;

    public interface IAuthorizeApi
    {
        Task Login(LoginParameters loginParameters);

        Task<HttpResponseMessage> Register(RegisterParameters parameters);

        Task ChangePassword(ChangePasswordParameters parameters);

        Task Logout();

        Task<bool> IsFirstLogin();

        Task<UserInfo> GetUserInfo();

        Task CreateRole(CreateRoleParameters createRoleParameters);

        Task<IEnumerable<KeyValuePair<string, string>>> GetAllRolesAsKeyValuePairs();
    }
}
