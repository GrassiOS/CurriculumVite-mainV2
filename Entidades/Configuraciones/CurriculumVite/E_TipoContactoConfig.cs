using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entidades.Modelos.CurriculumVite;

namespace Entidades.Configuraciones.CurriculumVite
{
    public class E_TipoContactoConfig : IEntityTypeConfiguration<E_TipoContacto>
    {
        public void Configure(EntityTypeBuilder<E_TipoContacto> builder)
        {
            builder.ToTable("TipoContacto", "CV");
            builder.HasKey(e => e.TipoContactoId);
            builder.Property(e => e.Nombre).HasMaxLength(200).IsRequired();
        }
    }
}
