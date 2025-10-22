using Domain.Models.Organization;

namespace Application.Interfaces.Services;

public interface IAuthenticationService
{
    Task<User?> GetCurrentUserAsync();
    Guid? GetCurrentUserId();
    Guid? GetCurrentUserOrganizationId();
}
