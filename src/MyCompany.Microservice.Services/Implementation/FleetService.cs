using Microsoft.Extensions.Logging;
using MyCompany.Microservice.Domain.DTO;
using MyCompany.Microservice.Infrastructure.Interfaces;
using MyCompany.Microservice.Infrastructure.Logging;
using MyCompany.Microservice.Services.Interfaces;

namespace MyCompany.Microservice.Services.Implementation
{
    /// <inheritdoc />
    public class FleetService : IFleetService
    {
        private readonly IFleetRepository _fleetRepository;
        private readonly IFleetQueryRepository _fleetQueryRepository;
        private readonly ILogger<FleetService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="FleetService"/> class.
        /// </summary>
        /// <param name="fleetRepository">Instance of <see cref="IFleetRepository"/>.</param>
        /// <param name="fleetQueryRepository">Instance of <see cref="IFleetQueryRepository"/>.</param>
        /// <param name="logger">Instance of <see cref="ILogger"/>.</param>
        public FleetService(
            IFleetRepository fleetRepository,
            IFleetQueryRepository fleetQueryRepository,
            ILogger<FleetService> logger)
        {
            ArgumentNullException.ThrowIfNull(fleetRepository);
            ArgumentNullException.ThrowIfNull(fleetQueryRepository);
            ArgumentNullException.ThrowIfNull(logger);

            _fleetRepository = fleetRepository;
            _fleetQueryRepository = fleetQueryRepository;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task<FleetDto?> AddNewFleet(string newFleetName)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(newFleetName);

            var fleetDto = new FleetDto { FleetName = newFleetName };

            var result = await _fleetRepository.AddNewFleetAsync(fleetDto);

            return result != null ? await _fleetQueryRepository.GetFleetByIdAsync(result.FleetId) : null;
        }

        /// <inheritdoc />
        public async Task<FleetDto?> AddNewVehicle(FleetDto sourceFleet, VehicleDto sourceVehicle)
        {
            ArgumentNullException.ThrowIfNull(sourceFleet);
            ArgumentNullException.ThrowIfNull(sourceVehicle);

            var existingFleet = await _fleetQueryRepository.GetFleetByIdAsync(sourceFleet.FleetId);
            if (existingFleet == null)
            {
                _logger.LogWarningNotFoundFleet(
                    $"{nameof(FleetService)} - {nameof(AddNewVehicle)} - ",
                    sourceFleet.FleetId.ToString());

                ArgumentNullException.ThrowIfNull(existingFleet);
            }

            var result = await _fleetRepository.AddNewVehicleToFleetAsync(existingFleet.FleetId, sourceVehicle);

            return result != null ? await _fleetQueryRepository.GetFleetByIdAsync(result.FleetId) : null;
        }

        /// <inheritdoc />
        public async Task<IReadOnlyCollection<VehicleDto>> GetAvailableFleetVehicles(FleetDto sourceFleet)
        {
            ArgumentNullException.ThrowIfNull(sourceFleet);

            return await _fleetQueryRepository.GetAvailableFleetVehiclesAsync(sourceFleet.FleetId);
        }
    }
}
