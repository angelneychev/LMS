namespace Infrastructure.Entities
{
    using Microsoft.AspNetCore.Identity;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static Infrastructure.Common.Validation.ApplicationUserValidation;

    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser() : base()
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

        // Audit info
        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        // Deletable entity
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public ICollection<IdentityUserRole<string>> Roles { get; set; }

        public ICollection<IdentityUserClaim<string>> Claims { get; set; }

        public ICollection<IdentityUserLogin<string>> Logins { get; set; }
    }
}
