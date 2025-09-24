using MyCompany.Microservice.Domain.DTO;

namespace MyCompany.Microservice.Application.UseCases.Customer.CreateNewCustomer
{
    /// <summary>
    /// CreateNewCustomerResponse definition.
    /// </summary>
    public class CreateNewCustomerResponse
    {
        /// <summary>
        /// Gets or sets the Customer.
        /// </summary>
        public CustomerDto? Customer { get; set; }
    }
}
