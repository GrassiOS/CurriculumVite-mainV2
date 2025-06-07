using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entidades.Modelos.CurriculumVite;

namespace Entidades.Configuraciones.CurriculumVite
{
    public class E_PRODEPConfig : IEntityTypeConfiguration<E_PRODEP>
    {
        public void Configure(EntityTypeBuilder<E_PRODEP> builder)
        {
            builder.ToTable("PRODEP", "UTL");
            builder.HasKey(e => e.IdPRODEP);
            builder.Property(e => e.TienePRODEP).IsRequired().HasMaxLength(100);
        }
    }
} 