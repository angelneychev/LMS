using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Shared.ViewModels.Common
{
    public class ModelValidations
    {
        public static class Password
        {
            public const int PasswordMinLength = 6;

            public const int PasswordMaxLength = 16;
        }
    }
}
