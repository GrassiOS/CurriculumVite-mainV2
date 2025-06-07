using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Entidades.Modelos.PlanesDeEstudio.AreasDeConocimiento;

namespace Entidades.Configuraciones
{
  public class AreaConocimientoConfig : IEntityTypeConfiguration<E_AreaConocimiento>
  {
    public void Configure(EntityTypeBuilder<E_AreaConocimiento> builder)
    {
      //Llave primaria y AutoIncremento
      builder.HasKey(ac => ac.IdAreaConocimiento);
      builder.Property(ac => ac.IdAreaConocimiento).ValueGeneratedOnAdd();

      // Configurar propiedades requeridas y longitudes
      builder.Property(ac => ac.ClaveAreaConocimiento).IsRequired().HasMaxLength(3);
      builder.HasIndex(ac => ac.ClaveAreaConocimiento).IsUnique().HasAnnotation("Relational:Name", "UK_ClaveAreaConocimiento");

      builder.Property(ac => ac.DescripcionAreaConocimiento).IsRequired().HasMaxLength(100);     
    }
  }
}
