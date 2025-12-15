using MyCompany.Microservice.Domain.DTO;

namespace MyCompany.Microservice.Infrastructure.Interfaces
{
    /// <summary>
    /// ICustomerQueryRepository definition.
    /// </summary>
    public interface ICustomerQueryRepository
    {
        /// <summary>
        /// Get an instance of <see cref="RentedVehicleDto"/> by its identifier.
        /// </summary>
        /// <param name="rentedVehicleId">Rented vehicle identifier.</param>
        /// <returns>Instance of <see cref="RentedVehicleDto"/>.</returns>
        Task<RentedVehicleDto?> GetRentedVehicleByIdAsync(Guid rentedVehicleId);

        /// <summary>
        /// Get an instance of <see cref="CustomerDto"/> by its identifier.
        /// </summary>
        /// <param name="customerId">Customer identifier.</param>
        /// <returns>Instance of <see cref="CustomerDto"/>.</returns>
        Task<CustomerDto?> GetCustomerByIdAsync(Guid customerId);

        /// <summary>
        /// Get an instance of <see cref="RentedVehicleDto"/> by its identifier belonging to a customer.
        /// </summary>
        /// <param name="rentedVehicleId">Rented vehicle identifier.</param>
        /// <param name="customerId">Customer identifier.</param>
        /// <returns>Instance of <see cref="RentedVehicleDto"/>.</returns>
        Task<RentedVehicleDto?> GetRentedVehicleByIdAndCustomerIdAsync(Guid rentedVehicleId, Guid customerId);

        /// <summary>
        /// Get a collection of rented vehicles by customer.
        /// </summary>
        /// <param name="customerId">Customer identifier.</param>
        /// <returns>Collection of <see cref="RentedVehicleDto"/>.</returns>
        Task<IEnumerable<RentedVehicleDto>?> GetRentedVehiclesByCustomerIdAsync(Guid customerId);
    }
}
