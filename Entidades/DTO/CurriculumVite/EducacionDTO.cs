using System.ComponentModel.DataAnnotations;

namespace Entidades.DTO.CurriculumVite
{
    public class EducacionDTO
    {
        public int IdEducacion { get; set; }
        public int IdDocente { get; set; }
        
        [Required(ErrorMessage = "El nivel educativo es obligatorio")]
        public string Nivel { get; set; } = null!;
        
        public string? Titulo { get; set; }
        
        [Required(ErrorMessage = "La institución es obligatoria")]
        public string? Institucion { get; set; }
        
        public string? Especialidad { get; set; }
        public string? Pais { get; set; }
        public int? AnioInicio { get; set; }
        public int? AnioFin { get; set; }
        
        // Propiedades calculadas
        public string NombreDocente { get; set; } = null!;
        public string PeriodoFormateado => $"{AnioInicio ?? 0} - {(AnioFin?.ToString() ?? "En curso")}";
        public bool EnCurso => !AnioFin.HasValue;
        public int DuracionAnios => AnioFin.HasValue && AnioInicio.HasValue ? 
            AnioFin.Value - AnioInicio.Value : 0;
    }
}
