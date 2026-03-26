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
public class ProyectosController(AppDbContext context) : ControllerBase
{
    private readonly AppDbContext _context = context;

    [HttpGet]
    public async Task<ActionResult> Get()
    {
        var proyectos = await _context.Proyectos
            .OrderBy(x => x.Nombre)
            .ToListAsync();

        return Ok(proyectos);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult> Get(int id)
    {
        var proyecto = await _context.Proyectos
            .FirstOrDefaultAsync(x => x.Id == id);

        if (proyecto is null)
        {
            return NotFound();
        }

        return Ok(proyecto);
    }

    [HttpPost]
    public async Task<ActionResult> Post(Proyecto proyecto)
    {
        var exists = await _context.Proyectos
            .AnyAsync(x => x.Nombre == proyecto.Nombre && x.Cliente == proyecto.Cliente);

        if (exists)
        {
            return BadRequest("Ya existe un proyecto con el mismo nombre para ese cliente.");
        }

        _context.Add(proyecto);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = proyecto.Id }, proyecto);
    }

    [HttpPut]
    public async Task<ActionResult> Put(Proyecto proyecto)
    {
        var currentProyecto = await _context.Proyectos
            .FirstOrDefaultAsync(x => x.Id == proyecto.Id);

        if (currentProyecto is null)
        {
            return NotFound();
        }

        var exists = await _context.Proyectos
            .AnyAsync(x => x.Nombre == proyecto.Nombre &&
                           x.Cliente == proyecto.Cliente &&
                           x.Id != proyecto.Id);

        if (exists)
        {
            return BadRequest("Ya existe un proyecto con el mismo nombre para ese cliente.");
        }

        _context.Entry(currentProyecto).CurrentValues.SetValues(proyecto);
        await _context.SaveChangesAsync();
        return Ok(currentProyecto);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        var affectedRows = await _context.Proyectos
            .Where(x => x.Id == id)
            .ExecuteDeleteAsync();

        if (affectedRows == 0)
        {
            return NotFound();
        }

        return NoContent();
    }
}
