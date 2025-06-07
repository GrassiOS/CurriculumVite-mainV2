using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entidades.Modelos.CurriculumVite;

namespace Entidades.Configuraciones.CurriculumVite
{
    public class E_ProyectoConfig : IEntityTypeConfiguration<E_Proyecto>
    {
        public void Configure(EntityTypeBuilder<E_Proyecto> builder)
        {
            builder.ToTable("Proyecto", "CV");
            builder.HasKey(e => e.IdProyecto);
            
            // Mapeo de columnas según el schema de la BD
            builder.Property(e => e.IdProyecto).HasColumnName("IdProyecto");
            builder.Property(e => e.IdDocente).HasColumnName("IdDocente").IsRequired();
            builder.Property(e => e.Titulo).HasColumnName("titulo");
            builder.Property(e => e.Rol).HasColumnName("rol").HasMaxLength(200);
            builder.Property(e => e.Institucion).HasColumnName("institucion").HasMaxLength(400);
            builder.Property(e => e.Financiamiento).HasColumnName("financiamiento").HasMaxLength(400);
            builder.Property(e => e.PeriodoInicio).HasColumnName("periodoInicio");
            builder.Property(e => e.PeriodoFin).HasColumnName("periodoFin").IsRequired();
            
            // NO configurar relación aquí - ya está en E_DocenteConfig
        }
    }
}
