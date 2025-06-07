using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using System.Net;
using FluentAssertions;
using Domain.Models.Nature;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using DocumentFormat.OpenXml.Wordprocessing;
using Infrastructure.Providers;
using Domain.Enums;

namespace Tests.IntegrationTests.Controllers;
public class WaterDeficiencyControllerTests : IClassFixture<NatureHelpWebAppFactory>
{
    private readonly HttpClient _client;
    private readonly ILogger<WaterDeficiencyControllerTests> _logger;

    public WaterDeficiencyControllerTests(NatureHelpWebAppFactory factory)
    {
        _client = factory.CreateClient();

        var scope = factory.Services.CreateScope();
        _logger = scope.ServiceProvider.GetRequiredService<ILogger<WaterDeficiencyControllerTests>>();
    }

    [Fact]
    public async Task Post_Creates_New_WaterDeficiency()
    {
        // Arrange
        var newDeficiency = new WaterDeficiency
        {
            Title = "Test data",
            PH = 6.8,
            DissolvedOxygen = 8.0,
            LeadConcentration = 0.005,
            MercuryConcentration = 0.0002,
            NitrateConcentration = 20,
            PesticidesContent = 0.001,
            MicrobialActivity = 400,
            RadiationLevel = 2,
            ChemicalOxygenDemand = 150,
            BiologicalOxygenDemand = 10,
            PhosphateConcentration = 1.0,
            CadmiumConcentration = 0.002,
            TotalDissolvedSolids = 800,
            ElectricalConductivity = 1000,
            MicrobialLoad = 1200
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/waterdeficiency", newDeficiency);

        // Assert
        var contentString = await response.Content.ReadAsStringAsync();
        _logger.LogInformation("Response status: {Status}", response.StatusCode);
        _logger.LogInformation("Response body: {Body}", contentString);

        // If it's a bad request, force the test to fail and include content
        if (response.StatusCode == HttpStatusCode.BadRequest)
        {
            throw new Exception($"BadRequest returned. Details: {contentString}");
        }

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        // Optional: deserialize to expected model instead of dynamic
        var result = JsonConvert.DeserializeObject<WaterDeficiency>(contentString);
        result.PH.Should().BeApproximately(6.8, 0.1); // or whatever your logic expects
    }

    [Fact]
    public async Task Put_Updates_WaterDeficiency()
    {
        // Arrange: створюємо новий запис
        var original = new WaterDeficiency
        {
            Title = "First Water def",
            PH = 7.2,
            DissolvedOxygen = 6.8,
            BiologicalOxygenDemand = 4.5,
            NitrateConcentration = 20.0,
            PhosphateConcentration = 2.1,
            LeadConcentration = 0.0015,
            MercuryConcentration = 0.0002,
            CadmiumConcentration = 0.003,
            PesticidesContent = 0.0001,
            TotalDissolvedSolids = 500.0,
            ElectricalConductivity = 1.2,
            EDangerState = EDangerState.Moderate,
            MicrobialLoad = 1500,
            CreatedBy = new Guid("11112222-3333-4444-5555-666677778888"),
            ResponsibleUserId = new Guid("11112222-3333-4444-5555-666677778888"),
            LocationId = new Guid("b1111111-1111-1111-1111-111111111111"),
        };

        var postResponse = await _client.PostAsJsonAsync("/api/waterdeficiency", original);

        var contentString = await postResponse.Content.ReadAsStringAsync();
        _logger.LogInformation("Response status: {Status}", postResponse.StatusCode);
        _logger.LogInformation("Response body: {Body}", contentString);

        if (postResponse.StatusCode != HttpStatusCode.Created)
        {
            throw new Exception($"POST request failed. Details: {contentString}");
        }
        postResponse.EnsureSuccessStatusCode();

        var created = await postResponse.Content.ReadFromJsonAsync<WaterDeficiency>();

        created.Should().NotBeNull();

        // Act: оновлюємо pH
        created.PH = 7.2;

        var response = await _client.PutAsJsonAsync($"/api/waterdeficiency/{created.Id}", created);
        contentString = await response.Content.ReadAsStringAsync();
        _logger.LogInformation("Response status: {Status}", response.StatusCode);
        _logger.LogInformation("Response body: {Body}", contentString);

        if (response.StatusCode != HttpStatusCode.OK)
        {
            throw new Exception($"PUT request failed. Details: {contentString}");
        }

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        // Перевіримо, що pH дійсно оновлений
        var getResponse = await _client.GetAsync($"/api/waterdeficiency/{created.Id}");
        var updated = await getResponse.Content.ReadFromJsonAsync<WaterDeficiency>();
        updated!.PH.Should().BeApproximately(7.2, 0.01);
    }

}
