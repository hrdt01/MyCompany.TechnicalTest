using Microsoft.Extensions.Logging;
using MyCompany.Microservice.Domain.DTO;
using MyCompany.Microservice.Infrastructure.Interfaces;
using MyCompany.Microservice.Infrastructure.Logging;
using MyCompany.Microservice.Services.Interfaces;

namespace MyCompany.Microservice.Services.Implementation
{
    /// <inheritdoc />
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerCommandRepository _customerRepository;
        private readonly IFleetQueryRepository _fleetQueryRepository;
        private readonly ICustomerQueryRepository _customerQueryRepository;
        private readonly ILogger<CustomerService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerService"/> class.
        /// </summary>
        /// <param name="customerRepository">Instance of <see cref="ICustomerCommandRepository"/>.</param>
        /// <param name="fleetQueryRepository">Instance of <see cref="IFleetCommandRepository"/>.</param>
        /// <param name="customerQueryRepository">Instance of <see cref="ICustomerQueryRepository"/>.</param>
        /// <param name="logger">Instance of <see cref="ILogger"/>.</param>
        public CustomerService(
            ICustomerCommandRepository customerRepository,
            IFleetQueryRepository fleetQueryRepository,
            ICustomerQueryRepository customerQueryRepository,
            ILogger<CustomerService> logger)
        {
            ArgumentNullException.ThrowIfNull(customerRepository);
            ArgumentNullException.ThrowIfNull(fleetQueryRepository);
            ArgumentNullException.ThrowIfNull(logger);

            _customerRepository = customerRepository;
            _fleetQueryRepository = fleetQueryRepository;
            _customerQueryRepository = customerQueryRepository;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task<RentedVehicleDto?> RentVehicleAsync(RentedVehicleDto source)
        {
            ArgumentNullException.ThrowIfNull(source);
            var availableVehicles =
                await _fleetQueryRepository.GetAvailableFleetVehiclesAsync(source.FleetId);
            var activeRentedVehicles = await CustomerHasActiveRentedVehiclesAsync(source);
            var isAvailable = availableVehicles.Any(vehicle => vehicle.VehicleId == source.VehicleId);
            var result = !isAvailable || activeRentedVehicles
                ? null
                : await _customerRepository.RentVehicleAsync(source);
            return result != null
                ? await _customerQueryRepository.GetRentedVehicleByIdAsync(result.RentedVehicleId)
                : null;
        }

        /// <inheritdoc />
        public async Task<RentedVehicleDto?> ReturnRentedVehicleAsync(RentedVehicleDto rentedVehicle)
        {
            ArgumentNullException.ThrowIfNull(rentedVehicle);

            var existingRentedVehicle =
                await _customerQueryRepository.GetRentedVehicleByIdAndCustomerIdAsync(
                    rentedVehicle.RentedVehicleId,
                    rentedVehicle.CustomerId);

            if (existingRentedVehicle == null)
            {
                _logger.LogWarningNotFoundRentedVehicle(
                    $"{nameof(CustomerService)} - {nameof(ReturnRentedVehicleAsync)} - ",
                    rentedVehicle.RentedVehicleId.ToString());

                ArgumentNullException.ThrowIfNull(existingRentedVehicle);
            }

            var result = await _customerRepository.ReturnRentedVehicle(existingRentedVehicle);

            return result != null ? await _customerQueryRepository.GetRentedVehicleByIdAsync(result.RentedVehicleId) : null;
        }

        /// <inheritdoc />
        public async Task<CustomerDto?> AddNewCustomerAsync(string newCustomerName)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(newCustomerName);

            var customerDto = new CustomerDto { CustomerName = newCustomerName };

            var result = await _customerRepository.AddNewCustomerAsync(customerDto);

            return result != null ? await _customerQueryRepository.GetCustomerByIdAsync(result.CustomerId) : null;
        }

        /// <inheritdoc />
        public async Task<bool> CustomerHasActiveRentedVehiclesAsync(RentedVehicleDto source)
        {
            ArgumentNullException.ThrowIfNull(source);
            var allRentingsByUser = await _customerQueryRepository.GetRentedVehiclesByCustomerIdAsync(source.CustomerId);
            return allRentingsByUser != null && allRentingsByUser.Any(renting => renting.EndRent > DateTime.UtcNow);
        }
    }
}
