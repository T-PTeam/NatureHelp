using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NatureHelp.Controllers.Analytics;

[Authorize(Roles = "SuperAdmin, Manager, Supervisor")]
[Route("api/[controller]")]
public class ReportController : Controller
{
    public ReportController()
    {
    }
}
