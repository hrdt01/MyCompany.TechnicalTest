using Microsoft.Extensions.DependencyInjection;
using MyCompany.Microservice.Infrastructure.Extensions;
using MyCompany.Microservice.Services.Implementation;
using MyCompany.Microservice.Services.Interfaces;

namespace MyCompany.Microservice.Services.Extensions
{
    /// <summary>
    /// Extensions register.
    /// </summary>
    public static class ServicesExtensions
    {
        /// <summary>
        /// Add to the DI Service Collection the services needed to work with injected services.
        /// </summary>
        /// <param name="services">Current contract where the services will be added.</param>
        /// <returns>Updated current contract.</returns>
        public static IServiceCollection RegisterServicesDependencies(this IServiceCollection services)
        {
            services.RegisterInfrastructureDependencies();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IFleetService, FleetService>();

            return services;
        }
    }
}
