using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ShelfMaster.Application.Commons;

public static class Bootstrapper
{
    public static IServiceCollection ApplicationRegister(this IServiceCollection service)
    {
        service.AddMediatR(x => x.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

        return service;
    }
}
