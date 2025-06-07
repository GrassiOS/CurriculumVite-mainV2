using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entidades.Modelos.CurriculumVite;

namespace Entidades.Configuraciones.CurriculumVite
{
    public class E_SexoConfig : IEntityTypeConfiguration<E_Sexo>
    {
        public void Configure(EntityTypeBuilder<E_Sexo> builder)
        {
            builder.ToTable("Sexo", "UTL");
            builder.HasKey(e => e.IdSexo);
            builder.Property(e => e.Sexo).IsRequired().HasMaxLength(100);
        }
    }
} 