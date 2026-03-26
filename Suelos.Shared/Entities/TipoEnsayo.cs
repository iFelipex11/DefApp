using System.ComponentModel.DataAnnotations;

namespace Suelos.Shared.Entities;

public class TipoEnsayo
{
    public int Id { get; set; }

    [Required]
    [MaxLength(20)]
    public string Codigo { get; set; } = null!;

    [Required]
    [MaxLength(100)]
    public string Nombre { get; set; } = null!;

    [MaxLength(300)]
    public string? Descripcion { get; set; }

    public bool Activo { get; set; } = true;

    public ICollection<ParametroEnsayo> ParametrosEnsayo { get; set; } = new List<ParametroEnsayo>();
    public ICollection<EnsayoRealizado> EnsayosRealizados { get; set; } = new List<EnsayoRealizado>();
}
