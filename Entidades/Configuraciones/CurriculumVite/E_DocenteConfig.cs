using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entidades.Modelos.CurriculumVite;
using PlanCarrera = Entidades.Modelos.PlanesDeEstudio.Carreras.E_Carrera;

namespace Entidades.Configuraciones.CurriculumVite
{
    public class E_DocenteConfig : IEntityTypeConfiguration<E_Docente>
    {
        public void Configure(EntityTypeBuilder<E_Docente> builder)
        {
            builder.ToTable("Docentes", "CEF");
            builder.HasKey(e => e.IdDocente);
            
            // Propiedades requeridas
            builder.Property(e => e.NumeroEmpleado).IsRequired().HasMaxLength(10);
            builder.Property(e => e.NombreDocente).IsRequired().HasMaxLength(100);
            builder.Property(e => e.PaternoDocente).HasMaxLength(100);
            builder.Property(e => e.MaternoDocente).HasMaxLength(100);
            
            // Información personal
            builder.Property(e => e.Direccion).HasMaxLength(500);
            builder.Property(e => e.FechaNacimiento).HasColumnType("smalldatetime");
            
            // Contactos
            builder.Property(e => e.TelefonoCasa).HasMaxLength(40);
            builder.Property(e => e.TelefonoCelular).HasMaxLength(40);
            builder.Property(e => e.TelefonoTrabajo).HasMaxLength(40);
            builder.Property(e => e.Extension).HasMaxLength(14);
            builder.Property(e => e.EmailInstitucional).HasMaxLength(300);
            builder.Property(e => e.EmailAlterno).HasMaxLength(300);
            builder.Property(e => e.PaginaWeb).HasMaxLength(500);
            
            // Información adicional
            builder.Property(e => e.Casillero).HasMaxLength(10);
            builder.Property(e => e.CedulaProfesional).HasMaxLength(50);
            builder.Property(e => e.RFC).HasMaxLength(40);
            builder.Property(e => e.CURP).HasMaxLength(40);
            builder.Property(e => e.UniversidadLicenciatura).HasMaxLength(500);
            
            // Fechas importantes
            builder.Property(e => e.FechaIngreso).HasColumnType("smalldatetime");
            builder.Property(e => e.FechaInicialSNI).HasColumnType("smalldatetime");
            builder.Property(e => e.FechaFinalSNI).HasColumnType("smalldatetime");
            builder.Property(e => e.FechaInicialPRODEP).HasColumnType("smalldatetime");
            builder.Property(e => e.FechaFinalPRODEP).HasColumnType("smalldatetime");
            
            // Campos de texto largos
            builder.Property(e => e.URL_Drive).HasColumnType("nvarchar(max)");
            builder.Property(e => e.SembalzaDocente).HasColumnType("nvarchar(max)");
            builder.Property(e => e.Foto).HasColumnType("varbinary(max)");
            builder.Property(e => e.URLFoto).HasColumnType("nvarchar(max)");

            // Propiedades de compatibilidad ignoradas (no se mapean a la BD)
            builder.Ignore(e => e.ApellidoPaterno);
            builder.Ignore(e => e.ApellidoMaterno);
            builder.Ignore(e => e.Email);
            builder.Ignore(e => e.Telefono);
            builder.Ignore(e => e.Cedula);
            builder.Ignore(e => e.Especialidad);
            builder.Ignore(e => e.FechaRegistro);
            builder.Ignore(e => e.EstadoDocenteBool);
            
            // Relaciones con catálogos
            builder.HasOne(e => e.Sexo)
                .WithMany(s => s.Docentes)
                .HasForeignKey(e => e.IdSexo)
                .OnDelete(DeleteBehavior.Restrict);
                
            builder.HasOne(e => e.EstadoCivil)
                .WithMany(ec => ec.Docentes)
                .HasForeignKey(e => e.IdEstadoCivil)
                .OnDelete(DeleteBehavior.Restrict);
                
            builder.HasOne(e => e.Categoria)
                .WithMany(c => c.Docentes)
                .HasForeignKey(e => e.IdCategoria)
                .OnDelete(DeleteBehavior.Restrict);
                
            builder.HasOne(e => e.Nombramiento)
                .WithMany(n => n.Docentes)
                .HasForeignKey(e => e.IdNombramiento)
                .OnDelete(DeleteBehavior.Restrict);
                
            builder.HasOne(e => e.Escolaridad)
                .WithMany(es => es.Docentes)
                .HasForeignKey(e => e.IdEscolaridad)
                .OnDelete(DeleteBehavior.Restrict);
                
            builder.HasOne(e => e.NivelSNI)
                .WithMany(sni => sni.Docentes)
                .HasForeignKey(e => e.IdNivelSNI)
                .OnDelete(DeleteBehavior.Restrict);
                
            builder.HasOne(e => e.PRODEP)
                .WithMany(p => p.Docentes)
                .HasForeignKey(e => e.IdPRODEP)
                .OnDelete(DeleteBehavior.Restrict);
                
            builder.HasOne(e => e.Carrera)
                .WithMany()
                .HasForeignKey(e => e.IdCarrera)
                .OnDelete(DeleteBehavior.Restrict);
                
            builder.HasOne(e => e.CuerpoAcademico)
                .WithMany(ca => ca.Docentes)
                .HasForeignKey(e => e.IdCuerpoAcademico)
                .OnDelete(DeleteBehavior.SetNull);
                
            // Relaciones con entidades dependientes (colecciones)
            builder.HasMany(d => d.Contactos)
                .WithOne()
                .HasForeignKey(c => c.IdDocente)
                .OnDelete(DeleteBehavior.Cascade);
                
            builder.HasMany(d => d.Educaciones)
                .WithOne()
                .HasForeignKey(e => e.IdDocente)
                .OnDelete(DeleteBehavior.Cascade);
                
            builder.HasMany(d => d.Experiencias)
                .WithOne()
                .HasForeignKey(e => e.IdDocente)
                .OnDelete(DeleteBehavior.Cascade);
                
            builder.HasMany(d => d.Publicaciones)
                .WithOne()
                .HasForeignKey(p => p.IdDocente)
                .OnDelete(DeleteBehavior.Cascade);
                
            builder.HasMany(d => d.Proyectos)
                .WithOne()
                .HasForeignKey(p => p.IdDocente)
                .OnDelete(DeleteBehavior.Cascade);
                
            builder.HasMany(d => d.TesisDirigidas)
                .WithOne()
                .HasForeignKey(t => t.IdDocente)
                .OnDelete(DeleteBehavior.Cascade);
                
            builder.HasMany(d => d.Distinciones)
                .WithOne()
                .HasForeignKey(d => d.IdDocente)
                .OnDelete(DeleteBehavior.Cascade);
                
            builder.HasMany(d => d.Documentos)
                .WithOne()
                .HasForeignKey(doc => doc.IdDocente)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
} 