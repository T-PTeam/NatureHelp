using Domain.Interfaces;
using Domain.Models.Nature;
using Microsoft.AspNetCore.Mvc;

namespace NatureHelp.Controllers.Nature;

[Route("api/[controller]")]
public class WaterDeficiencyController : BaseController<WaterDeficiency>
{
    public WaterDeficiencyController(IBaseService<WaterDeficiency> waterDeficiencyService)
        : base(waterDeficiencyService) { }
}