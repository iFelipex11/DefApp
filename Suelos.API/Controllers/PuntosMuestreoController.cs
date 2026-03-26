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
public class PuntosMuestreoController(AppDbContext context) : ControllerBase
{
    private readonly AppDbContext _context = context;

    [HttpGet]
    public async Task<ActionResult> Get()
    {
        var puntos = await _context.PuntosMuestreo
            .Include(x => x.Proyecto)
            .OrderBy(x => x.Codigo)
            .ToListAsync();

        return Ok(puntos);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult> Get(int id)
    {
        var punto = await _context.PuntosMuestreo
            .Include(x => x.Proyecto)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (punto is null)
        {
            return NotFound();
        }

        return Ok(punto);
    }

    [HttpPost]
    public async Task<ActionResult> Post(PuntoMuestreo puntoMuestreo)
    {
        var proyectoExists = await _context.Proyectos
            .AnyAsync(x => x.Id == puntoMuestreo.ProyectoId);

        if (!proyectoExists)
        {
            return BadRequest("El proyecto asociado no existe.");
        }

        var exists = await _context.PuntosMuestreo
            .AnyAsync(x => x.ProyectoId == puntoMuestreo.ProyectoId &&
                           x.Codigo == puntoMuestreo.Codigo);

        if (exists)
        {
            return BadRequest("Ya existe un punto de muestreo con ese codigo en el proyecto.");
        }

        _context.Add(puntoMuestreo);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = puntoMuestreo.Id }, puntoMuestreo);
    }

    [HttpPut]
    public async Task<ActionResult> Put(PuntoMuestreo puntoMuestreo)
    {
        var currentPunto = await _context.PuntosMuestreo
            .FirstOrDefaultAsync(x => x.Id == puntoMuestreo.Id);

        if (currentPunto is null)
        {
            return NotFound();
        }

        var proyectoExists = await _context.Proyectos
            .AnyAsync(x => x.Id == puntoMuestreo.ProyectoId);

        if (!proyectoExists)
        {
            return BadRequest("El proyecto asociado no existe.");
        }

        var exists = await _context.PuntosMuestreo
            .AnyAsync(x => x.ProyectoId == puntoMuestreo.ProyectoId &&
                           x.Codigo == puntoMuestreo.Codigo &&
                           x.Id != puntoMuestreo.Id);

        if (exists)
        {
            return BadRequest("Ya existe un punto de muestreo con ese codigo en el proyecto.");
        }

        _context.Entry(currentPunto).CurrentValues.SetValues(puntoMuestreo);
        await _context.SaveChangesAsync();
        return Ok(currentPunto);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        var affectedRows = await _context.PuntosMuestreo
            .Where(x => x.Id == id)
            .ExecuteDeleteAsync();

        if (affectedRows == 0)
        {
            return NotFound();
        }

        return NoContent();
    }
}
