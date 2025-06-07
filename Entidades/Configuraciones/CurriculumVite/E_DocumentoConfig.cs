using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using Entidades.Modelos.CurriculumVite;

namespace Entidades.Configuraciones.CurriculumVite
{
    public class E_DocumentoConfig : IEntityTypeConfiguration<E_Documento>
    {
        public void Configure(EntityTypeBuilder<E_Documento> builder)
        {
            builder.ToTable("Documento", "CV");
            builder.HasKey(e => e.IdDocumento);
            builder.Property(e => e.IdDocumento).HasColumnName("IdDocumento").ValueGeneratedOnAdd();
            builder.Property(e => e.IdDocente).HasColumnName("IdDocente").IsRequired();
            builder.Property(e => e.IdPublicacion).HasColumnName("IdPublicacion").IsRequired(false);
            builder.Property(e => e.IdDistincion).HasColumnName("IdDistincion").IsRequired(false);
            builder.Property(e => e.IdProyecto).HasColumnName("IdProyecto").IsRequired(false);
            builder.Property(e => e.IdTesis).HasColumnName("IdTesis").IsRequired(false);
            builder.Property(e => e.IdEducacion).HasColumnName("IdEducacion").IsRequired(false);
            builder.Property(e => e.Titulo).HasColumnName("titulo").HasMaxLength(510).IsRequired(false);
            builder.Property(e => e.Url).HasColumnName("url").IsRequired();
            builder.Property(e => e.Descripcion).HasColumnName("descripcion").IsRequired(false);
            builder.Property(e => e.FechaSubida).HasColumnName("fechaSubida").IsRequired();
        }
    }
}
