using Entidades.Modelos.PlanesDeEstudio.Carreras;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entidades.Configuraciones.PlanesDeEstudio
{
  public class CarreraConfig : IEntityTypeConfiguration<E_Carrera>
  {
    public void Configure(EntityTypeBuilder<E_Carrera> builder)
    {
      // Configuración para el esquema CEF de la base de datos
      builder.ToTable("Carreras", "CEF");
      //Llave primaria y AutoIncremento
      builder.HasKey(c => c.IdCarrera);
      builder.Property(c => c.IdCarrera).ValueGeneratedOnAdd();

      // Configurar propiedades requeridas y longitudes
      builder.Property(c => c.ClaveCarrera).IsRequired().HasMaxLength(3);
      builder.HasIndex(c => c.ClaveCarrera).IsUnique().HasAnnotation("Relational:Name", "UK_ClaveCarrera");

      builder.Property(c => c.NombreCarrera).IsRequired().HasMaxLength(50);
      builder.HasIndex(c => c.NombreCarrera).IsUnique().HasAnnotation("Relational:Name", "UK_NombreCarrera");

      builder.Property(c => c.AliasCarrera).IsRequired().HasMaxLength(50);
      builder.HasIndex(c => c.AliasCarrera).IsUnique().HasAnnotation("Relational:Name", "UK_AliasCarrera"); ;

    }
  }
}
