using Domain.Interfaces;
using Domain.Models.Nature;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NatureHelp.Controllers.Nature;

[Route("api/[controller]")]
[AllowAnonymous]
public class SoilDeficiencyController : BaseController<SoilDeficiency>
{
    public SoilDeficiencyController(IBaseService<SoilDeficiency> soilDeficiencyService)
        : base(soilDeficiencyService) { }
}