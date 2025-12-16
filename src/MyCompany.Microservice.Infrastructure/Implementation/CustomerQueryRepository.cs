using System.Data;
using Dapper;
using MyCompany.Microservice.Domain.DbEntities;
using MyCompany.Microservice.Domain.DTO;
using MyCompany.Microservice.Infrastructure.Interfaces;

namespace MyCompany.Microservice.Infrastructure.Implementation
{
    /// <inheritdoc />
    public class CustomerQueryRepository : ICustomerQueryRepository
    {
        private readonly IDbConnection _connection;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerQueryRepository"/> class.
        /// </summary>
        /// <param name="connection">Database connection.</param>
        public CustomerQueryRepository(IDbConnection connection)
        {
            ArgumentNullException.ThrowIfNull(connection);

            _connection = connection;
        }

        /// <inheritdoc />
        public async Task<RentedVehicleDto?> GetRentedVehicleByIdAsync(Guid rentedVehicleId)
        {
            var paramValue = new DynamicParameters();
            paramValue.Add("id", rentedVehicleId);

            var queryToExecute =
                "SELECT RentedVehicleId, VehicleId, CustomerId, FleetId, RentStartedOn, RentFinishedOn " +
                "FROM RentedVehicle WHERE RentedVehicleId = @id ORDER BY RentedVehicleId ASC";
            var fromDb = await _connection.QueryFirstOrDefaultAsync<RentedVehicle>(queryToExecute, paramValue);

            return fromDb == null
                ? null
                : new RentedVehicleDto
                {
                    RentedVehicleId = fromDb.RentedVehicleId,
                    FleetId = fromDb.FleetId,
                    VehicleId = fromDb.VehicleId,
                    CustomerId = fromDb.CustomerId,
                    StartRent = fromDb.RentStartedOn,
                    EndRent = fromDb.RentFinishedOn
                };
        }

        /// <inheritdoc />
        public async Task<CustomerDto?> GetCustomerByIdAsync(Guid customerId)
        {
            var paramValue = new DynamicParameters();
            paramValue.Add("id", customerId);

            var queryToExecute =
                "SELECT CustomerId, CustomerName FROM Customer WHERE CustomerId = @id ORDER BY CustomerId ASC";

            var fromDb = await _connection.QueryFirstOrDefaultAsync<Customer>(queryToExecute, paramValue);

            return fromDb == null
                ? null
                : new CustomerDto
                {
                    CustomerId = fromDb.CustomerId,
                    CustomerName = fromDb.CustomerName
                };
        }

        /// <inheritdoc />
        public async Task<RentedVehicleDto?> GetRentedVehicleByIdAndCustomerIdAsync(Guid rentedVehicleId, Guid customerId)
        {
            var paramValue = new DynamicParameters();
            paramValue.Add("customerId", customerId);
            paramValue.Add("id", rentedVehicleId);

            var queryToExecute =
                "SELECT RentedVehicleId, VehicleId, CustomerId, FleetId, RentStartedOn, RentFinishedOn " +
                "FROM RentedVehicle WHERE RentedVehicleId = @id AND CustomerId = @customerId ORDER BY RentedVehicleId ASC";

            var fromDb = await _connection.QueryFirstOrDefaultAsync<RentedVehicle>(queryToExecute, paramValue);

            return fromDb == null
                ? null
                : new RentedVehicleDto
                {
                    RentedVehicleId = fromDb.RentedVehicleId,
                    FleetId = fromDb.FleetId,
                    VehicleId = fromDb.VehicleId,
                    CustomerId = fromDb.CustomerId,
                    StartRent = fromDb.RentStartedOn,
                    EndRent = fromDb.RentFinishedOn
                };
        }

        /// <inheritdoc />
        public async Task<IEnumerable<RentedVehicleDto>?> GetRentedVehiclesByCustomerIdAsync(Guid customerId)
        {
            var paramValue = new DynamicParameters();
            paramValue.Add("customerId", customerId);

            var queryToExecute =
                "SELECT RentedVehicleId, VehicleId, CustomerId, FleetId, RentStartedOn, RentFinishedOn " +
                "FROM RentedVehicle WHERE CustomerId = @customerId ORDER BY RentFinishedOn DESC";
            var fromDb = await _connection.QueryAsync<RentedVehicle>(queryToExecute, paramValue);

            return !fromDb.Any()
                ? null
                : fromDb.Select(entity => new RentedVehicleDto
                {
                    RentedVehicleId = entity.RentedVehicleId,
                    FleetId = entity.FleetId,
                    VehicleId = entity.VehicleId,
                    CustomerId = entity.CustomerId,
                    StartRent = entity.RentStartedOn,
                    EndRent = entity.RentFinishedOn
                });
        }
    }
}
