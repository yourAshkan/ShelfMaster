using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShelfMaster.Domain.IRepository;
using ShelfMaster.Infrastructure.DbContext;
using ShelfMaster.Infrastructure.Repositories;

namespace ShelfMaster.Infrastructure.Commons;

public static class Bootstrapper
{
    public static IServiceCollection ContextRegister(this IServiceCollection service,IConfiguration configuration)
    {
        service.AddDbContext<AppDbContext>(x => x.UseSqlServer(configuration.GetConnectionString("DbConnection"),
            y => y.MigrationsAssembly("ShelfMaster.Infrastructure")));


        service.AddScoped<IBookRepository,BookRepository>();
        service.AddScoped<ILoanRepository,LoanRepository>();

        return service;
    }
}
