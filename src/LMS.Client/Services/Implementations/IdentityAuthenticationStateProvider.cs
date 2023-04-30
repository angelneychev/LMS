namespace LMS.Client.States
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using LMS.Client.Services.Contracts;
    using LMS.Shared.ViewModels.UsersInRoles;
    using Microsoft.AspNetCore.Components;
    using Microsoft.AspNetCore.Components.Authorization;
    using Microsoft.JSInterop;

    public class IdentityAuthenticationStateProvider : AuthenticationStateProvider
    {
        private UserInfo? userInfoCache;
        private readonly IAuthorizeApi authorizeApi;

        public IdentityAuthenticationStateProvider(IAuthorizeApi authorizeApi)
        {
            this.authorizeApi = authorizeApi;
        }

        public async Task Login(LoginParameters loginParameters)
        {
            await this.authorizeApi.Login(loginParameters);

            this.NotifyAuthenticationStateChanged(this.GetAuthenticationStateAsync());
        }

        public async Task Register(RegisterParameters registerParameters)
        {
            await this.authorizeApi.Register(registerParameters);

            this.NotifyAuthenticationStateChanged(this.GetAuthenticationStateAsync());
        }

        public async Task Logout()
        {
            await this.authorizeApi.Logout();

            this.userInfoCache = null;

            this.NotifyAuthenticationStateChanged(this.GetAuthenticationStateAsync());
        }

        private async Task<UserInfo> GetUserInfo()
        {
            if (this.userInfoCache != null && this.userInfoCache.IsAuthenticated)
            {
                return this.userInfoCache;
            }

            this.userInfoCache = await this.authorizeApi.GetUserInfo();

            return this.userInfoCache;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var identity = new ClaimsIdentity();

            try
            {
                var userInfo = await this.GetUserInfo();
                if (userInfo.IsAuthenticated)
                {
                    var claims = new[] { new Claim(ClaimTypes.Name, userInfo.UserName) }
                    .Concat(userInfo.ExposedClaims.Select(c => new Claim(c.Key, c.Value)));

                    identity = new ClaimsIdentity(claims, "Server authentication");
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("Request failed:" + ex.ToString());
            }

            return new AuthenticationState(new ClaimsPrincipal(identity));
        }
    }
}
