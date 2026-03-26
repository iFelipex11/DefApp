using System.ComponentModel.DataAnnotations;

namespace Suelos.Shared.Entities;

public class Muestra
{
    public int Id { get; set; }
    public int PuntoMuestreoId { get; set; }

    [Required]
    [MaxLength(50)]
    public string CodigoMuestra { get; set; } = null!;

    public DateTime FechaRecepcion { get; set; } = DateTime.Today;
    public DateTime FechaMuestreo { get; set; } = DateTime.Today;
    public decimal? ProfundidadInicial { get; set; }
    public decimal? ProfundidadFinal { get; set; }

    [MaxLength(50)]
    public string? TipoMuestra { get; set; }

    [Required]
    [MaxLength(50)]
    public string EstadoMuestra { get; set; } = "Registrada";

    [MaxLength(300)]
    public string? Observaciones { get; set; }

    public PuntoMuestreo? PuntoMuestreo { get; set; }
    public ICollection<EnsayoRealizado> EnsayosRealizados { get; set; } = new List<EnsayoRealizado>();
}
