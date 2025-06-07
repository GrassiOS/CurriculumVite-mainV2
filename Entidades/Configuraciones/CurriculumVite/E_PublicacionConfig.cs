using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entidades.Modelos.CurriculumVite;

namespace Entidades.Configuraciones.CurriculumVite
{
    public class E_PublicacionConfig : IEntityTypeConfiguration<E_Publicacion>
    {
        public void Configure(EntityTypeBuilder<E_Publicacion> builder)
        {
            builder.ToTable("Publicacion", "CV");
            builder.HasKey(e => e.IdPublicacion);
            builder.Property(e => e.IdDocente).IsRequired();
            builder.Property(e => e.Titulo);
            builder.Property(e => e.TipoPublicacion).HasMaxLength(100);
            builder.Property(e => e.Autores);
            builder.Property(e => e.Fuente).HasMaxLength(300);
            builder.Property(e => e.Anio);
            builder.Property(e => e.Enlace).IsRequired(false);
            
            // NO configurar relación aquí - ya está en E_DocenteConfig
        }
    }
}
