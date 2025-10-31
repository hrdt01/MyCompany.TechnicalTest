using System.Data.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyCompany.Microservice.Infrastructure.Database;

namespace MyCompany.Microservice.Api.UnitTest.Helpers
{
    /// <inheritdoc />
    public class IntegrationTestWebApplicationFactory : WebApplicationFactory<Startup>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IntegrationTestWebApplicationFactory"/> class.
        /// </summary>
        public IntegrationTestWebApplicationFactory()
        {
            HttpClient = CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        /// <summary>
        /// Gets a <see cref="HttpClient"/> instance to be used in tests.
        /// </summary>
        public HttpClient HttpClient { get; private set; }

        /// <inheritdoc />
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);
            builder.UseEnvironment("Test");
            builder.ConfigureAppConfiguration(config =>
            {
                var integrationConfig = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.test.json")
                    .Build();

                config.AddConfiguration(integrationConfig);
            });

            builder.ConfigureTestServices(services =>
            {
                var dbContext = services.SingleOrDefault(x =>
                    x.ServiceType == typeof(DbContextOptions<FleetContext>));
                services.Remove(dbContext!);

                var dbConnection = services.SingleOrDefault(x =>
                    x.ServiceType == typeof(DbConnection));
                services.Remove(dbConnection!);

                services.AddSingleton<DbConnection>(container =>
                {
                    var connection = new SqliteConnection("DataSource=:memory:");
                    connection.Open();
                    return connection;
                });

                services.AddDbContext<FleetContext>((container, options) =>
                {
                    var connection = container.GetRequiredService<DbConnection>();
                    options.UseSqlite(connection);
                });
            });
        }
    }
}
