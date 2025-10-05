using Microsoft.AspNetCore.Identity;

namespace ShelfMaster.Infrastructure.IdentityUser;

public static class IdentityDataSeeder
{
    public static async Task SeedRoles(RoleManager<IdentityRole<int>> roleManager)
    {
        string[] roleNames = { "Admin", "User" };

        foreach (var roleName in roleNames)
        {
            var roleExist = await roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                await roleManager.CreateAsync(new IdentityRole<int>(roleName));
            }
        }
    }

}
