using System.Data;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyCompany.Microservice.Domain.Interfaces;
using MyCompany.Microservice.Infrastructure.Database;
using MyCompany.Microservice.Infrastructure.Implementation;
using MyCompany.Microservice.Infrastructure.Interfaces;
using MyCompany.Microservice.Infrastructure.Mappers;

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
            services.AddScoped<IDbConnection>(sp =>
            {
                var context = sp.GetRequiredService<FleetContext>();
                return context.Database.GetDbConnection();
            });
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IFleetRepository, FleetRepository>();

            services.AddSingleton<ICustomerEntityFactory, EntityFactory>();
            services.AddSingleton<IVehicleEntityFactory, EntityFactory>();
            services.AddSingleton<IRentedVehicleEntityFactory, EntityFactory>();
            services.AddSingleton<IFleetEntityFactory, EntityFactory>();

            SqLiteTypeHandler();
            return services;
        }

        /// <summary>
        /// Define type handler for Dapper type management.
        /// </summary>
        public static void SqLiteTypeHandler()
        {
            SqlMapper.AddTypeHandler<Guid>(new GuidAsStringHandler());
            SqlMapper.RemoveTypeMap(typeof(Guid));
            SqlMapper.RemoveTypeMap(typeof(Guid?));
        }
    }
}
