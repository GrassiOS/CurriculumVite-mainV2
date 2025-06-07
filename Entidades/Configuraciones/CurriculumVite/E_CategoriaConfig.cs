using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entidades.Modelos.CurriculumVite;

namespace Entidades.Configuraciones.CurriculumVite
{
    public class E_CategoriaConfig : IEntityTypeConfiguration<E_Categoria>
    {
        public void Configure(EntityTypeBuilder<E_Categoria> builder)
        {
            builder.ToTable("Categorias", "UTL");
            builder.HasKey(e => e.IdCategoria);
            builder.Property(e => e.ClaveCategoria).IsRequired().HasMaxLength(10);
            builder.Property(e => e.NombreCategoria).IsRequired().HasMaxLength(300);
        }
    }
} 