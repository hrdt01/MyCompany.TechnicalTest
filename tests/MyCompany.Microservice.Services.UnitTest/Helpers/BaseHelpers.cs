using Moq;
using MyCompany.Microservice.Infrastructure.Interfaces;

namespace MyCompany.Microservice.Services.UnitTest.Helpers
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
            CustomerRepositoryMock = new Mock<ICustomerRepository>();
            FleetCommandRepositoryMock = new Mock<IFleetCommandRepository>();
            FleetQueryRepositoryMock = new Mock<IFleetQueryRepository>();
        }

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
        public Mock<ICustomerRepository> CustomerRepositoryMock { get; }
    }
}
