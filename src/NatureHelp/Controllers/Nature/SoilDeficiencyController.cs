using Domain.Interfaces;
using Domain.Models.Nature;
using Microsoft.AspNetCore.Mvc;

namespace NatureHelp.Controllers.Nature;

[Route("api/[controller]")]
public class SoilDeficiencyController : BaseController<SoilDeficiency>
{
    public SoilDeficiencyController(IBaseService<SoilDeficiency> soilDeficiencyService)
        : base(soilDeficiencyService) { }
}