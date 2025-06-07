using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entidades.Modelos.CurriculumVite;

namespace Entidades.Configuraciones.CurriculumVite
{
    public class E_NombramientoConfig : IEntityTypeConfiguration<E_Nombramiento>
    {
        public void Configure(EntityTypeBuilder<E_Nombramiento> builder)
        {
            builder.ToTable("Nombramiento", "UTL");
            builder.HasKey(e => e.IdNombramiento);
            builder.Property(e => e.Nombramiento).IsRequired().HasMaxLength(100);
        }
    }
} 