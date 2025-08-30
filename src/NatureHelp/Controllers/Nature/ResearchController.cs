using Domain.Interfaces;
using Domain.Models.Analitycs;
using Microsoft.AspNetCore.Mvc;

namespace NatureHelp.Controllers.Nature;

[Route("api/[controller]")]
public class ResearchController : BaseController<Research>
{
    public ResearchController(IBaseService<Research> researchService)
        : base(researchService) { }
}
