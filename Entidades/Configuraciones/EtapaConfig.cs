using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Entidades.Modelos.PlanesDeEstudio.Etapas;

namespace Entidades.Configuraciones
{
  public class EtapaConfig : IEntityTypeConfiguration<E_Etapa>
  {
    public void Configure(EntityTypeBuilder<E_Etapa> builder)
    {
      // Configuración para el esquema CEF de la base de datos
      builder.ToTable("Etapas", "CEF");
      //Llave primaria y AutoIncremento
      builder.HasKey(e => e.IdEtapa);
      builder.Property(e => e.IdEtapa).ValueGeneratedOnAdd();
          
      builder.Property(e => e.NombreEtapa).IsRequired().HasMaxLength(50);
      builder.HasIndex(e => e.NombreEtapa).IsUnique().HasAnnotation("Relational:Name", "UK_NombreEtapa");
    }
  }
}
