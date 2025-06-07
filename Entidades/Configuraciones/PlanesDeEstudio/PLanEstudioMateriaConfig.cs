using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Entidades.Modelos.PlanesDeEstudio.PlanEstudios;

namespace Entidades.Configuraciones.PlanesDeEstudio
{
  public class PlanEstudioMateriaConfig : IEntityTypeConfiguration<E_PlanEstudioMateria>
  {
    public void Configure(EntityTypeBuilder<E_PlanEstudioMateria> builder)
    {
      builder.ToTable("PlanEstudioMaterias", "CEF");
      // 1. Definir la clave primaria (PK)
      builder.HasKey(pem => pem.IdPlanEstudioMateria);
      builder.Property(m => m.IdPlanEstudioMateria).ValueGeneratedOnAdd();

      // Configuración de propiedades
      builder.Property(pem => pem.Semestre)
             .IsRequired();

      // Relaciones
      builder.HasOne(pem => pem.PlanEstudio)
             .WithMany(pe => pe.PlanEstudioMaterias)
             .HasForeignKey(pem => pem.IdPlanEstudio)
             .OnDelete(DeleteBehavior.Restrict);

      builder.HasOne(pem => pem.Materia)
             .WithMany(m => m.PlanEstudioMaterias)
             .HasForeignKey(pem => pem.IdMateria)
             .OnDelete(DeleteBehavior.Restrict);

      builder.HasOne(pem => pem.Etapa)
             .WithMany(e => e.PlanEstudioMaterias)
             .HasForeignKey(pem => pem.IdEtapa)
             .OnDelete(DeleteBehavior.Restrict);

      builder.HasOne(pem => pem.AreaConocimiento)
             .WithMany(ac => ac.PlanEstudioMaterias)
             .HasForeignKey(pem => pem.IdAreaConocimiento)
             .OnDelete(DeleteBehavior.Restrict);
    }
  }
}
