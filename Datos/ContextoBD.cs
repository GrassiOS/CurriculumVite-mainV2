using Entidades.Configuraciones;
using Entidades.Configuraciones.PlanesDeEstudio;
using Entidades.Modelos.PlanesDeEstudio.AreasDeConocimiento;
using Entidades.Modelos.PlanesDeEstudio.Carreras;
using Entidades.Modelos.PlanesDeEstudio.Etapas;
using Entidades.Modelos.PlanesDeEstudio.Materias;
using Entidades.Modelos.PlanesDeEstudio.PlanEstudios;
using Entidades.Configuraciones.CurriculumVite;
using Microsoft.EntityFrameworkCore;

// Usar alias para evitar conflictos de nombres
using PlanCarrera = Entidades.Modelos.PlanesDeEstudio.Carreras.E_Carrera;
using CVDocente = Entidades.Modelos.CurriculumVite.E_Docente;
using CVDistincion = Entidades.Modelos.CurriculumVite.E_Distincion;
using CVExperiencia = Entidades.Modelos.CurriculumVite.E_Experiencia;
using CVTipoContacto = Entidades.Modelos.CurriculumVite.E_TipoContacto;
using CVContactoDocente = Entidades.Modelos.CurriculumVite.E_ContactoDocente;
using CVProyecto = Entidades.Modelos.CurriculumVite.E_Proyecto;
using CVEducacion = Entidades.Modelos.CurriculumVite.E_Educacion;
using CVPublicacion = Entidades.Modelos.CurriculumVite.E_Publicacion;
using CVTesisDirigida = Entidades.Modelos.CurriculumVite.E_TesisDirigida;
using CVDocumento = Entidades.Modelos.CurriculumVite.E_Documento;
using CVSexo = Entidades.Modelos.CurriculumVite.E_Sexo;
using CVEstadoCivil = Entidades.Modelos.CurriculumVite.E_EstadoCivil;
using CVCategoria = Entidades.Modelos.CurriculumVite.E_Categoria;
using CVNombramiento = Entidades.Modelos.CurriculumVite.E_Nombramiento;
using CVEscolaridad = Entidades.Modelos.CurriculumVite.E_Escolaridad;
using CVSNI = Entidades.Modelos.CurriculumVite.E_SNI;
using CVPRODEP = Entidades.Modelos.CurriculumVite.E_PRODEP;
using CVCuerpoAcademico = Entidades.Modelos.CurriculumVite.E_CuerpoAcademico;

namespace Datos
{
    public class ContextoBD : DbContext
    {
        public ContextoBD(DbContextOptions<ContextoBD> options)
            : base(options)
        {
        }

        // Planes de Estudio DbSets
        public DbSet<PlanCarrera> CarrerasPlanEstudio { get; set; }
        public DbSet<E_PlanEstudio> PlanEstudios { get; set; }
        public DbSet<E_PlanEstudioMateria> PlanEstudioMaterias { get; set; }
        public DbSet<E_Materia> Materias { get; set; }
        public DbSet<E_Etapa> Etapas { get; set; }
        public DbSet<E_AreaConocimiento> AreasConocimiento { get; set; }

        // CurriculumVite DbSets - Entidades principales
        public DbSet<CVDocente> Docentes { get; set; }
        
        // Catálogos
        public DbSet<CVSexo> Sexos { get; set; }
        public DbSet<CVEstadoCivil> EstadosCiviles { get; set; }
        public DbSet<CVCategoria> Categorias { get; set; }
        public DbSet<CVNombramiento> Nombramientos { get; set; }
        public DbSet<CVEscolaridad> Escolaridades { get; set; }
        public DbSet<CVSNI> NivelesSNI { get; set; }
        public DbSet<CVPRODEP> PRODEP { get; set; }
        public DbSet<CVCuerpoAcademico> CuerposAcademicos { get; set; }
        
        // CV Entidades
        public DbSet<CVDistincion> Distinciones { get; set; }
        public DbSet<CVExperiencia> Experiencias { get; set; }
        public DbSet<CVTipoContacto> TipoContactos { get; set; }
        public DbSet<CVContactoDocente> ContactoDocentes { get; set; }
        public DbSet<CVProyecto> Proyectos { get; set; }
        public DbSet<CVEducacion> Educaciones { get; set; }
        public DbSet<CVPublicacion> Publicaciones { get; set; }
        public DbSet<CVTesisDirigida> TesisDirigidas { get; set; }
        public DbSet<CVDocumento> Documentos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurar esquema por defecto para entidades CV
            modelBuilder.HasDefaultSchema("dbo");
            
            // Configurar EXPLÍCITAMENTE que las entidades CV usen el esquema CV
            modelBuilder.Entity<CVDocumento>().ToTable("Documento", "CV");
            modelBuilder.Entity<CVPublicacion>().ToTable("Publicacion", "CV");
            modelBuilder.Entity<CVDistincion>().ToTable("Distincion", "CV");
            modelBuilder.Entity<CVProyecto>().ToTable("Proyecto", "CV");
            modelBuilder.Entity<CVTesisDirigida>().ToTable("TesisDirigida", "CV");
            modelBuilder.Entity<CVExperiencia>().ToTable("Experiencia", "CV");
            modelBuilder.Entity<CVEducacion>().ToTable("Educacion", "CV");
            modelBuilder.Entity<CVContactoDocente>().ToTable("ContactoProfesor", "CV");
            modelBuilder.Entity<CVTipoContacto>().ToTable("TipoContacto", "CV");
            modelBuilder.Entity<CVCuerpoAcademico>().ToTable("CuerpoAcademico", "CV");

            // Planes de Estudio Configurations
            modelBuilder.ApplyConfiguration(new CarreraConfig());
            modelBuilder.ApplyConfiguration(new PlanEstudiosConfig());
            modelBuilder.ApplyConfiguration(new PlanEstudioMateriaConfig());
            modelBuilder.ApplyConfiguration(new MateriaConfig());
            modelBuilder.ApplyConfiguration(new EtapaConfig());
            modelBuilder.ApplyConfiguration(new AreaConocimientoConfig());

            // CurriculumVite Configurations
            modelBuilder.ApplyConfiguration(new E_DocenteConfig());
            modelBuilder.ApplyConfiguration(new E_SexoConfig());
            modelBuilder.ApplyConfiguration(new E_EstadoCivilConfig());
            modelBuilder.ApplyConfiguration(new E_CategoriaConfig());
            modelBuilder.ApplyConfiguration(new E_NombramientoConfig());
            modelBuilder.ApplyConfiguration(new E_EscolaridadConfig());
            modelBuilder.ApplyConfiguration(new E_SNIConfig());
            modelBuilder.ApplyConfiguration(new E_PRODEPConfig());
            modelBuilder.ApplyConfiguration(new E_CuerpoAcademicoConfig());
            
            modelBuilder.ApplyConfiguration(new E_DistincionConfig());
            modelBuilder.ApplyConfiguration(new E_ExperienciaConfig());
            modelBuilder.ApplyConfiguration(new E_TipoContactoConfig());
            modelBuilder.ApplyConfiguration(new E_ContactoDocenteConfig());
            modelBuilder.ApplyConfiguration(new E_ProyectoConfig());
            modelBuilder.ApplyConfiguration(new E_EducacionConfig());
            modelBuilder.ApplyConfiguration(new E_PublicacionConfig());
            modelBuilder.ApplyConfiguration(new E_TesisDirigidaConfig());
            modelBuilder.ApplyConfiguration(new E_DocumentoConfig());
        }
    }
}
