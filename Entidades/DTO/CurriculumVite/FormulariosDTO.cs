using System.ComponentModel.DataAnnotations;

namespace Entidades.DTO.CurriculumVite
{
    /// <summary>
    /// DTO para crear/editar un docente
    /// </summary>
    public class DocenteFormDTO
    {
        public int IdDocente { get; set; }
        
        // Información básica requerida
        [Required(ErrorMessage = "El número de empleado es requerido")]
        [StringLength(10, ErrorMessage = "El número de empleado no puede exceder 10 caracteres")]
        public string NumeroEmpleado { get; set; } = null!;
        
        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
        public string NombreDocente { get; set; } = null!;
        
        [StringLength(100, ErrorMessage = "El apellido paterno no puede exceder 100 caracteres")]
        public string? PaternoDocente { get; set; }
        
        [StringLength(100, ErrorMessage = "El apellido materno no puede exceder 100 caracteres")]
        public string? MaternoDocente { get; set; }
        
        // Información personal
        [StringLength(500, ErrorMessage = "La dirección no puede exceder 500 caracteres")]
        public string? Direccion { get; set; }
        
        public DateTime? FechaNacimiento { get; set; }
        
        // Contactos
        [StringLength(40, ErrorMessage = "El teléfono de casa no puede exceder 40 caracteres")]
        public string? TelefonoCasa { get; set; }
        
        [StringLength(40, ErrorMessage = "El teléfono celular no puede exceder 40 caracteres")]
        public string? TelefonoCelular { get; set; }
        
        [StringLength(40, ErrorMessage = "El teléfono de trabajo no puede exceder 40 caracteres")]
        public string? TelefonoTrabajo { get; set; }
        
        [StringLength(14, ErrorMessage = "La extensión no puede exceder 14 caracteres")]
        public string? Extension { get; set; }
        
        [EmailAddress(ErrorMessage = "El email institucional no tiene un formato válido")]
        [StringLength(300, ErrorMessage = "El email institucional no puede exceder 300 caracteres")]
        public string? EmailInstitucional { get; set; }
        
        [EmailAddress(ErrorMessage = "El email alterno no tiene un formato válido")]
        [StringLength(300, ErrorMessage = "El email alterno no puede exceder 300 caracteres")]
        public string? EmailAlterno { get; set; }
        
        [Url(ErrorMessage = "La página web no tiene un formato válido")]
        [StringLength(500, ErrorMessage = "La página web no puede exceder 500 caracteres")]
        public string? PaginaWeb { get; set; }
        
        // Información adicional
        [StringLength(10, ErrorMessage = "El casillero no puede exceder 10 caracteres")]
        public string? Casillero { get; set; }
        
        [StringLength(50, ErrorMessage = "La cédula profesional no puede exceder 50 caracteres")]
        public string? CedulaProfesional { get; set; }
        
        [StringLength(40, ErrorMessage = "El RFC no puede exceder 40 caracteres")]
        public string? RFC { get; set; }
        
        [StringLength(40, ErrorMessage = "El CURP no puede exceder 40 caracteres")]
        public string? CURP { get; set; }
        
        [StringLength(500, ErrorMessage = "La universidad de licenciatura no puede exceder 500 caracteres")]
        public string? UniversidadLicenciatura { get; set; }
        
        // Fechas importantes
        public DateTime? FechaIngreso { get; set; }
        public DateTime? FechaInicialSNI { get; set; }
        public DateTime? FechaFinalSNI { get; set; }
        public DateTime? FechaInicialPRODEP { get; set; }
        public DateTime? FechaFinalPRODEP { get; set; }
        
        // Catálogos (requeridos)
        [Required(ErrorMessage = "El sexo es requerido")]
        public int IdSexo { get; set; }
        
        [Required(ErrorMessage = "El estado civil es requerido")]
        public int IdEstadoCivil { get; set; }
        
        [Required(ErrorMessage = "La categoría es requerida")]
        public int IdCategoria { get; set; }
        
        [Required(ErrorMessage = "El nombramiento es requerido")]
        public int IdNombramiento { get; set; }
        
        [Required(ErrorMessage = "La escolaridad es requerida")]
        public int IdEscolaridad { get; set; }
        
        [Required(ErrorMessage = "El nivel SNI es requerido")]
        public int IdNivelSNI { get; set; }
        
        [Required(ErrorMessage = "El PRODEP es requerido")]
        public int IdPRODEP { get; set; }
        
        [Required(ErrorMessage = "La carrera es requerida")]
        public int IdCarrera { get; set; }
        
        public int? IdCuerpoAcademico { get; set; }
        
        // Estado
        public int EstadoDocente { get; set; } = 1;
        
        // URL Drive y semblanza
        public string? URL_Drive { get; set; }
        public string? SembalzaDocente { get; set; }
    }

    /// <summary>
    /// DTO para crear/editar educación
    /// </summary>
    public class EducacionFormDTO
    {
        public int IdEducacion { get; set; }
        public int IdDocente { get; set; }
        
        [Required(ErrorMessage = "El nivel educativo es requerido")]
        [StringLength(100, ErrorMessage = "El nivel no puede exceder 100 caracteres")]
        public string Nivel { get; set; } = null!;
        
        [StringLength(400, ErrorMessage = "El título no puede exceder 400 caracteres")]
        public string? Titulo { get; set; }
        
        [StringLength(400, ErrorMessage = "La institución no puede exceder 400 caracteres")]
        public string? Institucion { get; set; }
        
        [StringLength(400, ErrorMessage = "La especialidad no puede exceder 400 caracteres")]
        public string? Especialidad { get; set; }
        
        [StringLength(200, ErrorMessage = "El país no puede exceder 200 caracteres")]
        public string? Pais { get; set; }
        
        [Range(1900, 2100, ErrorMessage = "El año de inicio debe estar entre 1900 y 2100")]
        public int? AnioInicio { get; set; }
        
        [Range(1900, 2100, ErrorMessage = "El año de fin debe estar entre 1900 y 2100")]
        public int? AnioFin { get; set; }
    }

    /// <summary>
    /// DTO para crear/editar experiencia
    /// </summary>
    public class ExperienciaFormDTO
    {
        public int IdExperiencia { get; set; }
        public int IdDocente { get; set; }
        
        [StringLength(400, ErrorMessage = "El puesto no puede exceder 400 caracteres")]
        public string? Puesto { get; set; }
        
        [StringLength(400, ErrorMessage = "La institución no puede exceder 400 caracteres")]
        public string? Institucion { get; set; }
        
        public string? Descripcion { get; set; }
        
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
    }

    /// <summary>
    /// DTO para crear/editar publicación
    /// </summary>
    public class PublicacionFormDTO
    {
        public int IdPublicacion { get; set; }
        public int IdDocente { get; set; }
        
        [Required(ErrorMessage = "El título es requerido")]
        public string? Titulo { get; set; }
        
        [StringLength(200, ErrorMessage = "El tipo de publicación no puede exceder 200 caracteres")]
        public string? TipoPublicacion { get; set; }
        
        public string? Autores { get; set; }
        
        [StringLength(600, ErrorMessage = "La fuente no puede exceder 600 caracteres")]
        public string? Fuente { get; set; }
        
        [Range(1900, 2100, ErrorMessage = "El año debe estar entre 1900 y 2100")]
        public int? Anio { get; set; }
        
        [Url(ErrorMessage = "El enlace no tiene un formato válido")]
        public string? Enlace { get; set; }
    }

    /// <summary>
    /// DTO para crear/editar proyecto
    /// </summary>
    public class ProyectoFormDTO
    {
        public int IdProyecto { get; set; }
        public int IdDocente { get; set; }
        
        [Required(ErrorMessage = "El título es requerido")]
        public string? Titulo { get; set; }
        
        [StringLength(200, ErrorMessage = "El rol no puede exceder 200 caracteres")]
        public string? Rol { get; set; }
        
        [StringLength(400, ErrorMessage = "La institución no puede exceder 400 caracteres")]
        public string? Institucion { get; set; }
        
        [StringLength(400, ErrorMessage = "El financiamiento no puede exceder 400 caracteres")]
        public string? Financiamiento { get; set; }
        
        [Range(1900, 2100, ErrorMessage = "El período de inicio debe estar entre 1900 y 2100")]
        public int? PeriodoInicio { get; set; }
        
        [Required(ErrorMessage = "El período de fin es requerido")]
        [Range(1900, 2100, ErrorMessage = "El período de fin debe estar entre 1900 y 2100")]
        public int PeriodoFin { get; set; }
    }

    /// <summary>
    /// DTO para crear/editar tesis dirigida
    /// </summary>
    public class TesisDirigidaFormDTO
    {
        public int IdTesis { get; set; }
        public int IdDocente { get; set; }
        
        [StringLength(400, ErrorMessage = "El autor no puede exceder 400 caracteres")]
        public string? Autor { get; set; }
        
        [Required(ErrorMessage = "El título es requerido")]
        public string? Titulo { get; set; }
        
        [StringLength(100, ErrorMessage = "El nivel no puede exceder 100 caracteres")]
        public string? Nivel { get; set; }
        
        [StringLength(400, ErrorMessage = "La universidad no puede exceder 400 caracteres")]
        public string? Universidad { get; set; }
        
        [Range(1900, 2100, ErrorMessage = "El año debe estar entre 1900 y 2100")]
        public int? Anio { get; set; }
    }

    /// <summary>
    /// DTO para crear/editar distinción
    /// </summary>
    public class DistincionFormDTO
    {
        public int IdDistincion { get; set; }
        public int IdDocente { get; set; }
        
        [Required(ErrorMessage = "El nombre de la distinción es requerido")]
        [StringLength(400, ErrorMessage = "El nombre no puede exceder 400 caracteres")]
        public string? Nombre { get; set; }
        
        [StringLength(400, ErrorMessage = "La institución no puede exceder 400 caracteres")]
        public string? Institucion { get; set; }
        
        public string? Descripcion { get; set; }
        
        [Range(1900, 2100, ErrorMessage = "El año debe estar entre 1900 y 2100")]
        public int? Anio { get; set; }
    }

    /// <summary>
    /// DTO para crear/editar documento
    /// </summary>
    public class DocumentoFormDTO
    {
        public int IdDocumento { get; set; }
        public int IdDocente { get; set; }
        public int? IdPublicacion { get; set; }
        public int? IdDistincion { get; set; }
        public int? IdProyecto { get; set; }
        public int? IdTesis { get; set; }
        public int? IdEducacion { get; set; }
        
        [StringLength(510, ErrorMessage = "El título no puede exceder 510 caracteres")]
        public string? Titulo { get; set; }
        
        [Required(ErrorMessage = "La URL del documento es requerida")]
        [Url(ErrorMessage = "La URL no tiene un formato válido")]
        public string Url { get; set; } = null!;
        
        public string? Descripcion { get; set; }
        
        public DateTime FechaSubida { get; set; } = DateTime.Now;
    }
} 