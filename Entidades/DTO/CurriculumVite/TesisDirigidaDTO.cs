using System.ComponentModel.DataAnnotations;

namespace Entidades.DTO.CurriculumVite
{
    public class TesisDirigidaDTO
    {
        public int IdTesis { get; set; }
        
        [Required(ErrorMessage = "Debe seleccionar un docente")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un docente válido")]
        public int IdDocente { get; set; }
        
        [Required(ErrorMessage = "El autor de la tesis es obligatorio")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "El autor debe tener entre 2 y 200 caracteres")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚüÜñÑ\s\-'\.]+$", ErrorMessage = "El autor solo puede contener letras, espacios, guiones, apostrofes y puntos")]
        public string? Autor { get; set; }
        
        [Required(ErrorMessage = "El título de la tesis es obligatorio")]
        [StringLength(500, MinimumLength = 5, ErrorMessage = "El título debe tener entre 5 y 500 caracteres")]
        public string? Titulo { get; set; }
        
        [Required(ErrorMessage = "El nivel es obligatorio")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "El nivel debe tener entre 2 y 100 caracteres")]
        public string? Nivel { get; set; }
        
        [Required(ErrorMessage = "La universidad es obligatoria")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "La universidad debe tener entre 2 y 200 caracteres")]
        public string? Universidad { get; set; }
        
        [Range(1950, 2100, ErrorMessage = "El año debe estar entre 1950 y 2100")]
        public int? Anio { get; set; }
        
        // Propiedades calculadas
        public string NombreDocente { get; set; } = null!;
        public string TituloCorto => Titulo?.Length > 100 ? Titulo[..97] + "..." : Titulo ?? "";
        public bool EsReciente => Anio.HasValue && Anio.Value >= DateTime.Now.Year - 5;
        public string AnioFormateado => Anio?.ToString() ?? "En proceso";
        public string AutorFormateado => !string.IsNullOrEmpty(Autor) ? Autor : "Autor no especificado";
        public string NivelFormateado => !string.IsNullOrEmpty(Nivel) ? Nivel : "Nivel no especificado";
    }
}
