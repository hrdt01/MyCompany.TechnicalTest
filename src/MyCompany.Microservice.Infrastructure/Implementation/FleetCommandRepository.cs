using MyCompany.Microservice.Domain.DbEntities;
using MyCompany.Microservice.Domain.DTO;
using MyCompany.Microservice.Domain.Interfaces;
using MyCompany.Microservice.Infrastructure.Database;
using MyCompany.Microservice.Infrastructure.Interfaces;
using MyCompany.Microservice.Infrastructure.Mappers;

namespace MyCompany.Microservice.Infrastructure.Implementation
{
    /// <inheritdoc />
    public class FleetCommandRepository : IFleetCommandRepository
    {
        private readonly FleetContext _fleetContext;
        private readonly IFleetEntityFactory _fleetEntityFactory;
        private readonly IVehicleEntityFactory _vehicleEntityFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="FleetCommandRepository"/> class.
        /// </summary>
        /// <param name="fleetContext">DB context.</param>
        /// <param name="fleetEntityFactory">Instance of <see cref="IFleetEntityFactory"/>.</param>
        /// <param name="vehicleEntityFactory">Instance of <see cref="IVehicleEntityFactory"/>.</param>
        public FleetCommandRepository(
            FleetContext fleetContext,
            IFleetEntityFactory fleetEntityFactory,
            IVehicleEntityFactory vehicleEntityFactory)
        {
            ArgumentNullException.ThrowIfNull(fleetContext);
            ArgumentNullException.ThrowIfNull(fleetEntityFactory);
            ArgumentNullException.ThrowIfNull(vehicleEntityFactory);

            _fleetContext = fleetContext;
            _fleetContext.Database.EnsureCreated();
            _fleetEntityFactory = fleetEntityFactory;
            _vehicleEntityFactory = vehicleEntityFactory;
        }

        /// <inheritdoc />
        public async Task<FleetVehicleDto?> AddNewVehicleToFleetAsync(Guid fleetId, VehicleDto sourceVehicle)
        {
            ArgumentNullException.ThrowIfNull(fleetId);
            ArgumentNullException.ThrowIfNull(sourceVehicle);

            var newVehicleDbInstance = sourceVehicle.ToDbEntity(_vehicleEntityFactory);
            _ = await _fleetContext.Vehicles.AddAsync(newVehicleDbInstance);

            var newFleetVehicle = new FleetVehicle { FleetId = fleetId, VehicleId = newVehicleDbInstance.VehicleId };
            _ = await _fleetContext.FleetVehicles.AddAsync(newFleetVehicle);

            var result = await _fleetContext.SaveChangesAsync();
            return result > 0 ? newFleetVehicle.ToDtoFromDbEntity() : null;
        }

        /// <inheritdoc />
        public async Task<FleetDto?> AddNewFleetAsync(FleetDto newFleet)
        {
            ArgumentNullException.ThrowIfNull(newFleet);

            var fleetDbInstance = newFleet.ToDbEntity(_fleetEntityFactory);

            await _fleetContext.Fleet.AddAsync(fleetDbInstance);
            var result = await _fleetContext.SaveChangesAsync();
            return result > 0 ? fleetDbInstance.FromDbEntityToDto() : null;
        }
    }
}
