using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Suelos.API.Data;
using Suelos.Shared.Entities;

namespace Suelos.API.Controllers;

[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Route("api/[controller]")]
public class TiposEnsayoController(AppDbContext context) : ControllerBase
{
    private readonly AppDbContext _context = context;

    [HttpGet]
    public async Task<ActionResult> Get()
    {
        var tiposEnsayo = await _context.TiposEnsayo
            .OrderBy(x => x.Nombre)
            .ToListAsync();

        return Ok(tiposEnsayo);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult> Get(int id)
    {
        var tipoEnsayo = await _context.TiposEnsayo
            .FirstOrDefaultAsync(x => x.Id == id);

        if (tipoEnsayo is null)
        {
            return NotFound();
        }

        return Ok(tipoEnsayo);
    }

    [HttpPost]
    public async Task<ActionResult> Post(TipoEnsayo tipoEnsayo)
    {
        var codigoExists = await _context.TiposEnsayo
            .AnyAsync(x => x.Codigo == tipoEnsayo.Codigo);

        if (codigoExists)
        {
            return BadRequest("Ya existe un tipo de ensayo con ese codigo.");
        }

        var nombreExists = await _context.TiposEnsayo
            .AnyAsync(x => x.Nombre == tipoEnsayo.Nombre);

        if (nombreExists)
        {
            return BadRequest("Ya existe un tipo de ensayo con ese nombre.");
        }

        _context.Add(tipoEnsayo);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = tipoEnsayo.Id }, tipoEnsayo);
    }

    [HttpPut]
    public async Task<ActionResult> Put(TipoEnsayo tipoEnsayo)
    {
        var currentTipoEnsayo = await _context.TiposEnsayo
            .FirstOrDefaultAsync(x => x.Id == tipoEnsayo.Id);

        if (currentTipoEnsayo is null)
        {
            return NotFound();
        }

        var codigoExists = await _context.TiposEnsayo
            .AnyAsync(x => x.Codigo == tipoEnsayo.Codigo && x.Id != tipoEnsayo.Id);

        if (codigoExists)
        {
            return BadRequest("Ya existe un tipo de ensayo con ese codigo.");
        }

        var nombreExists = await _context.TiposEnsayo
            .AnyAsync(x => x.Nombre == tipoEnsayo.Nombre && x.Id != tipoEnsayo.Id);

        if (nombreExists)
        {
            return BadRequest("Ya existe un tipo de ensayo con ese nombre.");
        }

        _context.Entry(currentTipoEnsayo).CurrentValues.SetValues(tipoEnsayo);
        await _context.SaveChangesAsync();
        return Ok(currentTipoEnsayo);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        var affectedRows = await _context.TiposEnsayo
            .Where(x => x.Id == id)
            .ExecuteDeleteAsync();

        if (affectedRows == 0)
        {
            return NotFound();
        }

        return NoContent();
    }
}
