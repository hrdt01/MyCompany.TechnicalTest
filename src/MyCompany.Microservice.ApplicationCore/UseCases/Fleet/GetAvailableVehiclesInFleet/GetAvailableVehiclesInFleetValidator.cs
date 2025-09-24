using FluentValidation;
using MyCompany.Microservice.Application.UseCases.Validators;

namespace MyCompany.Microservice.Application.UseCases.Fleet.GetAvailableVehiclesInFleet
{
    /// <summary>
    /// GetAvailableVehiclesInFleetValidator definition.
    /// </summary>
    public class GetAvailableVehiclesInFleetValidator : AbstractValidator<GetAvailableVehiclesInFleetRequest>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetAvailableVehiclesInFleetValidator"/> class.
        /// </summary>
        public GetAvailableVehiclesInFleetValidator()
        {
            Include(new SuppliedFleetIdInRequestValidator<GetAvailableVehiclesInFleetRequest>());
        }
    }
}
