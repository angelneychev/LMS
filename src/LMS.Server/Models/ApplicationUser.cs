﻿namespace LMS.Server.Models
{
    using Microsoft.AspNetCore.Identity;
    using System.ComponentModel.DataAnnotations;

    using static LMS.Server.Models.Common.Validation.ApplicationUserValidation;

    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Roles = new HashSet<IdentityUserRole<string>>();
            this.Claims = new HashSet<IdentityUserClaim<string>>();
            this.Logins = new HashSet<IdentityUserLogin<string>>();
            this.IsFirstLogin = false;
        }

        [Required]
        [MaxLength(MaxLengthPersonalIdentificationNumber)]
        [MinLength(MinLengthPersonalIdentificationNumber)]
        public string UniqueCitizenshipNumber { get; set; }

        [Required]
        [MaxLength(MaxLengthFullName)]
        [MinLength(MinLengthFullName)]
        public string FullName { get; set; }

        public bool IsFirstLogin { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public virtual ICollection<IdentityUserRole<string>> Roles { get; set; }

        public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }

        public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; }
    }
}
