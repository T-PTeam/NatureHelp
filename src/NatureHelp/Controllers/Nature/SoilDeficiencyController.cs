using Domain.Interfaces;
using Domain.Models.Nature;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NatureHelp.Controllers.Nature;

[Authorize(Roles = "SuperAdmin, Owner, Manager, Researcher")]
[Route("api/[controller]")]
public class SoilDeficiencyController : BaseController<SoilDeficiency>
{
    public SoilDeficiencyController(IBaseService<SoilDeficiency> soilDeficiencyService)
        : base(soilDeficiencyService) { }
}