using Microsoft.AspNetCore.Identity;
using ShelfMaster.Application.Interfasces;
using ShelfMaster.Infrastructure.Entities;

namespace ShelfMaster.Infrastructure.Repositories;

public class UserService(UserManager<ApplicationUser> _userManager) : IUserService
{
    public async Task<string?> GetGetUserNameByIdAsync(int id)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());
        return user != null ? user.UserName : "Unknown";
    }
}
