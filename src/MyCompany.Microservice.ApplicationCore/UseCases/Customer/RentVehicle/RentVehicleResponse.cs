using MyCompany.Microservice.Domain.DTO;

namespace MyCompany.Microservice.Application.UseCases.Customer.RentVehicle
{
    /// <summary>
    /// RentVehicleResponse definition.
    /// </summary>
    public class RentVehicleResponse
    {
        /// <summary>
        /// Gets or sets the rented vehicle.
        /// </summary>
        public RentedVehicleDto? RentedVehicle { get; set; }
    }
}
