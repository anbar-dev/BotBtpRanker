using Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Application.Services;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection service)
    {
        service.AddTransient<IBondService, BondService>();
        return service;
    }
}
