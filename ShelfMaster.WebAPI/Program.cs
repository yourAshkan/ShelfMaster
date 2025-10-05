using Microsoft.AspNetCore.Identity;
using ShelfMaster.Infrastructure.Entities;
using ShelfMaster.Infrastructure.IdentityUser;
using ShelfMaster.WebAPI.Commons;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiService(builder.Configuration);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    await IdentityDataSeeder.SeedRoles(roleManager);

    var adminEmail = "AdminEmail";
    var adminUser = await userManager.FindByEmailAsync(adminEmail);

    if (adminUser == null)
    {
        var newAdmin = new ApplicationUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(newAdmin, "AdminPassword");

        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(newAdmin, "Admin");

        }
    }
}

app.UseApiMiddleware();

app.Run();
