using Domain.Interfaces;
using Domain.Models.Organization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NatureHelp.Controllers.Organization;

[Authorize(Roles = "SuperAdmin")]
[Route("api/[controller]")]
public class OrganizationController : BaseController<Domain.Models.Organization.Organization>
{
    public OrganizationController(IBaseService<Domain.Models.Organization.Organization> soilDeficiencyService)
        : base(soilDeficiencyService) { }
}
