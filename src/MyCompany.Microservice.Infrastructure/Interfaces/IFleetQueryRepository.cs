using MyCompany.Microservice.Domain.DTO;

namespace MyCompany.Microservice.Infrastructure.Interfaces
{
    /// <summary>
    /// IFleetQueryRepository definition.
    /// </summary>
    public interface IFleetQueryRepository
    {
        /// <summary>
        /// Get all available <see cref="VehicleDto"/> in the fleet.
        /// </summary>
        /// <param name="fleetId">Fleet identifier.</param>
        /// <returns>Collection of <see cref="VehicleDto"/>.</returns>
        Task<IReadOnlyCollection<VehicleDto>> GetAvailableFleetVehiclesAsync(Guid fleetId);

        /// <summary>
        /// Get an instance of <see cref="FleetDto"/> by its identifier.
        /// </summary>
        /// <param name="fleetId">Fleet identifier.</param>
        /// <returns>Instance of <see cref="FleetDto"/>.</returns>
        Task<FleetDto?> GetFleetByIdAsync(Guid fleetId);
    }
}
