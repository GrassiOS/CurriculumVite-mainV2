using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PlanCarrera = Entidades.Modelos.PlanesDeEstudio.Carreras.E_Carrera;

namespace Entidades.Modelos.CurriculumVite
{
    public class E_Docente
    {
        public int IdDocente { get; set; }
        
        // Foreign Keys para catálogos
        public int IdSexo { get; set; }
        public int IdEstadoCivil { get; set; }
        public int IdCategoria { get; set; }
        public int IdNombramiento { get; set; }
        public int IdEscolaridad { get; set; }
        public int IdNivelSNI { get; set; }
        public int IdPRODEP { get; set; }
        public int IdCarrera { get; set; }
        public int? IdCuerpoAcademico { get; set; }
        
        // Información básica
        public string NumeroEmpleado { get; set; } = null!;
        public string NombreDocente { get; set; } = null!;
        public string? PaternoDocente { get; set; }
        public string? MaternoDocente { get; set; }
        
        // Información personal
        public string? Direccion { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        
        // Contactos
        public string? TelefonoCasa { get; set; }
        public string? TelefonoCelular { get; set; }
        public string? TelefonoTrabajo { get; set; }
        public string? Extension { get; set; }
        public string? EmailInstitucional { get; set; }
        public string? EmailAlterno { get; set; }
        public string? PaginaWeb { get; set; }
        
        // Información adicional
        public string? Casillero { get; set; }
        public string? CedulaProfesional { get; set; }
        public string? RFC { get; set; }
        public string? CURP { get; set; }
        public string? UniversidadLicenciatura { get; set; }
        
        // Fechas importantes
        public DateTime? FechaIngreso { get; set; }
        public DateTime? FechaInicialSNI { get; set; }
        public DateTime? FechaFinalSNI { get; set; }
        public DateTime? FechaInicialPRODEP { get; set; }
        public DateTime? FechaFinalPRODEP { get; set; }
        
        // Estado y archivos
        public int EstadoDocente { get; set; }
        public string? URL_Drive { get; set; }
        public byte[]? Foto { get; set; }
        public string? URLFoto{ get; set; }
        public string? SembalzaDocente { get; set; }
        
        // Campos de respaldo para propiedades de compatibilidad
        private string? _especialidad;
        private string? _emailOverride;
        private string? _telefonoOverride;
        private string? _cedulaOverride;
        private DateTime? _fechaRegistroOverride;
        
        // Propiedades de compatibilidad con la versión anterior
        public string? ApellidoPaterno 
        { 
            get => PaternoDocente; 
            set => PaternoDocente = value; 
        }
        
        public string? ApellidoMaterno 
        { 
            get => MaternoDocente; 
            set => MaternoDocente = value; 
        }
        
        // Propiedades calculadas con setters para compatibilidad
        public string Email 
        { 
            get => _emailOverride ?? EmailInstitucional ?? EmailAlterno ?? "";
            set => _emailOverride = value;
        }
        
        public string Telefono 
        { 
            get => _telefonoOverride ?? TelefonoCelular ?? TelefonoTrabajo ?? TelefonoCasa ?? "";
            set => _telefonoOverride = value;
        }
        
        public string? Cedula 
        { 
            get => _cedulaOverride ?? CedulaProfesional;
            set => _cedulaOverride = value;
        }
        
        public string? Especialidad 
        { 
            get => _especialidad; 
            set => _especialidad = value; 
        }
        
        public DateTime FechaRegistro 
        { 
            get => _fechaRegistroOverride ?? FechaIngreso ?? DateTime.Now;
            set => _fechaRegistroOverride = value;
        }
        
        // Propiedades booleanas de compatibilidad para EstadoDocente
        public bool EstadoDocenteBool 
        { 
            get => EstadoDocente == 1; 
            set => EstadoDocente = value ? 1 : 0; 
        }
        
        // Navigation Properties (para Entity Framework)
        public virtual E_Sexo? Sexo { get; set; }
        public virtual E_EstadoCivil? EstadoCivil { get; set; }
        public virtual E_Categoria? Categoria { get; set; }
        public virtual E_Nombramiento? Nombramiento { get; set; }
        public virtual E_Escolaridad? Escolaridad { get; set; }
        public virtual E_SNI? NivelSNI { get; set; }
        public virtual E_PRODEP? PRODEP { get; set; }
        public virtual PlanCarrera? Carrera { get; set; }
        public virtual E_CuerpoAcademico? CuerpoAcademico { get; set; }
        
        // Colecciones relacionadas
        public virtual ICollection<E_ContactoDocente>? Contactos { get; set; }
        public virtual ICollection<E_Educacion>? Educaciones { get; set; }
        public virtual ICollection<E_Experiencia>? Experiencias { get; set; }
        public virtual ICollection<E_Publicacion>? Publicaciones { get; set; }
        public virtual ICollection<E_Proyecto>? Proyectos { get; set; }
        public virtual ICollection<E_TesisDirigida>? TesisDirigidas { get; set; }
        public virtual ICollection<E_Distincion>? Distinciones { get; set; }
        public virtual ICollection<E_Documento>? Documentos { get; set; }
    }
} 