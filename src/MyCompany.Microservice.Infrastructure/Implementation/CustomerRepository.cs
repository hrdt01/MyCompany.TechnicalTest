using System.Data.Common;
using Dapper;
using Microsoft.EntityFrameworkCore;
using MyCompany.Microservice.Domain.DbEntities;
using MyCompany.Microservice.Domain.DTO;
using MyCompany.Microservice.Domain.Interfaces;
using MyCompany.Microservice.Infrastructure.Database;
using MyCompany.Microservice.Infrastructure.Interfaces;
using MyCompany.Microservice.Infrastructure.Mappers;

namespace MyCompany.Microservice.Infrastructure.Implementation
{
    /// <inheritdoc />
    public class CustomerRepository : ICustomerRepository
    {
        private readonly FleetContext _fleetContext;
        private readonly IRentedVehicleEntityFactory _rentedVehicleEntityFactory;
        private readonly ICustomerEntityFactory _customerEntityFactory;
        private readonly DbConnection _connection;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerRepository"/> class.
        /// </summary>
        /// <param name="fleetContext">DB context.</param>
        /// <param name="rentedVehicleEntityFactory">Instance of <see cref="IRentedVehicleEntityFactory"/>.</param>
        /// <param name="customerEntityFactory">Instance of <see cref="ICustomerEntityFactory"/>.</param>
        public CustomerRepository(
            FleetContext fleetContext,
            IRentedVehicleEntityFactory rentedVehicleEntityFactory,
            ICustomerEntityFactory customerEntityFactory)
        {
            ArgumentNullException.ThrowIfNull(fleetContext);
            ArgumentNullException.ThrowIfNull(rentedVehicleEntityFactory);
            ArgumentNullException.ThrowIfNull(customerEntityFactory);

            _fleetContext = fleetContext;
            _fleetContext.Database.EnsureCreated();
            _rentedVehicleEntityFactory = rentedVehicleEntityFactory;
            _customerEntityFactory = customerEntityFactory;
            _connection = _fleetContext.Database.GetDbConnection();
        }

        /// <inheritdoc />
        public async Task<CustomerDto?> AddNewCustomerAsync(CustomerDto newCustomer)
        {
            ArgumentNullException.ThrowIfNull(newCustomer);

            var customerDbInstance = newCustomer.ToDbEntity(_customerEntityFactory);
            await _fleetContext.Customers.AddAsync(customerDbInstance);
            await _fleetContext.SaveChangesAsync();

            return await GetCustomerByIdAsync(customerDbInstance.CustomerId);
        }

        /// <inheritdoc />
        public async Task<RentedVehicleDto?> RentVehicleAsync(RentedVehicleDto sourceRentedVehicle)
        {
            ArgumentNullException.ThrowIfNull(sourceRentedVehicle);

            var newRentedVehicleDbInstance = sourceRentedVehicle.ToDbEntity(_rentedVehicleEntityFactory);
            await _fleetContext.RentedVehicles.AddAsync(newRentedVehicleDbInstance);

            await _fleetContext.SaveChangesAsync();

            return await GetRentedVehicleByIdAsync(newRentedVehicleDbInstance.RentedVehicleId);
        }

        /// <inheritdoc />
        public async Task<RentedVehicleDto?> ReturnRentedVehicle(Guid rentedVehicleId)
        {
            ArgumentNullException.ThrowIfNull(rentedVehicleId);

            var rentedVehicleDto = await GetRentedVehicleByIdAsync(rentedVehicleId);
            if (rentedVehicleDto == null)
            {
                ArgumentNullException.ThrowIfNull(rentedVehicleDto);
            }

            var rentedVehicleDbEntity = rentedVehicleDto.ToDbEntity(_rentedVehicleEntityFactory);

            rentedVehicleDbEntity.RentFinishedOn = DateTime.UtcNow;

            _fleetContext.RentedVehicles.Update(rentedVehicleDbEntity);

            await _fleetContext.SaveChangesAsync();

            return await GetRentedVehicleByIdAsync(rentedVehicleId);
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
    }
}
