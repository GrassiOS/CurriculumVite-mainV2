using System.ComponentModel.DataAnnotations;

namespace Entidades.DTO.CurriculumVite
{
    public class EducacionDTO
    {
        public int IdEducacion { get; set; }
        
        [Required(ErrorMessage = "Debe seleccionar un docente")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un docente válido")]
        public int IdDocente { get; set; }
        
        [Required(ErrorMessage = "El nivel educativo es obligatorio")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "El nivel educativo debe tener entre 2 y 100 caracteres")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚüÜñÑ\s\-\.]+$", ErrorMessage = "El nivel educativo solo puede contener letras, espacios, guiones y puntos")]
        public string Nivel { get; set; } = null!;
        
        [StringLength(300, ErrorMessage = "El título no puede exceder 300 caracteres")]
        public string? Titulo { get; set; }
        
        [Required(ErrorMessage = "La institución es obligatoria")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "La institución debe tener entre 2 y 200 caracteres")]
        public string? Institucion { get; set; }
        
        [StringLength(200, ErrorMessage = "La especialidad no puede exceder 200 caracteres")]
        public string? Especialidad { get; set; }
        
        [StringLength(100, ErrorMessage = "El país no puede exceder 100 caracteres")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚüÜñÑ\s\-]+$", ErrorMessage = "El país solo puede contener letras, espacios y guiones")]
        public string? Pais { get; set; }
        
        [Range(1950, 2100, ErrorMessage = "El año de inicio debe estar entre 1950 y 2100")]
        public int? AnioInicio { get; set; }
        
        [Range(1950, 2100, ErrorMessage = "El año de fin debe estar entre 1950 y 2100")]
        public int? AnioFin { get; set; }
        
        // Propiedades calculadas
        public string NombreDocente { get; set; } = null!;
        public string PeriodoFormateado => $"{AnioInicio ?? 0} - {(AnioFin?.ToString() ?? "En curso")}";
        public bool EnCurso => !AnioFin.HasValue;
        public int DuracionAnios => AnioFin.HasValue && AnioInicio.HasValue ? 
            AnioFin.Value - AnioInicio.Value : 0;
    }
}
