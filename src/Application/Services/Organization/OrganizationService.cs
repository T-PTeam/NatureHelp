using Application.Interfaces.Services.Organization;
using Domain.Enums;
using Domain.Models.Organization;

namespace Application.Services.Organization;
public class OrganizationService : IOrganizationService
{
    public Task<User> AddUserToOrganization()
    {
        throw new NotImplementedException();
    }

    public Task<Domain.Models.Organization.Organization> GetOrganizationInfo(Guid organizationId)
    {
        throw new NotImplementedException();
    }

    public Task<User> SetRoleToUser(Guid user, ERole role)
    {
        throw new NotImplementedException();
    }
}
