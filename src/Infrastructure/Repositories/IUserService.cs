using Infrastructure.DTOs.User;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public interface IUserService
    {
        Task<IdentityResult> RegisterUserAsync(ApplicationUser currentUser, RegisterUserDto registerUserDto);
    }
}
