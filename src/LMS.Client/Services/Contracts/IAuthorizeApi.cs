using LMS.Shared.ViewModels.UsersInRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.Client.Services.Contracts
{
    public interface IAuthorizeApi
    {
        Task Login(LoginParameters loginParameters);

        Task<HttpResponseMessage> Register(RegisterParameters parameters);

        Task Logout();

        Task<UserInfo> GetUserInfo();

        Task CreateRole(CreateRoleParameters createRoleParameters);

        Task<IEnumerable<KeyValuePair<string, string>>> GetAllRolesAsKeyValuePairs();

    }
}
