namespace MyCompany.Microservice.Domain.DTO
{
    /// <summary>
    /// DTO Entity FleetVehicle.
    /// </summary>
    public class FleetVehicleDto : IDtoEntity
    {
        /// <summary>
        /// Gets or sets the fleet vehicle id.
        /// </summary>
        public Guid FleetVehicleId { get; set; }

        /// <summary>
        /// Gets or sets the fleet id.
        /// </summary>
        public Guid FleetId { get; set; }

        /// <summary>
        /// Gets or sets the vehicle id.
        /// </summary>
        public Guid VehicleId { get; set; }
    }
}
