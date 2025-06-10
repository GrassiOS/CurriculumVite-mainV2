using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Entidades.DTO.CurriculumVite
{
    public class DocenteDTO
    {
        public int IdDocente { get; set; }
        
        // Foreign Keys para catálogos
        [Required(ErrorMessage = "Debe seleccionar el sexo")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un sexo válido")]
        public int IdSexo { get; set; }
        
        [Required(ErrorMessage = "Debe seleccionar el estado civil")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un estado civil válido")]
        public int IdEstadoCivil { get; set; }
        
        [Required(ErrorMessage = "Debe seleccionar la categoría")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una categoría válida")]
        public int IdCategoria { get; set; }
        
        [Required(ErrorMessage = "Debe seleccionar el nombramiento")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un nombramiento válido")]
        public int IdNombramiento { get; set; }
        
        [Required(ErrorMessage = "Debe seleccionar la escolaridad")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una escolaridad válida")]
        public int IdEscolaridad { get; set; }
        
        [Required(ErrorMessage = "Debe seleccionar el nivel SNI")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un nivel SNI válido")]
        public int IdNivelSNI { get; set; }
        
        public int IdPRODEP { get; set; }
        
        [Required(ErrorMessage = "Debe seleccionar la carrera")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una carrera válida")]
        public int IdCarrera { get; set; }
        
        public int? IdCuerpoAcademico { get; set; }
        
        // Información básica
        [Required(ErrorMessage = "El número de empleado es obligatorio")]
        [RegularExpression(@"^\d{3,10}$", ErrorMessage = "El número de empleado debe contener entre 3 y 10 dígitos")]
        public string NumeroEmpleado { get; set; } = null!;
        
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 100 caracteres")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚüÜñÑ\s\-']+$", ErrorMessage = "El nombre solo puede contener letras, espacios, guiones y apostrofes")]
        public string NombreDocente { get; set; } = null!;
        
        [StringLength(100, ErrorMessage = "El apellido paterno no puede exceder 100 caracteres")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚüÜñÑ\s\-']*$", ErrorMessage = "El apellido paterno solo puede contener letras, espacios, guiones y apostrofes")]
        public string? PaternoDocente { get; set; }
        
        [StringLength(100, ErrorMessage = "El apellido materno no puede exceder 100 caracteres")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚüÜñÑ\s\-']*$", ErrorMessage = "El apellido materno solo puede contener letras, espacios, guiones y apostrofes")]
        public string? MaternoDocente { get; set; }
        
        // Información personal
        [StringLength(500, ErrorMessage = "La dirección no puede exceder 500 caracteres")]
        public string? Direccion { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime? FechaNacimiento { get; set; }
        
        // Contactos
        [Phone(ErrorMessage = "El teléfono de casa no tiene un formato válido")]
        [RegularExpression(@"^[\d\s\-\(\)\+\.]*$", ErrorMessage = "El teléfono de casa solo puede contener números, espacios, guiones, paréntesis y símbolos +")]
        public string? TelefonoCasa { get; set; }
        
        [Phone(ErrorMessage = "El teléfono celular no tiene un formato válido")]
        [RegularExpression(@"^[\d\s\-\(\)\+\.]*$", ErrorMessage = "El teléfono celular solo puede contener números, espacios, guiones, paréntesis y símbolos +")]
        public string? TelefonoCelular { get; set; }
        
        [Phone(ErrorMessage = "El teléfono de trabajo no tiene un formato válido")]
        [RegularExpression(@"^[\d\s\-\(\)\+\.]*$", ErrorMessage = "El teléfono de trabajo solo puede contener números, espacios, guiones, paréntesis y símbolos +")]
        public string? TelefonoTrabajo { get; set; }
        
        [StringLength(10, ErrorMessage = "La extensión no puede exceder 10 caracteres")]
        [RegularExpression(@"^\d*$", ErrorMessage = "La extensión solo puede contener números")]
        public string? Extension { get; set; }
        
        [EmailAddress(ErrorMessage = "El email institucional no tiene un formato válido")]
        [StringLength(200, ErrorMessage = "El email institucional no puede exceder 200 caracteres")]
        public string? EmailInstitucional { get; set; }
        
        [EmailAddress(ErrorMessage = "El email alterno no tiene un formato válido")]
        [StringLength(200, ErrorMessage = "El email alterno no puede exceder 200 caracteres")]
        public string? EmailAlterno { get; set; }
        
        [Url(ErrorMessage = "La página web no tiene un formato válido")]
        [StringLength(500, ErrorMessage = "La página web no puede exceder 500 caracteres")]
        public string? PaginaWeb { get; set; }
        
        // Información adicional
        [StringLength(50, ErrorMessage = "El casillero no puede exceder 50 caracteres")]
        public string? Casillero { get; set; }
        
        [StringLength(20, ErrorMessage = "La cédula profesional no puede exceder 20 caracteres")]
        [RegularExpression(@"^[\d\-]*$", ErrorMessage = "La cédula profesional solo puede contener números y guiones")]
        public string? CedulaProfesional { get; set; }
        
        [StringLength(13, ErrorMessage = "El RFC no puede exceder 13 caracteres")]
        [RegularExpression(@"^[A-Z&Ñ]{3,4}\d{6}[A-Z0-9]{3}$", ErrorMessage = "El RFC no tiene un formato válido (formato: 3-4 letras, 6 dígitos de fecha, 3 caracteres de homoclave)")]
        public string? RFC { get; set; }
        
        [StringLength(18, ErrorMessage = "El CURP no puede exceder 18 caracteres")]
        [RegularExpression(@"^[A-Z]{1}[AEIOU]{1}[A-Z]{2}\d{2}(0[1-9]|1[0-2])(0[1-9]|[12]\d|3[01])[HM](AS|BC|BS|CC|CL|CM|CS|CH|DF|DG|GT|GR|HG|JC|MC|MN|MS|NT|NL|OC|PL|QT|QR|SP|SL|SR|TC|TS|TL|VZ|YN|ZS|NE)[B-DF-HJ-NP-TV-Z]{3}[A-Z0-9]{1}\d{1}$", ErrorMessage = "El CURP no tiene un formato válido")]
        public string? CURP { get; set; }
        
        [StringLength(200, ErrorMessage = "La universidad de licenciatura no puede exceder 200 caracteres")]
        public string? UniversidadLicenciatura { get; set; }
        
        // Fechas importantes
        [DataType(DataType.Date)]
        public DateTime? FechaIngreso { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime? FechaInicialSNI { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime? FechaFinalSNI { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime? FechaInicialPRODEP { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime? FechaFinalPRODEP { get; set; }
        
        // Estado y archivos
        [Required(ErrorMessage = "El estado del docente es obligatorio")]
        public int EstadoDocente { get; set; }
        
        [Url(ErrorMessage = "La URL de Drive no tiene un formato válido")]
        [StringLength(1000, ErrorMessage = "La URL de Drive no puede exceder 1000 caracteres")]
        public string? URL_Drive { get; set; }
        
        public byte[]? Foto { get; set; }
        
        [Url(ErrorMessage = "La URL de la foto no tiene un formato válido")]
        [StringLength(1000, ErrorMessage = "La URL de la foto no puede exceder 1000 caracteres")]
        public string? URLFoto { get; set; }
        
        [StringLength(2000, ErrorMessage = "La semblanza no puede exceder 2000 caracteres")]
        public string? SembalzaDocente { get; set; }
        
        // Información de catálogos (para mostrar en la UI)
        public string? NombreSexo { get; set; }
        public string? NombreEstadoCivil { get; set; }
        public string? NombreCategoria { get; set; }
        public string? NombreNombramiento { get; set; }
        public string? NombreEscolaridad { get; set; }
        public string? NombreNivelSNI { get; set; }
        public string? NombrePRODEP { get; set; }
        public string? NombreCarrera { get; set; }
        public string? AliasCarrera { get; set; }
        public string? NombreCuerpoAcademico { get; set; }
        
        // Colecciones relacionadas (IDs o conteos para evitar problemas de serialización)
        public List<ContactoDocenteDTO>? Contactos { get; set; }
        public int CantidadEducaciones { get; set; }
        public int CantidadExperiencias { get; set; }
        public int CantidadPublicaciones { get; set; }
        public int CantidadProyectos { get; set; }
        public int CantidadTesisDirigidas { get; set; }
        public int CantidadDistinciones { get; set; }
        public int CantidadDocumentos { get; set; }
        
        // Campos de respaldo para propiedades de compatibilidad
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
        public string NombreCompleto => $"{NombreDocente} {PaternoDocente} {MaternoDocente}".Trim();
        
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
        
        [StringLength(200, ErrorMessage = "La especialidad no puede exceder 200 caracteres")]
        public string? Especialidad { get; set; }
        
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
        
        // Propiedades de ayuda para UI
        public string InicialNombre => !string.IsNullOrEmpty(NombreCompleto) ? 
            NombreCompleto.Split(' ').Take(2).Select(n => n[0]).Aggregate("", (a, b) => a + b) : "";
        public int AniosEnUABC => FechaIngreso.HasValue ? 
            DateTime.Now.Year - FechaIngreso.Value.Year : 0;
        public bool EsActivo => EstadoDocente == 1;
        public string EstadoTexto => EsActivo ? "Activo" : "Inactivo";
    }
} 