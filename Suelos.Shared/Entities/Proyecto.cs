using System.ComponentModel.DataAnnotations;

namespace Suelos.Shared.Entities;

public class Proyecto
{
    public int Id { get; set; }

    [Required]
    [MaxLength(150)]
    public string Nombre { get; set; } = null!;

    [Required]
    [MaxLength(150)]
    public string Cliente { get; set; } = null!;

    [MaxLength(200)]
    public string? Ubicacion { get; set; }

    [Required]
    [MaxLength(50)]
    public string Estado { get; set; } = "Activo";

    public ICollection<PuntoMuestreo> PuntosMuestreo { get; set; } = new List<PuntoMuestreo>();
}
