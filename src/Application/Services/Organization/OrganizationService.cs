using Domain.Interfaces;

namespace Application.Services.Organization;
public class OrganizationService : BaseService<Domain.Models.Organization.Organization>
{
    private readonly IBaseRepository<Domain.Models.Organization.Organization> _userRepository;
    public OrganizationService(IBaseRepository<Domain.Models.Organization.Organization> userRepository)
        : base(userRepository)
    {
        _userRepository = userRepository;
    }
}
