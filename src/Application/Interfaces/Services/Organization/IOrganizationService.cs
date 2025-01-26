using Domain.Enums;
using Domain.Models.Organization;

namespace Application.Interfaces.Services.Organization;
public interface IOrganizationService
{
    public Task<User> AddUserToOrganization();
    public Task<Domain.Models.Organization.Organization> GetOrganizationInfo(Guid organizationId);
    public Task<User> SetRoleToUser(Guid user, ERole role);

}
