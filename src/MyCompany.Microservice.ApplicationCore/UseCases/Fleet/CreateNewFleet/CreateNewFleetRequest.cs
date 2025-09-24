using MediatR;

namespace MyCompany.Microservice.Application.UseCases.Fleet.CreateNewFleet
{
    /// <inheritdoc />
    public class CreateNewFleetRequest : IRequest<CreateNewFleetResponse>
    {
        /// <summary>
        /// Gets or sets the fleet name.
        /// </summary>
        public string FleetName { get; set; } = null!;
    }
}
