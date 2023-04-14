namespace LMS.Services.Data.Contracts
{
    using System.Threading.Tasks;

    using LMS.Web.ViewModels.Contracts;

    public interface IUserService
    {
        Task<bool> RegisterUserAsync(RegisterViewModel model);

        Task<bool> UpdateUserAsync(UserViewModel model);

        Task<bool> DeleteUserAsync(string id);
    }
}
