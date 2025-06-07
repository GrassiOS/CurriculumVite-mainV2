using System.ComponentModel.DataAnnotations;

namespace Entidades.DTO.CurriculumVite
{
    public class PublicacionDTO
    {
        public int IdPublicacion { get; set; }
        public int IdDocente { get; set; }
        
        [Required(ErrorMessage = "El título es requerido")]
        [StringLength(500, ErrorMessage = "El título no puede exceder 500 caracteres")]
        public string? Titulo { get; set; }
        
        [Required(ErrorMessage = "El tipo de publicación es requerido")]
        public string? TipoPublicacion { get; set; }
        
        [StringLength(1000, ErrorMessage = "Los autores no pueden exceder 1000 caracteres")]
        public string? Autores { get; set; }
        
        [StringLength(600, ErrorMessage = "La fuente no puede exceder 600 caracteres")]
        public string? Fuente { get; set; }
        
        [Range(1950, 2100, ErrorMessage = "El año debe estar entre 1950 y 2100")]
        public int? Anio { get; set; }
        
        [StringLength(1000, ErrorMessage = "El enlace no puede exceder 1000 caracteres")]
        public string? Enlace { get; set; }
        
        // Propiedades calculadas
        public string NombreDocente { get; set; } = null!;
        public bool TieneEnlace => !string.IsNullOrEmpty(Enlace);
        public string TituloCorto => Titulo?.Length > 100 ? Titulo[..97] + "..." : Titulo ?? "";
        public List<string> ListaAutores => 
            string.IsNullOrEmpty(Autores) ? new List<string>() : 
            Autores.Split(',').Select(a => a.Trim()).ToList();
        public int CantidadAutores => ListaAutores.Count;
        public string AutoresFormateados => 
            CantidadAutores > 3 ? 
                string.Join(", ", ListaAutores.Take(3)) + " et al." : 
                Autores ?? "";
    }
}
