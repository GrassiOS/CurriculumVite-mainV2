using Entidades.Modelos.PlanesDeEstudio.PlanEstudios;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace Entidades.Configuraciones.PlanesDeEstudio
{
  public class PlanEstudiosConfig : IEntityTypeConfiguration<E_PlanEstudio>
  {
    public void Configure(EntityTypeBuilder<E_PlanEstudio> builder)
    {
      //Llave primaria y AutoIncremento
      builder.HasKey(pe => pe.IdPlanEstudio);
      builder.Property(pe => pe.IdPlanEstudio).ValueGeneratedOnAdd();
      builder.ToTable("PlanEstudios", "CEF");
      // Configurar propiedades requeridas y longitudes
      builder.Property(pe => pe.PlanEstudio).IsRequired().HasMaxLength(6);
      builder.HasIndex(pe => pe.PlanEstudio).IsUnique().HasAnnotation("Relational:Name", "UK_PlaEstudio");

      builder.Property(pe => pe.FechaCreacion).IsRequired();     
      builder.Property(pe => pe.TotalCreditos).IsRequired();

      builder.Property(pe => pe.PerfilDeIngreso).IsRequired().HasMaxLength(2048);
      builder.Property(pe => pe.PerfilDeEgreso).IsRequired().HasMaxLength(2048);
      builder.Property(pe => pe.CampoOcupacional).IsRequired().HasMaxLength(2048);
      builder.Property(pe => pe.Comentarios).HasMaxLength(2048);


      // Relación con Carrera
      builder.HasOne(pe => pe.Carrera)
             .WithMany(c => c.PlanesEstudio)
             .HasForeignKey(pe => pe.IdCarrera)
             .OnDelete(DeleteBehavior.Restrict);
    }
  }
}
