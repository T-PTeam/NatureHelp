using Application.Interfaces.Services.Organization;
using Domain.Enums;
using Domain.Models.Organization;
using Infrastructure.Interfaces;

namespace Application.Services.Organization;
public class OrganizationService : IOrganizationService
{
    private readonly IOrganizationRepository _organizationRepository;
    public OrganizationService(IOrganizationRepository organizationRepository)
    {
        _organizationRepository = organizationRepository;
    }

    public Task<User> AddUserToOrganizationAsync(Guid userId)
    {
        throw new NotImplementedException();
    }

    public Task<Domain.Models.Organization.Organization> GetOrganizationInfoAsync(Guid organizationId)
    {
        throw new NotImplementedException();
    }

    public Task<User> SetRoleToUserAsync(Guid user, ERole role)
    {
        throw new NotImplementedException();
    }
}
