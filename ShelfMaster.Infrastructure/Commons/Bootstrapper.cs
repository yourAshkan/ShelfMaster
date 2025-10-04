using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShelfMaster.Domain.IRepository;
using ShelfMaster.Infrastructure.DbContext;
using ShelfMaster.Infrastructure.Entities;
using ShelfMaster.Infrastructure.Repositories;

namespace ShelfMaster.Infrastructure.Commons;

public static class Bootstrapper
{
    public static IServiceCollection ContextRegister(this IServiceCollection service,IConfiguration configuration)
    {
        service.AddDbContext<AppDbContext>(x => x.UseSqlServer(configuration.GetConnectionString("DbConnection"),
            y => y.MigrationsAssembly("ShelfMaster.Infrastructure")));

        service.AddIdentity<ApplicationUser, IdentityRole<int>>(x =>
        {
            x.Password.RequireDigit = false;
            x.Password.RequiredLength = 8;
            x.Password.RequireUppercase = false;
        })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();


        service.AddScoped<IBookRepository,BookRepository>();
        service.AddScoped<ILoanRepository,LoanRepository>();

        return service;
    }
}
