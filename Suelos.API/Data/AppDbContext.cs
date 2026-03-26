using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Suelos.Shared.Entities;

namespace Suelos.API.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<User>(options)
{
    public DbSet<Proyecto> Proyectos => Set<Proyecto>();
    public DbSet<PuntoMuestreo> PuntosMuestreo => Set<PuntoMuestreo>();
    public DbSet<Muestra> Muestras => Set<Muestra>();
    public DbSet<TipoEnsayo> TiposEnsayo => Set<TipoEnsayo>();
    public DbSet<ParametroEnsayo> ParametrosEnsayo => Set<ParametroEnsayo>();
    public DbSet<EnsayoRealizado> EnsayosRealizados => Set<EnsayoRealizado>();
    public DbSet<ResultadoParametro> ResultadosParametro => Set<ResultadoParametro>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<PuntoMuestreo>().HasIndex(x => new { x.ProyectoId, x.Codigo }).IsUnique();
        modelBuilder.Entity<Muestra>().HasIndex(x => x.CodigoMuestra).IsUnique();
        modelBuilder.Entity<TipoEnsayo>().HasIndex(x => x.Codigo).IsUnique();
        modelBuilder.Entity<ParametroEnsayo>().HasIndex(x => new { x.TipoEnsayoId, x.Nombre }).IsUnique();
        modelBuilder.Entity<EnsayoRealizado>().HasIndex(x => new { x.MuestraId, x.TipoEnsayoId }).IsUnique();
        modelBuilder.Entity<ResultadoParametro>().HasIndex(x => new { x.EnsayoRealizadoId, x.ParametroEnsayoId }).IsUnique();

        modelBuilder.Entity<PuntoMuestreo>()
            .HasOne(x => x.Proyecto)
            .WithMany(x => x.PuntosMuestreo)
            .HasForeignKey(x => x.ProyectoId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Muestra>()
            .HasOne(x => x.PuntoMuestreo)
            .WithMany(x => x.Muestras)
            .HasForeignKey(x => x.PuntoMuestreoId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ParametroEnsayo>()
            .HasOne(x => x.TipoEnsayo)
            .WithMany(x => x.ParametrosEnsayo)
            .HasForeignKey(x => x.TipoEnsayoId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<EnsayoRealizado>()
            .HasOne(x => x.Muestra)
            .WithMany(x => x.EnsayosRealizados)
            .HasForeignKey(x => x.MuestraId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<EnsayoRealizado>()
            .HasOne(x => x.TipoEnsayo)
            .WithMany(x => x.EnsayosRealizados)
            .HasForeignKey(x => x.TipoEnsayoId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ResultadoParametro>()
            .HasOne(x => x.EnsayoRealizado)
            .WithMany(x => x.ResultadosParametro)
            .HasForeignKey(x => x.EnsayoRealizadoId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ResultadoParametro>()
            .HasOne(x => x.ParametroEnsayo)
            .WithMany(x => x.ResultadosParametro)
            .HasForeignKey(x => x.ParametroEnsayoId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
