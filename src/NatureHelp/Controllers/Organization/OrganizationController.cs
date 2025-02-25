using Application.Interfaces.Services.Organization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NatureHelp.Controllers.Organization;

[Authorize(Roles = "SuperAdmin, Owner, Manager")]
[Route("api/[controller]")]
public class OrganizationController : Controller
{
    private readonly IOrganizationService _organizationService;

    public OrganizationController(IOrganizationService organizationService)
    {
        _organizationService = organizationService;
    }
}
