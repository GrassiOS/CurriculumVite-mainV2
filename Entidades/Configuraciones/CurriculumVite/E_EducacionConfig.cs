using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entidades.Modelos.CurriculumVite;

namespace Entidades.Configuraciones.CurriculumVite
{
    public class E_EducacionConfig : IEntityTypeConfiguration<E_Educacion>
    {
        public void Configure(EntityTypeBuilder<E_Educacion> builder)
        {
            builder.ToTable("Educacion", "CV");
            builder.HasKey(e => e.IdEducacion);
            
            // Mapeo de columnas según el schema de la BD
            builder.Property(e => e.IdEducacion).HasColumnName("IdEducacion");
            builder.Property(e => e.IdDocente).HasColumnName("IdDocente").IsRequired();
            builder.Property(e => e.Nivel).HasColumnName("nivel").HasMaxLength(100).IsRequired();
            builder.Property(e => e.Titulo).HasColumnName("titulo").HasMaxLength(400);
            builder.Property(e => e.Institucion).HasColumnName("institucion").HasMaxLength(400);
            builder.Property(e => e.Especialidad).HasColumnName("especialidad").HasMaxLength(400);
            builder.Property(e => e.Pais).HasColumnName("pais").HasMaxLength(200);
            builder.Property(e => e.AnioInicio).HasColumnName("anioInicio");
            builder.Property(e => e.AnioFin).HasColumnName("anioFin");
            
            // NO configurar relación aquí - ya está en E_DocenteConfig
        }
    }
}
