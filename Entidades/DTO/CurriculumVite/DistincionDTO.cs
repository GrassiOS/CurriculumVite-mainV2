using System.ComponentModel.DataAnnotations;

namespace Entidades.DTO.CurriculumVite
{
    public class DistincionDTO
    {
        public int IdDistincion { get; set; }
        
        [Required(ErrorMessage = "Debe seleccionar un docente")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un docente válido")]
        public int IdDocente { get; set; }
        
        [Required(ErrorMessage = "El nombre de la distinción es obligatorio")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 200 caracteres")]
        public string? Nombre { get; set; }
        
        [Required(ErrorMessage = "La institución es obligatoria")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "La institución debe tener entre 2 y 200 caracteres")]
        public string? Institucion { get; set; }
        
        [StringLength(1000, ErrorMessage = "La descripción no puede exceder 1000 caracteres")]
        public string? Descripcion { get; set; }
        
        [Range(1950, 2100, ErrorMessage = "El año debe estar entre 1950 y 2100")]
        public int? Anio { get; set; }
        
        // Propiedades calculadas
        public string NombreDocente { get; set; } = null!;
        public string NombreFormateado => !string.IsNullOrEmpty(Nombre) ? Nombre : "Distinción sin nombre";
        public string InstitucionFormateada => !string.IsNullOrEmpty(Institucion) ? Institucion : "Institución no especificada";
        public bool EsReciente => Anio.HasValue && Anio.Value >= DateTime.Now.Year - 5;
        public string AnioFormateado => Anio?.ToString() ?? "Año no especificado";
        public bool TieneDescripcion => !string.IsNullOrEmpty(Descripcion);
        public string DescripcionCorta => Descripcion?.Length > 150 ? Descripcion[..147] + "..." : Descripcion ?? "";
    }
}
