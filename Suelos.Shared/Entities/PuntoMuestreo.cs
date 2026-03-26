using System.ComponentModel.DataAnnotations;

namespace Suelos.Shared.Entities;

public class PuntoMuestreo
{
    public int Id { get; set; }
    public int ProyectoId { get; set; }

    [Required]
    [MaxLength(50)]
    public string Codigo { get; set; } = null!;

    [MaxLength(100)]
    public string? Sector { get; set; }

    [MaxLength(200)]
    public string? Descripcion { get; set; }

    [MaxLength(100)]
    public string? CoordenadaX { get; set; }

    [MaxLength(100)]
    public string? CoordenadaY { get; set; }

    public Proyecto? Proyecto { get; set; }
    public ICollection<Muestra> Muestras { get; set; } = new List<Muestra>();
}
