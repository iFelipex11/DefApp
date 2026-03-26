using System.ComponentModel.DataAnnotations;

namespace Suelos.Shared.Entities;

public class EnsayoRealizado
{
    public int Id { get; set; }
    public int MuestraId { get; set; }
    public int TipoEnsayoId { get; set; }
    public DateTime FechaAsignacion { get; set; } = DateTime.Today;
    public DateTime FechaEjecucion { get; set; } = DateTime.Today;
    public DateTime? FechaValidacion { get; set; }

    [MaxLength(100)]
    public string? Responsable { get; set; }

    [Required]
    [MaxLength(50)]
    public string Estado { get; set; } = "Pendiente";

    public Muestra? Muestra { get; set; }
    public TipoEnsayo? TipoEnsayo { get; set; }
    public ICollection<ResultadoParametro> ResultadosParametro { get; set; } = new List<ResultadoParametro>();
}
