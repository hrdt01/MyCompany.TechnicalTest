using MyCompany.Microservice.Domain.Entities.ValueObjects;
using MyCompany.Microservice.Domain.Interfaces;

namespace MyCompany.Microservice.Domain.Entities
{
    /// <summary>
    /// Entity Customer.
    /// </summary>
    public abstract class Customer : ICustomer
    {
        /// <summary>
        /// Gets or sets Customer identifier.
        /// </summary>
        public CustomerId Id { get; protected set; }

        /// <summary>
        /// Gets or sets Customer name.
        /// </summary>
        public CustomerName CustomerName { get; protected set; }

        /// <summary>
        /// Gets or sets Customer's rented vehicles.
        /// </summary>
        public RentedVehicles RentedVehicles { get; protected set; }
    }
}
