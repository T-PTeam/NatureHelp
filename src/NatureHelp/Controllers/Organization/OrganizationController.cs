using Application.Interfaces.Services.Organization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NatureHelp.Controllers.Organization;

[Authorize(Roles = "SuperAdmin, Owner, Manager")]
[Route("api/[controller]")]
public class OrganizationController : Controller
{
    private readonly IOrganizationService _organizationService;
    private readonly IUserService _userService;

    public OrganizationController(IOrganizationService organizationService, IUserService userService)
    {
        _userService = userService;
        _organizationService = organizationService;
    }
}
