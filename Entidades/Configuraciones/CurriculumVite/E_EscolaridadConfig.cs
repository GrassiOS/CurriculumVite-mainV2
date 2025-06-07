using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entidades.Modelos.CurriculumVite;

namespace Entidades.Configuraciones.CurriculumVite
{
    public class E_EscolaridadConfig : IEntityTypeConfiguration<E_Escolaridad>
    {
        public void Configure(EntityTypeBuilder<E_Escolaridad> builder)
        {
            builder.ToTable("Escolaridad", "UTL");
            builder.HasKey(e => e.IdEscolaridad);
            builder.Property(e => e.ClaveEscolaridad).IsRequired().HasMaxLength(20);
            builder.Property(e => e.NombreEscolaridad).IsRequired().HasMaxLength(100);
        }
    }
} 