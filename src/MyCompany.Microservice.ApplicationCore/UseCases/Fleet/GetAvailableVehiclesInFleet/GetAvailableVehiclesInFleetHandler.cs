using MediatR;
using Microsoft.Extensions.Logging;
using MyCompany.Microservice.Domain.DTO;
using MyCompany.Microservice.Infrastructure.Logging;
using MyCompany.Microservice.Services.Interfaces;

namespace MyCompany.Microservice.Application.UseCases.Fleet.GetAvailableVehiclesInFleet
{
    /// <inheritdoc />
    internal sealed class GetAvailableVehiclesInFleetHandler
        : IRequestHandler<GetAvailableVehiclesInFleetRequest, GetAvailableVehiclesInFleetResponse>
    {
        private readonly IFleetService _fleetService;
        private readonly ILogger<GetAvailableVehiclesInFleetHandler> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetAvailableVehiclesInFleetHandler"/> class.
        /// </summary>
        /// <param name="fleetService"><see cref="IFleetService"/> instance.</param>
        /// <param name="logger"><see cref="ILogger"/> instance.</param>
        public GetAvailableVehiclesInFleetHandler(
            IFleetService fleetService,
            ILogger<GetAvailableVehiclesInFleetHandler> logger)
        {
            ArgumentNullException.ThrowIfNull(fleetService);
            ArgumentNullException.ThrowIfNull(logger);

            _fleetService = fleetService;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task<GetAvailableVehiclesInFleetResponse> Handle(GetAvailableVehiclesInFleetRequest request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            _logger.LogInfoHandlingGetRequest(
                $"{nameof(GetAvailableVehiclesInFleetHandler)} - {nameof(Handle)} - ",
                request.FleetId);
            try
            {
                var fleetDto = new FleetDto { FleetId = Guid.Parse(request.FleetId) };
                var response = await _fleetService.GetAvailableFleetVehicles(fleetDto);
                var returnedResponse = new GetAvailableVehiclesInFleetResponse { Vehicles = response };
                return returnedResponse;
            }
            catch (Exception ex)
            {
                _logger.LogErrorProcessingRequest(
                    $"{nameof(GetAvailableVehiclesInFleetHandler)} - {nameof(Handle)} - ", ex.Message, ex.StackTrace);
                throw;
            }
        }
    }
}
