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
public class ResultadosController(AppDbContext context) : ControllerBase
{
    private readonly AppDbContext _context = context;

    [HttpGet]
    public async Task<ActionResult> Get()
    {
        var resultados = await _context.ResultadosParametro
            .Include(x => x.EnsayoRealizado)
            .ThenInclude(x => x!.Muestra)
            .Include(x => x.ParametroEnsayo)
            .ThenInclude(x => x!.TipoEnsayo)
            .OrderBy(x => x.Id)
            .ToListAsync();

        return Ok(resultados);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult> Get(int id)
    {
        var resultado = await _context.ResultadosParametro
            .Include(x => x.EnsayoRealizado)
            .ThenInclude(x => x!.Muestra)
            .Include(x => x.ParametroEnsayo)
            .ThenInclude(x => x!.TipoEnsayo)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (resultado is null)
        {
            return NotFound();
        }

        return Ok(resultado);
    }

    [HttpPost]
    public async Task<ActionResult> Post(ResultadoParametro resultadoParametro)
    {
        var ensayoExists = await _context.EnsayosRealizados
            .AnyAsync(x => x.Id == resultadoParametro.EnsayoRealizadoId);

        if (!ensayoExists)
        {
            return BadRequest("El ensayo realizado asociado no existe.");
        }

        var parametroExists = await _context.ParametrosEnsayo
            .AnyAsync(x => x.Id == resultadoParametro.ParametroEnsayoId);

        if (!parametroExists)
        {
            return BadRequest("El parametro de ensayo asociado no existe.");
        }

        var exists = await _context.ResultadosParametro
            .AnyAsync(x => x.EnsayoRealizadoId == resultadoParametro.EnsayoRealizadoId &&
                           x.ParametroEnsayoId == resultadoParametro.ParametroEnsayoId);

        if (exists)
        {
            return BadRequest("Ya existe un resultado para ese parametro dentro del ensayo.");
        }

        _context.Add(resultadoParametro);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = resultadoParametro.Id }, resultadoParametro);
    }

    [HttpPut]
    public async Task<ActionResult> Put(ResultadoParametro resultadoParametro)
    {
        var currentResultado = await _context.ResultadosParametro
            .FirstOrDefaultAsync(x => x.Id == resultadoParametro.Id);

        if (currentResultado is null)
        {
            return NotFound();
        }

        var ensayoExists = await _context.EnsayosRealizados
            .AnyAsync(x => x.Id == resultadoParametro.EnsayoRealizadoId);

        if (!ensayoExists)
        {
            return BadRequest("El ensayo realizado asociado no existe.");
        }

        var parametroExists = await _context.ParametrosEnsayo
            .AnyAsync(x => x.Id == resultadoParametro.ParametroEnsayoId);

        if (!parametroExists)
        {
            return BadRequest("El parametro de ensayo asociado no existe.");
        }

        var exists = await _context.ResultadosParametro
            .AnyAsync(x => x.EnsayoRealizadoId == resultadoParametro.EnsayoRealizadoId &&
                           x.ParametroEnsayoId == resultadoParametro.ParametroEnsayoId &&
                           x.Id != resultadoParametro.Id);

        if (exists)
        {
            return BadRequest("Ya existe un resultado para ese parametro dentro del ensayo.");
        }

        _context.Entry(currentResultado).CurrentValues.SetValues(resultadoParametro);
        await _context.SaveChangesAsync();
        return Ok(currentResultado);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        var affectedRows = await _context.ResultadosParametro
            .Where(x => x.Id == id)
            .ExecuteDeleteAsync();

        if (affectedRows == 0)
        {
            return NotFound();
        }

        return NoContent();
    }
}
