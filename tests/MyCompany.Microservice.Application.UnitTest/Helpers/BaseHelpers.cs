using Microsoft.Extensions.Logging.Testing;
using Moq;
using MyCompany.Microservice.Infrastructure.Interfaces;
using MyCompany.Microservice.Services.Implementation;

namespace MyCompany.Microservice.Application.UnitTest.Helpers
{
    /// <summary>
    /// BaseHelpers class.
    /// </summary>
    public class BaseHelpers
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseHelpers"/> class.
        /// </summary>
        public BaseHelpers()
        {
            CustomerCommandRepositoryMock = new Mock<ICustomerCommandRepository>();
            CustomerQueryRepositoryMock = new Mock<ICustomerQueryRepository>();
            FleetCommandRepositoryMock = new Mock<IFleetCommandRepository>();
            FleetQueryRepositoryMock = new Mock<IFleetQueryRepository>();

            CustomerServiceMock = new Mock<CustomerService>(
                CustomerCommandRepositoryMock.Object,
                FleetQueryRepositoryMock.Object,
                CustomerQueryRepositoryMock.Object,
                new FakeLogger<CustomerService>());

            FleetServiceMock = new Mock<FleetService>(
                FleetCommandRepositoryMock.Object,
                FleetQueryRepositoryMock.Object,
                new FakeLogger<FleetService>());
        }

        /// <summary>
        /// Gets the mock instance.
        /// </summary>
        public Mock<FleetService> FleetServiceMock { get; }

        /// <summary>
        /// Gets the mock instance.
        /// </summary>
        public Mock<CustomerService> CustomerServiceMock { get; }

        /// <summary>
        /// Gets the mock instance.
        /// </summary>
        public Mock<IFleetCommandRepository> FleetCommandRepositoryMock { get; }

        /// <summary>
        /// Gets the mock instance.
        /// </summary>
        public Mock<IFleetQueryRepository> FleetQueryRepositoryMock { get; }

        /// <summary>
        /// Gets the mock instance.
        /// </summary>
        public Mock<ICustomerCommandRepository> CustomerCommandRepositoryMock { get; }

        /// <summary>
        /// Gets the mock instance.
        /// </summary>
        public Mock<ICustomerQueryRepository> CustomerQueryRepositoryMock { get; }
    }
}
