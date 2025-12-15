using MyCompany.Microservice.Domain.DTO;
using MyCompany.Microservice.Domain.Interfaces;
using MyCompany.Microservice.Infrastructure.Database;
using MyCompany.Microservice.Infrastructure.Interfaces;
using MyCompany.Microservice.Infrastructure.Mappers;

namespace MyCompany.Microservice.Infrastructure.Implementation
{
    /// <inheritdoc />
    public class CustomerCommandRepository : ICustomerCommandRepository
    {
        private readonly FleetContext _fleetContext;
        private readonly IRentedVehicleEntityFactory _rentedVehicleEntityFactory;
        private readonly ICustomerEntityFactory _customerEntityFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerCommandRepository"/> class.
        /// </summary>
        /// <param name="fleetContext">DB context.</param>
        /// <param name="rentedVehicleEntityFactory">Instance of <see cref="IRentedVehicleEntityFactory"/>.</param>
        /// <param name="customerEntityFactory">Instance of <see cref="ICustomerEntityFactory"/>.</param>
        public CustomerCommandRepository(
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
        }

        /// <inheritdoc />
        public async Task<CustomerDto?> AddNewCustomerAsync(CustomerDto newCustomer)
        {
            ArgumentNullException.ThrowIfNull(newCustomer);

            var customerDbInstance = newCustomer.ToDbEntity(_customerEntityFactory);
            await _fleetContext.Customers.AddAsync(customerDbInstance);
            var result = await _fleetContext.SaveChangesAsync();

            return result > 0 ? customerDbInstance.ToDtoFromDbEntity() : null;
        }

        /// <inheritdoc />
        public async Task<RentedVehicleDto?> RentVehicleAsync(RentedVehicleDto sourceRentedVehicle)
        {
            ArgumentNullException.ThrowIfNull(sourceRentedVehicle);

            var newRentedVehicleDbInstance = sourceRentedVehicle.ToDbEntity(_rentedVehicleEntityFactory);
            await _fleetContext.RentedVehicles.AddAsync(newRentedVehicleDbInstance);

            var result = await _fleetContext.SaveChangesAsync();

            return result > 0 ? newRentedVehicleDbInstance.FromDbEntityToDto() : null;
        }

        /// <inheritdoc />
        public async Task<RentedVehicleDto?> ReturnRentedVehicle(RentedVehicleDto sourceRentedVehicle)
        {
            ArgumentNullException.ThrowIfNull(sourceRentedVehicle);

            var rentedVehicleDbEntity = sourceRentedVehicle.ToDbEntity(_rentedVehicleEntityFactory);

            rentedVehicleDbEntity.RentFinishedOn = DateTime.UtcNow;

            _fleetContext.RentedVehicles.Update(rentedVehicleDbEntity);

            var result = await _fleetContext.SaveChangesAsync();

            return result > 0 ? rentedVehicleDbEntity.FromDbEntityToDto() : null;
        }
    }
}
