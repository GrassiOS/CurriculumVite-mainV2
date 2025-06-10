using System.ComponentModel.DataAnnotations;

namespace Entidades.DTO.CurriculumVite
{
    public class ProyectoDTO
    {
        public int IdProyecto { get; set; }
        
        [Required(ErrorMessage = "Debe seleccionar un docente")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un docente válido")]
        public int IdDocente { get; set; }
        
        [Required(ErrorMessage = "El título del proyecto es obligatorio")]
        [StringLength(300, MinimumLength = 5, ErrorMessage = "El título debe tener entre 5 y 300 caracteres")]
        public string? Titulo { get; set; }
        
        [Required(ErrorMessage = "El rol en el proyecto es obligatorio")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "El rol debe tener entre 2 y 100 caracteres")]
        public string? Rol { get; set; }
        
        [Required(ErrorMessage = "La institución es obligatoria")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "La institución debe tener entre 2 y 200 caracteres")]
        public string? Institucion { get; set; }
        
        [StringLength(300, ErrorMessage = "El financiamiento no puede exceder 300 caracteres")]
        public string? Financiamiento { get; set; }
        
        [Required(ErrorMessage = "El año de inicio es obligatorio")]
        [Range(1950, 2100, ErrorMessage = "El año de inicio debe estar entre 1950 y 2100")]
        public int? PeriodoInicio { get; set; }
        
        [Required(ErrorMessage = "El año de fin es obligatorio")]
        [Range(1950, 2100, ErrorMessage = "El año de fin debe estar entre 1950 y 2100")]
        public int PeriodoFin { get; set; }
        
        // Propiedades calculadas
        public string NombreDocente { get; set; } = null!;
        public string PeriodoFormateado => $"{PeriodoInicio ?? 0} - {PeriodoFin}";
        public bool EsActivo => PeriodoFin >= DateTime.Now.Year;
        public int DuracionAnios => PeriodoInicio.HasValue ? 
            PeriodoFin - PeriodoInicio.Value + 1 : 1;
        public string TituloCorto => Titulo?.Length > 80 ? Titulo[..77] + "..." : Titulo ?? "";
        public bool TieneFinanciamiento => !string.IsNullOrEmpty(Financiamiento);
        public string EstadoProyecto => EsActivo ? "En curso" : "Finalizado";
        
        public string RolFormateado => string.IsNullOrEmpty(Rol) ? "Sin especificar" : Rol;
        public string FinanciamientoFormateado => string.IsNullOrEmpty(Financiamiento) ? "Sin financiamiento" : Financiamiento;
    }
}
