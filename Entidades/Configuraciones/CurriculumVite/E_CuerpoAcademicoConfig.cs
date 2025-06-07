using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entidades.Modelos.CurriculumVite;

namespace Entidades.Configuraciones.CurriculumVite
{
    public class E_CuerpoAcademicoConfig : IEntityTypeConfiguration<E_CuerpoAcademico>
    {
        public void Configure(EntityTypeBuilder<E_CuerpoAcademico> builder)
        {
            builder.ToTable("CuerpoAcademico", "CV");
            builder.HasKey(e => e.CuerpoAcademicoId);
            builder.Property(e => e.Nombre).IsRequired().HasMaxLength(300);
            builder.Property(e => e.Descripcion).HasColumnType("text");
        }
    }
} 