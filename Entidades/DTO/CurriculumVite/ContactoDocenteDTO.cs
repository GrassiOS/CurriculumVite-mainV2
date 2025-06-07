using System.ComponentModel.DataAnnotations;

namespace Entidades.DTO.CurriculumVite
{
    public class ContactoDocenteDTO
    {
        public int IdContacto { get; set; }
        public int IdDocente { get; set; }
        [Required(ErrorMessage = "Debe seleccionar un tipo de contacto")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un tipo de contacto válido")]
        public int IdTipoContacto { get; set; }
        [Required(ErrorMessage = "La URL/enlace es obligatoria")]
        [StringLength(500, ErrorMessage = "La URL/enlace no puede exceder 500 caracteres")]
        public string Url { get; set; } = null!;
        
        // Información del tipo de contacto (para display)
        public string? NombreTipoContacto { get; set; }
        public TipoContactoDTO? TipoContacto { get; set; }
        
        // Propiedades calculadas
        public string UrlFormateada => 
            Url.StartsWith("http://") || Url.StartsWith("https://") ? Url : $"https://{Url}";
            
        public string NombreContactoDisplay => 
            !string.IsNullOrEmpty(NombreTipoContacto) ? NombreTipoContacto : "Contacto";
            
        public string IconoTipoContacto => 
            TipoContacto?.Icono ?? "fas fa-link";
            
        public string ColorTipoContacto => 
            TipoContacto?.ColorFondo ?? "#2D6B3C";
    }
}
