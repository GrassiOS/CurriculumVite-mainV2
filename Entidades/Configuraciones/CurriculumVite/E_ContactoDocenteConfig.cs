using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entidades.Modelos.CurriculumVite;

namespace Entidades.Configuraciones.CurriculumVite
{
    public class E_ContactoDocenteConfig : IEntityTypeConfiguration<E_ContactoDocente>
    {
        public void Configure(EntityTypeBuilder<E_ContactoDocente> builder)
        {
            builder.ToTable("ContactoProfesor", "CV");
            builder.HasKey(e => e.IdContacto);
            builder.Property(e => e.IdDocente).IsRequired();
            builder.Property(e => e.IdTpoContacto).IsRequired();
            builder.Property(e => e.Url).IsRequired().HasColumnType("nvarchar(max)");
            
            // Relaciones
            builder.HasOne(e => e.Docente)
                .WithMany(d => d.Contactos)
                .HasForeignKey(e => e.IdDocente)
                .OnDelete(DeleteBehavior.Cascade);
                
            builder.HasOne(e => e.TipoContacto)
                .WithMany()
                .HasForeignKey(e => e.IdTpoContacto)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
