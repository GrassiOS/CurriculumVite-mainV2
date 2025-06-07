using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entidades.Modelos.CurriculumVite;

namespace Entidades.Configuraciones.CurriculumVite
{
    public class E_SNIConfig : IEntityTypeConfiguration<E_SNI>
    {
        public void Configure(EntityTypeBuilder<E_SNI> builder)
        {
            builder.ToTable("SNI", "UTL");
            builder.HasKey(e => e.IdNivelSNI);
            builder.Property(e => e.NivelSNI).IsRequired().HasMaxLength(100);
        }
    }
} 