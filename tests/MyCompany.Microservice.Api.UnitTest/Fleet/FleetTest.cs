using System.Net.Http.Json;
using MyCompany.Microservice.Api.Models;
using MyCompany.Microservice.Api.UnitTest.Helpers;
using MyCompany.Microservice.Application.UseCases.Fleet.AddNewVehicleToFleet;
using MyCompany.Microservice.Application.UseCases.Fleet.CreateNewFleet;
using MyCompany.Microservice.Application.UseCases.Fleet.GetAvailableVehiclesInFleet;
using MyCompany.Microservice.BaseTest.TestHelpers;

namespace MyCompany.Microservice.Api.UnitTest.Fleet
{
    /// <summary>
    /// Test class for Fleet related test cases.
    /// </summary>
    [TestFixture]
    public class FleetTest
    {
        private IntegrationTestWebApplicationFactory _factory;

        /// <summary>
        /// Method to allocate resources.
        /// </summary>
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _factory = new IntegrationTestWebApplicationFactory();
        }

        /// <summary>
        /// Method to free resources.
        /// </summary>
        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _factory.Dispose();
        }

        /// <summary>
        /// Test to GetAvailableVehicles.
        /// </summary>
        /// <returns>Task.</returns>
        [Test]
        public async Task GetAvailableVehiclesShouldReturnFailBecauseNotExistingFleet()
        {
            // Arrange
            var endpointToConsume =
                UseCasesEndpoints.GetAvailableVehiclesEndpoint
                    .Replace("{fleetId}", BaseTestConstants.FleetIdTest.ToString(), StringComparison.InvariantCultureIgnoreCase);

            // Act
            var result = await _factory.HttpClient.GetAsync(new Uri(endpointToConsume, UriKind.Relative));

            // Assert
            Assert.That(result.IsSuccessStatusCode, Is.False);
        }

        /// <summary>
        /// Test to GetAvailableVehicles.
        /// </summary>
        /// <returns>Task.</returns>
        [Test]
        public async Task GetAvailableVehiclesShouldReturnSuccessBecauseExistsFleetWithoutVehicles()
        {
            // Arrange
            var newFleetModel = new NewFleetModel
            {
                FleetName = BaseTestConstants.FleetNameTest
            };
            var newFleetEndpoint = UseCasesEndpoints.NewFleetEndpoint;
            var newFleetResponse = await _factory.HttpClient.PostAsJsonAsync(
                new Uri(newFleetEndpoint, UriKind.Relative),
                newFleetModel);
            var newFleetResult = await newFleetResponse.Content.ReadFromJsonAsync<CreateNewFleetResponse>();

            var endpointToConsume =
                UseCasesEndpoints.GetAvailableVehiclesEndpoint
                    .Replace("{fleetId}", newFleetResult!.Fleet!.FleetId.ToString(), StringComparison.InvariantCultureIgnoreCase);

            // Act
            var resultResponse = await _factory.HttpClient.GetAsync(new Uri(endpointToConsume, UriKind.Relative));
            var result = await resultResponse.Content.ReadFromJsonAsync<GetAvailableVehiclesInFleetResponse>();

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(resultResponse.IsSuccessStatusCode, Is.True);
                Assert.That(result, Is.Not.Null);
                Assert.That(result!.Vehicles, Is.Empty);
            }
        }

        /// <summary>
        /// Test to GetAvailableVehicles.
        /// </summary>
        /// <returns>Task.</returns>
        [Test]
        public async Task GetAvailableVehiclesShouldReturnSuccessBecauseExistsFleetWithOneVehicle()
        {
            // Arrange
            var newFleetModel = new NewFleetModel
            {
                FleetName = BaseTestConstants.FleetNameTest
            };
            var newFleetEndpoint = UseCasesEndpoints.NewFleetEndpoint;
            var newFleetResponse = await _factory.HttpClient.PostAsJsonAsync(
                new Uri(newFleetEndpoint, UriKind.Relative),
                newFleetModel);
            var newFleetResult = await newFleetResponse.Content.ReadFromJsonAsync<CreateNewFleetResponse>();

            var newVehicleModel = new AddNewVehicleModel()
            {
                FleetId = newFleetResult!.Fleet!.FleetId,
                VehicleBrand = BaseTestConstants.BrandNameTest,
                VehicleModel = BaseTestConstants.ModelNameTest,
                VehicleManufacturedOn = BaseTestConstants.ManufacturedOnTest
            };
            var addNewVehicleEndpoint = UseCasesEndpoints.AddNewVehicleEndpoint;
            var addNewVehicleResponse = await _factory.HttpClient.PostAsJsonAsync(
                new Uri(addNewVehicleEndpoint, UriKind.Relative),
                newVehicleModel);
            var addNewVehicleResult = await addNewVehicleResponse.Content.ReadFromJsonAsync<AddNewVehicleToFleetResponse>();

            var endpointToConsume =
                UseCasesEndpoints.GetAvailableVehiclesEndpoint
                    .Replace("{fleetId}", newFleetResult.Fleet!.FleetId.ToString(), StringComparison.InvariantCultureIgnoreCase);

            // Act
            var resultResponse = await _factory.HttpClient.GetAsync(new Uri(endpointToConsume, UriKind.Relative));
            var result = await resultResponse.Content.ReadFromJsonAsync<GetAvailableVehiclesInFleetResponse>();

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(resultResponse.IsSuccessStatusCode, Is.True);
                Assert.That(result, Is.Not.Null);
                Assert.That(result!.Vehicles, Is.Not.Empty);
                Assert.That(result.Vehicles!.First().VehicleId, Is.EqualTo(addNewVehicleResult!.Fleet!.Vehicles!.First().VehicleId));
            }
        }
    }
}
