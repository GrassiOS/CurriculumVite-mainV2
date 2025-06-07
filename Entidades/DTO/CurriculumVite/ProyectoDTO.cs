using System.ComponentModel.DataAnnotations;

namespace Entidades.DTO.CurriculumVite
{
    public class ProyectoDTO
    {
        public int IdProyecto { get; set; }
        public int IdDocente { get; set; }
        
        [Required(ErrorMessage = "El título del proyecto es obligatorio")]
        public string? Titulo { get; set; }
        
        [Required(ErrorMessage = "El rol en el proyecto es obligatorio")]
        public string? Rol { get; set; }
        
        [Required(ErrorMessage = "La institución es obligatoria")]
        public string? Institucion { get; set; }
        
        public string? Financiamiento { get; set; }
        
        [Required(ErrorMessage = "El año de inicio es obligatorio")]
        [Range(1900, 2100, ErrorMessage = "El año de inicio debe estar entre 1900 y 2100")]
        public int? PeriodoInicio { get; set; }
        
        [Required(ErrorMessage = "El año de fin es obligatorio")]
        [Range(1900, 2100, ErrorMessage = "El año de fin debe estar entre 1900 y 2100")]
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
