using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entidades.Modelos.CurriculumVite;

namespace Entidades.Configuraciones.CurriculumVite
{
    public class E_EstadoCivilConfig : IEntityTypeConfiguration<E_EstadoCivil>
    {
        public void Configure(EntityTypeBuilder<E_EstadoCivil> builder)
        {
            builder.ToTable("EstadoCivil", "UTL");
            builder.HasKey(e => e.IdEstadoCivil);
            builder.Property(e => e.EstadoCivil).IsRequired().HasMaxLength(100);
        }
    }
} 