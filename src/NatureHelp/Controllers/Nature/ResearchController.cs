using Domain.Interfaces;
using Domain.Models.Analitycs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NatureHelp.Controllers.Nature;

[Authorize(Roles = "SuperAdmin, Owner, Manager, Researcher")]
[Route("api/[controller]")]
public class ResearchController : BaseController<Research>
{
    public ResearchController(IBaseService<Research> researchService)
        : base(researchService) { }
}
