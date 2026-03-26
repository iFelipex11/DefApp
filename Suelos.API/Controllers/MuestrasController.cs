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
public class MuestrasController(AppDbContext context) : ControllerBase
{
    private readonly AppDbContext _context = context;

    [HttpGet]
    public async Task<ActionResult> Get()
    {
        var muestras = await _context.Muestras
            .Include(x => x.PuntoMuestreo)
            .ThenInclude(x => x!.Proyecto)
            .OrderBy(x => x.CodigoMuestra)
            .ToListAsync();

        return Ok(muestras);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult> Get(int id)
    {
        var muestra = await _context.Muestras
            .Include(x => x.PuntoMuestreo)
            .ThenInclude(x => x!.Proyecto)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (muestra is null)
        {
            return NotFound();
        }

        return Ok(muestra);
    }

    [HttpPost]
    public async Task<ActionResult> Post(Muestra muestra)
    {
        var puntoExists = await _context.PuntosMuestreo
            .AnyAsync(x => x.Id == muestra.PuntoMuestreoId);

        if (!puntoExists)
        {
            return BadRequest("El punto de muestreo asociado no existe.");
        }

        var exists = await _context.Muestras
            .AnyAsync(x => x.CodigoMuestra == muestra.CodigoMuestra);

        if (exists)
        {
            return BadRequest("Ya existe una muestra con ese codigo.");
        }

        _context.Add(muestra);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = muestra.Id }, muestra);
    }

    [HttpPut]
    public async Task<ActionResult> Put(Muestra muestra)
    {
        var currentMuestra = await _context.Muestras
            .FirstOrDefaultAsync(x => x.Id == muestra.Id);

        if (currentMuestra is null)
        {
            return NotFound();
        }

        var puntoExists = await _context.PuntosMuestreo
            .AnyAsync(x => x.Id == muestra.PuntoMuestreoId);

        if (!puntoExists)
        {
            return BadRequest("El punto de muestreo asociado no existe.");
        }

        var exists = await _context.Muestras
            .AnyAsync(x => x.CodigoMuestra == muestra.CodigoMuestra &&
                           x.Id != muestra.Id);

        if (exists)
        {
            return BadRequest("Ya existe una muestra con ese codigo.");
        }

        _context.Entry(currentMuestra).CurrentValues.SetValues(muestra);
        await _context.SaveChangesAsync();
        return Ok(currentMuestra);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        var affectedRows = await _context.Muestras
            .Where(x => x.Id == id)
            .ExecuteDeleteAsync();

        if (affectedRows == 0)
        {
            return NotFound();
        }

        return NoContent();
    }
}
