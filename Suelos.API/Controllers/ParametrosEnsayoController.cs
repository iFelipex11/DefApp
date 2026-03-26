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
public class ParametrosEnsayoController(AppDbContext context) : ControllerBase
{
    private readonly AppDbContext _context = context;

    [HttpGet]
    public async Task<ActionResult> Get()
    {
        var parametros = await _context.ParametrosEnsayo
            .Include(x => x.TipoEnsayo)
            .OrderBy(x => x.Nombre)
            .ToListAsync();

        return Ok(parametros);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult> Get(int id)
    {
        var parametro = await _context.ParametrosEnsayo
            .Include(x => x.TipoEnsayo)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (parametro is null)
        {
            return NotFound();
        }

        return Ok(parametro);
    }

    [HttpPost]
    public async Task<ActionResult> Post(ParametroEnsayo parametroEnsayo)
    {
        var tipoExists = await _context.TiposEnsayo
            .AnyAsync(x => x.Id == parametroEnsayo.TipoEnsayoId);

        if (!tipoExists)
        {
            return BadRequest("El tipo de ensayo asociado no existe.");
        }

        var exists = await _context.ParametrosEnsayo
            .AnyAsync(x => x.TipoEnsayoId == parametroEnsayo.TipoEnsayoId &&
                           x.Nombre == parametroEnsayo.Nombre);

        if (exists)
        {
            return BadRequest("Ya existe un parametro con ese nombre para el tipo de ensayo.");
        }

        _context.Add(parametroEnsayo);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = parametroEnsayo.Id }, parametroEnsayo);
    }

    [HttpPut]
    public async Task<ActionResult> Put(ParametroEnsayo parametroEnsayo)
    {
        var currentParametro = await _context.ParametrosEnsayo
            .FirstOrDefaultAsync(x => x.Id == parametroEnsayo.Id);

        if (currentParametro is null)
        {
            return NotFound();
        }

        var tipoExists = await _context.TiposEnsayo
            .AnyAsync(x => x.Id == parametroEnsayo.TipoEnsayoId);

        if (!tipoExists)
        {
            return BadRequest("El tipo de ensayo asociado no existe.");
        }

        var exists = await _context.ParametrosEnsayo
            .AnyAsync(x => x.TipoEnsayoId == parametroEnsayo.TipoEnsayoId &&
                           x.Nombre == parametroEnsayo.Nombre &&
                           x.Id != parametroEnsayo.Id);

        if (exists)
        {
            return BadRequest("Ya existe un parametro con ese nombre para el tipo de ensayo.");
        }

        _context.Entry(currentParametro).CurrentValues.SetValues(parametroEnsayo);
        await _context.SaveChangesAsync();
        return Ok(currentParametro);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        var affectedRows = await _context.ParametrosEnsayo
            .Where(x => x.Id == id)
            .ExecuteDeleteAsync();

        if (affectedRows == 0)
        {
            return NotFound();
        }

        return NoContent();
    }
}
