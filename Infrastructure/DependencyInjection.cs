using Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure.Fetcher;
using Infrastructure.Repository;
using GuerrillaNtp;
using Infrastructure.Mapping;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection service)
        {
            service.AddTransient<IBondFetcher, BondFetcher>();
            service.AddTransient<IBondRepository, BondRepository>();
            service.AddTransient<NtpClock>();
            service.AddTransient<NtpResponse>();
            service.AddAutoMapper(typeof(AutoMapperProfile));

            


            return service;
        }
    }
}
