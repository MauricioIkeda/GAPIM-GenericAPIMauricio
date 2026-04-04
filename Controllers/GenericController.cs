using GAPIM_GenericAPIMauricio.Entities;
using GAPIM_GenericAPIMauricio.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GAPIM_GenericAPIMauricio.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GenericController<TEntity, TRequest, TResponse> : ControllerBase where TEntity : BaseEntity
{
    protected readonly IGenericService<TEntity, TRequest, TResponse> _service;

    public GenericController(IGenericService<TEntity, TRequest, TResponse> service)
    {
        _service = service;
    }

    [HttpGet]
    public virtual async Task<ActionResult<List<TResponse>>> GetAllAsync(CancellationToken cancellationToken)
    {
        var result = await _service.GetAllAsync(cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public virtual async Task<ActionResult<TResponse>> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var result  = await _service.GetByIdAsync(id, cancellationToken);
            return Ok(result);
        }
        catch(KeyNotFoundException)
        {
            return NotFound(new { message = "Entidade nao encontrada!" });
        }
    }

    [HttpPost]
    public virtual async Task<ActionResult<TResponse>> AddAsync([FromBody] TRequest request, CancellationToken cancellationToken)
    {
        var result = await _service.AddAsync(request, cancellationToken);
        return Created(string.Empty, result);
    }

    [HttpPut("{id:guid}")]
    public virtual async Task<ActionResult> UpdateAsync(Guid id, [FromBody] TRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await _service.UpdateAsync(id, request, cancellationToken);
            return Ok(result);
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { message = "Entidade nao encontrada!" });
        }
    }

    [HttpDelete("{id:guid}")]
    public virtual async Task<IActionResult> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await  _service.DeleteAsync(id, cancellationToken);
            return NoContent();
        }
        catch(Exception exception)
        {
            return BadRequest(new { Message = exception.Message });
        }
    }
}