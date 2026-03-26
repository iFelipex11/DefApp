using System.ComponentModel.DataAnnotations;

namespace Suelos.Shared.Entities;

public class ResultadoParametro
{
    public int Id { get; set; }
    public int EnsayoRealizadoId { get; set; }
    public int ParametroEnsayoId { get; set; }
    public decimal? Valor { get; set; }

    [MaxLength(300)]
    public string? Observacion { get; set; }

    public bool CumpleRango { get; set; }

    [MaxLength(300)]
    public string? ObservacionTecnica { get; set; }

    public EnsayoRealizado? EnsayoRealizado { get; set; }
    public ParametroEnsayo? ParametroEnsayo { get; set; }
}
