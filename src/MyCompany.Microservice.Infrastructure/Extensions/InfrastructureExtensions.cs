using Microsoft.Extensions.DependencyInjection;
using MyCompany.Microservice.Domain.Interfaces;
using MyCompany.Microservice.Infrastructure.Database;
using MyCompany.Microservice.Infrastructure.Implementation;
using MyCompany.Microservice.Infrastructure.Interfaces;

namespace MyCompany.Microservice.Infrastructure.Extensions
{
    /// <summary>
    /// Extensions register.
    /// </summary>
    public static class InfrastructureExtensions
    {
        /// <summary>
        /// Add to the DI Service Collection the services needed to work with injected services.
        /// </summary>
        /// <param name="services">Current contract where the services will be added.</param>
        /// <returns>Updated current contract.</returns>
        public static IServiceCollection RegisterInfrastructureDependencies(this IServiceCollection services)
        {
            services.AddDbContext<FleetContext>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IFleetRepository, FleetRepository>();

            services.AddSingleton<ICustomerEntityFactory, EntityFactory>();
            services.AddSingleton<IVehicleEntityFactory, EntityFactory>();
            services.AddSingleton<IRentedVehicleEntityFactory, EntityFactory>();
            services.AddSingleton<IFleetEntityFactory, EntityFactory>();

            return services;
        }
    }
}
