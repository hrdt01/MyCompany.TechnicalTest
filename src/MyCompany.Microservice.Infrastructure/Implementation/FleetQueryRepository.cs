using System.Collections.ObjectModel;
using System.Data.Common;
using System.Text;
using Dapper;
using Microsoft.EntityFrameworkCore;
using MyCompany.Microservice.Domain.DbEntities;
using MyCompany.Microservice.Domain.DTO;
using MyCompany.Microservice.Infrastructure.Database;
using MyCompany.Microservice.Infrastructure.Interfaces;
using MyCompany.Microservice.Infrastructure.Mappers;

namespace MyCompany.Microservice.Infrastructure.Implementation
{
    /// <inheritdoc />
    public class FleetQueryRepository : IFleetQueryRepository
    {
        private readonly DbConnection _connection;

        /// <summary>
        /// Initializes a new instance of the <see cref="FleetQueryRepository"/> class.
        /// </summary>
        /// <param name="fleetContext">DB context.</param>
        public FleetQueryRepository(FleetContext fleetContext)
        {
            ArgumentNullException.ThrowIfNull(fleetContext);

            fleetContext.Database.EnsureCreated();
            _connection = fleetContext.Database.GetDbConnection();
        }

        /// <inheritdoc />
        public async Task<IReadOnlyCollection<VehicleDto>> GetAvailableFleetVehiclesAsync(Guid fleetId)
        {
            ArgumentNullException.ThrowIfNull(fleetId);

            var fleetFromDb = await GetFleetByIdAsync(fleetId);
            if (fleetFromDb == null)
            {
                ArgumentNullException.ThrowIfNull(fleetFromDb);
            }

            // Querying rented vehicles that currently has a rent start date
            // and with rent end date defined but not accomplished yet
            var currentNow = DateTime.UtcNow;

            var paramValue = new DynamicParameters();
            var valueToQuery = currentNow.ToString("O").Replace('T', ' ');
            var lastIndex = valueToQuery.Length - 1;
            valueToQuery = valueToQuery[0..lastIndex];
            paramValue.Add("rentFinishedOn", valueToQuery);
            paramValue.Add("fleetId", fleetId);

            var queryForCurrentRented =
                "SELECT VehicleId FROM RentedVehicle WHERE FleetId = @fleetId AND RentFinishedOn IS NOT NULL AND RentFinishedOn > @rentFinishedOn";
            var currentRented = await _connection.QueryAsync<string>(queryForCurrentRented, paramValue);

            // From fleet, only are available those vehicles whose vehicle id is not present in the previous collection
            var subset =
                fleetFromDb.Vehicles?.Where(vehicle => !IsCurrentlyRented(currentRented, vehicle.VehicleId.ToString()))
                    .ToArray();
            return subset != null
                ? new ReadOnlyCollection<VehicleDto>(subset)
                : ReadOnlyCollection<VehicleDto>.Empty;
        }

        /// <inheritdoc />
        public async Task<FleetDto?> GetFleetByIdAsync(Guid fleetId)
        {
            ArgumentNullException.ThrowIfNull(fleetId);

            var paramValue = new DynamicParameters();
            paramValue.Add("fleetId", fleetId);

            var queryForFleetToExecute =
                "SELECT FleetId, FleetName FROM Fleet WHERE FleetId = @fleetId ORDER BY FleetId ASC;";
            var queryForFleetVehiclesToExecute =
                "SELECT FleetVehicleId, FleetId, VehicleId FROM FleetVehicle WHERE FleetId = @fleetId ORDER BY FleetId ASC;";
            var queryForVehiclesToExecute =
                "SELECT FV.VehicleId, V.Brand, V.Model, V.ManufacturedOn " +
                "FROM FleetVehicle AS FV LEFT JOIN Vehicle AS V ON FV.VehicleId = V.VehicleId " +
                "WHERE FV.FleetId = @fleetId ORDER BY FV.FleetId ASC;";

            var queryToExecute = new StringBuilder();
            queryToExecute.AppendLine(queryForFleetToExecute);
            queryToExecute.AppendLine(queryForFleetVehiclesToExecute);
            queryToExecute.AppendLine(queryForVehiclesToExecute);
            await using var multi = await _connection.QueryMultipleAsync(queryToExecute.ToString(), paramValue);
            var fleet = await multi.ReadFirstOrDefaultAsync<Fleet>();
            var fleetVehicles = await multi.ReadAsync<FleetVehicle>();
            var vehicles = await multi.ReadAsync<Vehicle>();

            if (fleet == null)
            {
                return null;
            }

            if (!fleetVehicles.Any() || !vehicles.Any())
            {
                return new FleetDto() { FleetId = fleet.FleetId, FleetName = fleet.FleetName };
            }

            var result = new FleetDto() { FleetId = fleet.FleetId, FleetName = fleet.FleetName };

            foreach (var fleetVehicle in fleetVehicles)
            {
                fleetVehicle.Vehicle = vehicles.FirstOrDefault(vehicle => vehicle.VehicleId == fleetVehicle.VehicleId);
            }

            result.Vehicles = fleetVehicles.FromDbEntityToDto();
            return result;
        }

        /// <summary>
        /// Checks if an element to check exists in a given collection of elements.
        /// </summary>
        /// <param name="sourceCollection">Collection of elements.</param>
        /// <param name="elementToCheck">Element to check.</param>
        /// <returns>Boolean value.</returns>
        private static bool IsCurrentlyRented(IEnumerable<string> sourceCollection, string elementToCheck)
        {
#pragma warning disable CA1309
            return sourceCollection.Any(s => s.Equals(elementToCheck, StringComparison.InvariantCultureIgnoreCase));
#pragma warning restore CA1309
        }
    }
}
