﻿using LMS.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.Client.Services.Contracts
{
    public interface IAuthorizeApi
    {
        Task Login(LoginParameters loginParameters);

        //Task Register(RegisterParameters registerParameters);
        Task<HttpResponseMessage> Register(RegisterParameters parameters);

        Task Logout();

        Task<UserInfo> GetUserInfo();

        Task CreateRole(CreateRoleParameters createRoleParameters);

        Task<List<RoleModel>> GetRolesAsync();

        Task<IEnumerable<KeyValuePair<string, string>>> GetAllAsKeyValuePairs();

    }
}
