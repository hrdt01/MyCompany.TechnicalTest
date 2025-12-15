using MyCompany.Microservice.Domain.DTO;

namespace MyCompany.Microservice.Services.Interfaces
{
    /// <summary>
    /// ICustomService definition.
    /// </summary>
    public interface ICustomerService
    {
        /// <summary>
        /// Perform the renting process of a vehicle.
        /// </summary>
        /// <param name="source">RentedVehicleDto instance.</param>
        /// <returns>Instance of <see cref="RentedVehicleDto"/>.</returns>
        Task<RentedVehicleDto?> RentVehicleAsync(RentedVehicleDto source);

        /// <summary>
        /// Performs the process to return a rented vehicle.
        /// </summary>
        /// <param name="rentedVehicle">RentedVehicleDto instance.</param>
        /// <returns>Instance of <see cref="RentedVehicleDto"/>.</returns>
        Task<RentedVehicleDto?> ReturnRentedVehicleAsync(RentedVehicleDto rentedVehicle);

        /// <summary>
        /// Add new customer.
        /// </summary>
        /// <param name="newCustomerName">Customer's name.</param>
        /// <returns>Instance of <see cref="CustomerDto"/>.</returns>
        Task<CustomerDto?> AddNewCustomerAsync(string newCustomerName);

        /// <summary>
        /// Check if a customer trying to rent a vehicle still has active rented vehicles.
        /// </summary>
        /// <param name="source">RentedVehicleDto instance.</param>
        /// <returns>Boolean flag.</returns>
        Task<bool> CustomerHasActiveRentedVehiclesAsync(RentedVehicleDto source);
    }
}
