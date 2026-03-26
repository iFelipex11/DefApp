using System.ComponentModel.DataAnnotations;

namespace Suelos.Shared.Entities;

public class ParametroEnsayo
{
    public int Id { get; set; }
    public int TipoEnsayoId { get; set; }

    [Required]
    [MaxLength(100)]
    public string Nombre { get; set; } = null!;

    [MaxLength(50)]
    public string? Unidad { get; set; }

    public bool Requerido { get; set; } = true;
    public bool EsCalculado { get; set; }
    public decimal? MinReferencial { get; set; }
    public decimal? MaxReferencial { get; set; }

    public TipoEnsayo? TipoEnsayo { get; set; }
    public ICollection<ResultadoParametro> ResultadosParametro { get; set; } = new List<ResultadoParametro>();
}
