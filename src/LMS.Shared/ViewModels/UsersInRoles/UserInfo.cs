namespace LMS.Shared.ViewModels.UsersInRoles
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class UserInfo
    {
        public bool IsAuthenticated { get; set; }

        public string UserName { get; set; }

        public bool IsFirstLogin { get; set; }

        public Dictionary<string, string> ExposedClaims { get; set; }
    }
}
