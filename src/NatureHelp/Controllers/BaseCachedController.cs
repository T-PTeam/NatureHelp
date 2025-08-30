using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Dtos;


namespace NatureHelp.Controllers;
[ApiController]
[AllowAnonymous]
[Route("api/[controller]")]
public class BaseCachedController<T> : BaseController<T> where T : class
{
    public BaseCachedController(IBaseService<T> service)
        : base(service) { }

    [HttpGet("")]
    public override async Task<IActionResult> GetList([FromQuery] int scrollCount, [FromQuery] IDictionary<string, string?>? filters)
    {
        Func<Task<ListData<T>>> fetchFromDb = () => _service.GetList(scrollCount, filters);

        var listData = await _service.GetOrSetAsync(fetchFromDb);

        return Ok(listData);
    }
}

