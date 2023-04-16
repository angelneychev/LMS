﻿namespace Infrastructure.DTOs
{
    using System.ComponentModel.DataAnnotations;

    public class ChangePasswordDto
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string OldPassword { get; set; }

        [Required]
        public string NewPassword { get; set; }
    }
}
