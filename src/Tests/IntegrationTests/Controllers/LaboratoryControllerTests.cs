using Domain.Models.Organization;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Http.Json;

namespace Tests.IntegrationTests.Controllers;
public class LaboratoryControllerTests : IClassFixture<NatureHelpWebAppFactory>
{
    private readonly HttpClient _client;
    private readonly ILogger<LaboratoryControllerTests> _logger;

    public LaboratoryControllerTests(NatureHelpWebAppFactory factory)
    {
        _client = factory.CreateClient();

        var score = factory.Services.CreateScope();

        _logger = score.ServiceProvider.GetRequiredService<ILogger<LaboratoryControllerTests>>();
    }


    [Fact]
    public async Task Post_Creates_New_Laboratory()
    {
        // Arrange
        var newLab = new Laboratory
        {
            Title = "Test Lab",
            Researchers = new List<User>(),
            CreatedOn = DateTime.Now,
            ResearchersCount = 1,
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/Laboratory", newLab);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var created = await response.Content.ReadFromJsonAsync<Laboratory>();
        created.Should().NotBeNull();
        created!.Title.Should().Be("Test Lab");
    }

}
