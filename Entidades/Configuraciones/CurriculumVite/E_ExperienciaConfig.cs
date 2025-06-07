using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entidades.Modelos.CurriculumVite;

namespace Entidades.Configuraciones.CurriculumVite
{
    public class E_ExperienciaConfig : IEntityTypeConfiguration<E_Experiencia>
    {
        public void Configure(EntityTypeBuilder<E_Experiencia> builder)
        {
            builder.ToTable("Experiencia", "CV");
            builder.HasKey(e => e.IdExperiencia);
            
            // Mapeo de columnas según el schema de la BD
            builder.Property(e => e.IdExperiencia).HasColumnName("IdExperiencia");
            builder.Property(e => e.IdDocente).HasColumnName("IdDocente").IsRequired();
            builder.Property(e => e.Puesto).HasColumnName("puesto").HasMaxLength(400);
            builder.Property(e => e.Institucion).HasColumnName("institucion").HasMaxLength(400);
            builder.Property(e => e.Descripcion).HasColumnName("descripcion");
            builder.Property(e => e.FechaInicio).HasColumnName("fechaInicio");
            builder.Property(e => e.FechaFin).HasColumnName("fechaFin");
            
            // NO configurar relación aquí - ya está en E_DocenteConfig
        }
    }
}
