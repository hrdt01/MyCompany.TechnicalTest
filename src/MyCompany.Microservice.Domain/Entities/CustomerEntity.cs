using MyCompany.Microservice.Domain.Entities.ValueObjects;
using MyCompany.Microservice.Domain.Interfaces;

namespace MyCompany.Microservice.Domain.Entities
{
    /// <inheritdoc />
    public class CustomerEntity : Customer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerEntity"/> class.
        /// </summary>
        /// <param name="customerName">Instance of <see cref="CustomerName"/> struct.</param>
        public CustomerEntity(CustomerName customerName)
        {
            CustomerName = customerName;
            Id = new CustomerId(Guid.NewGuid());
            RentedVehicles = new RentedVehicles(Enumerable.Empty<IRentedVehicle>().ToList());
        }
    }
}
