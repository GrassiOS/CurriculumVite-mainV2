using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entidades.Modelos.CurriculumVite;

namespace Entidades.Configuraciones.CurriculumVite
{
    public class E_TesisDirigidaConfig : IEntityTypeConfiguration<E_TesisDirigida>
    {
        public void Configure(EntityTypeBuilder<E_TesisDirigida> builder)
        {
            builder.ToTable("TesisDirigida", "CV");
            builder.HasKey(e => e.IdTesis);
            builder.Property(e => e.IdDocente).IsRequired();
            builder.Property(e => e.Autor).HasMaxLength(200);
            builder.Property(e => e.Titulo);
            builder.Property(e => e.Nivel).HasMaxLength(50);
            builder.Property(e => e.Universidad).HasMaxLength(200);
            builder.Property(e => e.Anio);
            
            // Configurar la relación con E_Docente
            builder.HasOne<E_Docente>()
                .WithMany(d => d.TesisDirigidas)
                .HasForeignKey(t => t.IdDocente)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
