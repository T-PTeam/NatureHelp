using System.Net;
using System.Net.Http.Json;
using Application.Dtos;


namespace Tests.IntegrationTests.Controllers;

public class UserControllerTests : IClassFixture<NatureHelpWebAppFactory>
{
    private readonly NatureHelpWebAppFactory _factory;

    public UserControllerTests(NatureHelpWebAppFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetOrganizationUsers_ReturnsForbidden_ForNoAuth()
    {
        var client = _factory.CreateClient();

        var orgId = Guid.NewGuid();
        var scrollCount = 0;
        var response = await client.GetAsync($"/api/user/organization-users?organizationId={orgId}&scrollCount={scrollCount}");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetOrganizationUsers_ReturnsOk_ForOwnerRole()
    {
        var client = _factory.WithTestAuth("Owner").CreateClient();

        var orgId = Guid.NewGuid();
        var scrollCount = 0;
        var response = await client.GetAsync($"/api/user/organization-users?organizationId={orgId}&scrollCount={scrollCount}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task ChangeUsersRoles_ReturnsOk_ForOwnerRole()
    {
        var client = _factory.WithTestAuth("Owner").CreateClient();

        var body = new Dictionary<Guid, int>
        {
            [Guid.NewGuid()] = 1
        };

        var response = await client.PutAsJsonAsync("/api/user/users-roles", body);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task LoginAsync_ReturnsOk_WithoutAuth()
    {
        var client = _factory.CreateClient();

        var dto = new UserLoginDto
        {
            Email = "valentyn@example.com",
            Password = "12341234"
        };

        var response = await client.PostAsJsonAsync("/api/user/login", dto);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task ChangeUsersRoles_ReturnsForbidden_ForManagerRole()
    {
        var client = _factory.WithTestAuth("Manager").CreateClient();

        var body = new Dictionary<Guid, int>
        {
            [Guid.NewGuid()] = 2
        };

        var response = await client.PutAsJsonAsync("/api/user/users-roles", body);

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }
}
