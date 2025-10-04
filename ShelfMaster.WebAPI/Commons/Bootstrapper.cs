using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using ShelfMaster.Application.Commons;
using ShelfMaster.Application.Mapping;
using ShelfMaster.Infrastructure.Commons;
using System.Security.Claims;
using System.Text;

namespace ShelfMaster.WebAPI.Commons;

public static class Bootstrapper
{
    public static IServiceCollection AddApiService(this IServiceCollection service, IConfiguration configuration)
    {
        service.ContextRegister(configuration);
        service.ApplicationRegister();
        service.AddAutoMapper(typeof(MappingProfile).Assembly);

        service.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(x =>
            {
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["JWT : Issuer"],
                    ValidAudience = configuration["JWT : Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration["JWT : Key"])),
                    NameClaimType = ClaimTypes.Name,
                    RoleClaimType = ClaimTypes.Role
                };
            });

        return service;
    }
}
