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
public class EnsayosRealizadosController(AppDbContext context) : ControllerBase
{
    private readonly AppDbContext _context = context;

    [HttpGet]
    public async Task<ActionResult> Get()
    {
        var ensayos = await _context.EnsayosRealizados
            .Include(x => x.Muestra)
            .ThenInclude(x => x!.PuntoMuestreo)
            .Include(x => x.TipoEnsayo)
            .OrderByDescending(x => x.FechaAsignacion)
            .ToListAsync();

        return Ok(ensayos);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult> Get(int id)
    {
        var ensayo = await _context.EnsayosRealizados
            .Include(x => x.Muestra)
            .ThenInclude(x => x!.PuntoMuestreo)
            .Include(x => x.TipoEnsayo)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (ensayo is null)
        {
            return NotFound();
        }

        return Ok(ensayo);
    }

    [HttpPost]
    public async Task<ActionResult> Post(EnsayoRealizado ensayoRealizado)
    {
        var muestraExists = await _context.Muestras
            .AnyAsync(x => x.Id == ensayoRealizado.MuestraId);

        if (!muestraExists)
        {
            return BadRequest("La muestra asociada no existe.");
        }

        var tipoExists = await _context.TiposEnsayo
            .AnyAsync(x => x.Id == ensayoRealizado.TipoEnsayoId);

        if (!tipoExists)
        {
            return BadRequest("El tipo de ensayo asociado no existe.");
        }

        var exists = await _context.EnsayosRealizados
            .AnyAsync(x => x.MuestraId == ensayoRealizado.MuestraId &&
                           x.TipoEnsayoId == ensayoRealizado.TipoEnsayoId);

        if (exists)
        {
            return BadRequest("Ya existe un ensayo de este tipo para la muestra seleccionada.");
        }

        _context.Add(ensayoRealizado);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = ensayoRealizado.Id }, ensayoRealizado);
    }

    [HttpPut]
    public async Task<ActionResult> Put(EnsayoRealizado ensayoRealizado)
    {
        var currentEnsayo = await _context.EnsayosRealizados
            .FirstOrDefaultAsync(x => x.Id == ensayoRealizado.Id);

        if (currentEnsayo is null)
        {
            return NotFound();
        }

        var muestraExists = await _context.Muestras
            .AnyAsync(x => x.Id == ensayoRealizado.MuestraId);

        if (!muestraExists)
        {
            return BadRequest("La muestra asociada no existe.");
        }

        var tipoExists = await _context.TiposEnsayo
            .AnyAsync(x => x.Id == ensayoRealizado.TipoEnsayoId);

        if (!tipoExists)
        {
            return BadRequest("El tipo de ensayo asociado no existe.");
        }

        var exists = await _context.EnsayosRealizados
            .AnyAsync(x => x.MuestraId == ensayoRealizado.MuestraId &&
                           x.TipoEnsayoId == ensayoRealizado.TipoEnsayoId &&
                           x.Id != ensayoRealizado.Id);

        if (exists)
        {
            return BadRequest("Ya existe un ensayo de este tipo para la muestra seleccionada.");
        }

        _context.Entry(currentEnsayo).CurrentValues.SetValues(ensayoRealizado);
        await _context.SaveChangesAsync();
        return Ok(currentEnsayo);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        var affectedRows = await _context.EnsayosRealizados
            .Where(x => x.Id == id)
            .ExecuteDeleteAsync();

        if (affectedRows == 0)
        {
            return NotFound();
        }

        return NoContent();
    }
}
