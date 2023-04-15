namespace Infrastructure.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Validation
    {
        public class ApplicationUserValidation
        {
            public const int MaxLengthPersonalIdentificationNumber = 10;
            public const int MinLengthPersonalIdentificationNumber = 10;
            public const int MaxLengthFullName = 50;
            public const int MinLengthFullName = 3;
        }
        public class CompanyValidation
        {
            public const int MaxLengthCompanyName = 100;
            public const int MaxLengthCompanyAddress = 200;
        }

        public class DepartmentValidation
        {
            public const int MaxLengthDepartmentName = 100;
        }
    }
}
