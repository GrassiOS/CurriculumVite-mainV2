using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Entidades.Modelos.PlanesDeEstudio.Materias;

namespace Entidades.Configuraciones.PlanesDeEstudio
{
  public class MateriaConfig : IEntityTypeConfiguration<E_Materia>
  {
    public void Configure(EntityTypeBuilder<E_Materia> builder)
    {
      builder.ToTable("Materias", "CEF");
      //Llave primaria y AutoIncremento
      builder.HasKey(m => m.IdMateria);
      builder.Property(m => m.IdMateria).ValueGeneratedOnAdd();

      // Configurar propiedades requeridas y longitudes
      builder.Property(m => m.ClaveMateria).IsRequired().HasMaxLength(6);
      builder.HasIndex(m => m.ClaveMateria).IsUnique().HasAnnotation("Relational:Name", "UK_ClaveCarrera"); ;
      builder.Property(m => m.NombreMateria).IsRequired().HasMaxLength(100);

      builder.Property(m => m.HC).IsRequired();
      builder.Property(m => m.HL).IsRequired();
      builder.Property(m => m.HT).IsRequired();
      builder.Property(m => m.HPC).IsRequired();
      builder.Property(m => m.HCL).IsRequired();
      builder.Property(m => m.HE).IsRequired();
      builder.Property(m => m.CR).IsRequired();

      builder.Property(m => m.PropositoGeneral).IsRequired().HasMaxLength(2048);
      builder.Property(m => m.Competencia).IsRequired().HasMaxLength(2048);
      builder.Property(m => m.Evidencia).IsRequired().HasMaxLength(2048);
      builder.Property(m => m.Metodologia).IsRequired().HasMaxLength(2048);
      builder.Property(m => m.Criterios).IsRequired().HasMaxLength(2048);
      builder.Property(m => m.BibliografiaBasica).IsRequired().HasMaxLength(2048);
      builder.Property(m => m.BibliografiaComplementaria).IsRequired().HasMaxLength(2048);
      builder.Property(m => m.PerfilDocente).IsRequired().HasMaxLength(2048);
      builder.Property(pe => pe.PathPUA).HasMaxLength(256);
    }
  }
}
