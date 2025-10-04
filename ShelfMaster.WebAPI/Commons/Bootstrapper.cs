using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using ShelfMaster.Application.Commons;
using ShelfMaster.Application.Mapping;
using ShelfMaster.Infrastructure.Commons;
using ShelfMaster.Infrastructure.DbContext;
using ShelfMaster.Infrastructure.Entities;

namespace ShelfMaster.WebAPI.Commons;

public static class Bootstrapper
{
    public static IServiceCollection AddApiService(this IServiceCollection service, IConfiguration configuration)
    {
        service.ContextRegister(configuration);
        service.ApplicationRegister();
        service.AddAutoMapper(typeof(MappingProfile).Assembly);

        service.AddIdentity<ApplicationUser, IdentityRole<int>>(x =>
        {
            x.Password.RequireDigit = false;
            x.Password.RequiredLength = 8;
            x.Password.RequireUppercase = false;
        })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();


        return service;
    }
}
