using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NatureHelp.Controllers;
[ApiController]
[AllowAnonymous]
[Route("api/[controller]")]
public class BaseController<T> : Controller where T : class
{
    protected readonly IBaseService<T> _service;

    public BaseController(IBaseService<T> service)
    {
        _service = service;
    }

    [HttpGet("")]
    public virtual async Task<IActionResult> GetList([FromQuery] int scrollCount) => Ok(await _service.GetList(scrollCount));

    [HttpGet("{id}")]
    public virtual async Task<IActionResult> GetById(Guid id)
    {
        var entity = await _service.GetByIdAsync(id);
        return entity == null ? NotFound() : Ok(entity);
    }

    [HttpPost("")]
    public virtual async Task<IActionResult> Create([FromBody] T entity)
    {
        var createdEntity = await _service.AddAsync(entity);
        return CreatedAtAction(nameof(GetById), new { id = createdEntity }, createdEntity);
    }

    [HttpPut("{id}")]
    public virtual async Task<IActionResult> Update(Guid id, [FromBody] T entity)
    {
        var updatedEntity = await _service.UpdateAsync(entity);
        return Ok(updatedEntity);
    }
}

