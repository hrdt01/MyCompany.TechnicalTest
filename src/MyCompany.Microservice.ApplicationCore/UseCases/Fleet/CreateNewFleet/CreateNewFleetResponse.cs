using MyCompany.Microservice.Domain.DTO;

namespace MyCompany.Microservice.Application.UseCases.Fleet.CreateNewFleet
{
    /// <summary>
    /// CreateNewFleetResponse definition.
    /// </summary>
    public class CreateNewFleetResponse
    {
        /// <summary>
        /// Gets or sets the fleet.
        /// </summary>
        public FleetDto? Fleet { get; set; }
    }
}
