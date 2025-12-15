using MyCompany.Microservice.Domain.DTO;

namespace MyCompany.Microservice.Infrastructure.Interfaces
{
    /// <summary>
    /// ICustomerCommandRepository definition.
    /// </summary>
    public interface ICustomerCommandRepository
    {
        /// <summary>
        /// Perform the renting process of a vehicle.
        /// </summary>
        /// <param name="sourceRentedVehicle">RentedVehicleDto instance.</param>
        /// <returns>Instance of <see cref="RentedVehicleDto"/>.</returns>
        Task<RentedVehicleDto?> RentVehicleAsync(RentedVehicleDto sourceRentedVehicle);

        /// <summary>
        /// Performs the process to return a rented vehicle.
        /// </summary>
        /// <param name="sourceRentedVehicle">RentedVehicleDto instance.</param>
        /// <returns>Instance of <see cref="RentedVehicleDto"/>.</returns>
        Task<RentedVehicleDto?> ReturnRentedVehicle(RentedVehicleDto sourceRentedVehicle);

        /// <summary>
        /// Add new customer.
        /// </summary>
        /// <param name="newCustomer"><see cref="CustomerDto"/> instance.</param>
        /// <returns>Instance of <see cref="CustomerDto"/>.</returns>
        Task<CustomerDto?> AddNewCustomerAsync(CustomerDto newCustomer);
    }
}
