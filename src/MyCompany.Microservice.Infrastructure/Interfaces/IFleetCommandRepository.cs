using MyCompany.Microservice.Domain.DTO;

namespace MyCompany.Microservice.Infrastructure.Interfaces
{
    /// <summary>
    /// IFleetCommandRepository definition.
    /// </summary>
    public interface IFleetCommandRepository
    {
        /// <summary>
        /// Add new vehicle to the collection of fleet's vehicles.
        /// </summary>
        /// <param name="fleetId">Fleet identifier.</param>
        /// <param name="sourceVehicle">Instance of <see cref="VehicleDto"/>.</param>
        /// <returns>Instance of <see cref="FleetDto"/>.</returns>
        Task<FleetVehicleDto?> AddNewVehicleToFleetAsync(Guid fleetId, VehicleDto sourceVehicle);

        /// <summary>
        /// Add new fleet to the company.
        /// </summary>
        /// <param name="newFleet"><see cref="FleetDto"/> instance.</param>
        /// <returns>Instance of <see cref="FleetDto"/>.</returns>
        Task<FleetDto?> AddNewFleetAsync(FleetDto newFleet);
    }
}
