using MyCompany.Microservice.Domain.DTO;

namespace MyCompany.Microservice.Application.UseCases.Customer.ReturnRentedVehicle
{
    /// <summary>
    /// ReturnRentedVehicleResponse definition.
    /// </summary>
    public class ReturnRentedVehicleResponse
    {
        /// <summary>
        /// Gets or sets the rented vehicle.
        /// </summary>
        public RentedVehicleDto? RentedVehicle { get; set; }
    }
}
