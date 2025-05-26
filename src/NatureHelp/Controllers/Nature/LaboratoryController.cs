using Application.Interfaces.Services.Cache;
using Domain.Interfaces;
using Domain.Models.Organization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NatureHelp.Controllers.Nature;

[Authorize(Roles = "SuperAdmin, Owner, Manager, Researcher")]
[Route("api/[controller]")]
public class LaboratoryController : BaseCachedController<Laboratory>
{
    public LaboratoryController(IBaseService<Laboratory> laboratoryService)
        : base(laboratoryService) { }
}
