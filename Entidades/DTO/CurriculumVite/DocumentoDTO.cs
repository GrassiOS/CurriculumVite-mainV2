using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Entidades.DTO.CurriculumVite
{
    public class DocumentoDTO
    {
        public int IdDocumento { get; set; }
        
        [Required(ErrorMessage = "Debe seleccionar un docente")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un docente válido")]
        public int IdDocente { get; set; }
        
        public int? IdPublicacion { get; set; }
        public int? IdDistincion { get; set; }
        public int? IdProyecto { get; set; }
        public int? IdTesis { get; set; }
        public int? IdEducacion { get; set; }
        
        [StringLength(300, ErrorMessage = "El título no puede exceder 300 caracteres")]
        public string? Titulo { get; set; }
        
        [Required(ErrorMessage = "La URL del documento es obligatoria")]
        [Url(ErrorMessage = "La URL no tiene un formato válido")]
        [StringLength(1000, ErrorMessage = "La URL no puede exceder 1000 caracteres")]
        public string Url { get; set; } = null!;
        
        [StringLength(1000, ErrorMessage = "La descripción no puede exceder 1000 caracteres")]
        public string? Descripcion { get; set; }
        
        [Required(ErrorMessage = "La fecha de subida es obligatoria")]
        [DataType(DataType.DateTime)]
        public DateTime FechaSubida { get; set; }
        
        // Propiedades calculadas
        public string NombreDocente { get; set; } = null!;
        public string TituloFormateado => !string.IsNullOrEmpty(Titulo) ? Titulo : "Documento sin título";
        public string FechaSubidaFormateada => FechaSubida.ToString("dd 'de' MMMM 'de' yyyy", new System.Globalization.CultureInfo("es-ES"));
        public bool TieneDescripcion => !string.IsNullOrEmpty(Descripcion);
        public string DescripcionCorta => Descripcion?.Length > 100 ? Descripcion[..97] + "..." : Descripcion ?? "";
        public bool EsReciente => FechaSubida >= DateTime.Now.AddMonths(-6);
        
        // Propiedades para identificar el tipo de documento
        public bool EsDePublicacion => IdPublicacion.HasValue;
        public bool EsDeDistincion => IdDistincion.HasValue;
        public bool EsDeProyecto => IdProyecto.HasValue;
        public bool EsDeTesis => IdTesis.HasValue;
        public bool EsDeEducacion => IdEducacion.HasValue;
        public bool EsGeneral => !IdPublicacion.HasValue && !IdDistincion.HasValue && 
                                 !IdProyecto.HasValue && !IdTesis.HasValue && !IdEducacion.HasValue;
        
        public string TipoDocumento
        {
            get
            {
                if (EsDePublicacion) return "Publicación";
                if (EsDeDistincion) return "Distinción";
                if (EsDeProyecto) return "Proyecto";
                if (EsDeTesis) return "Tesis Dirigida";
                if (EsDeEducacion) return "Educación";
                return "General";
            }
        }
        
        // Extension del archivo
        public string Extension => System.IO.Path.GetExtension(Url)?.ToUpperInvariant() ?? "";
        public bool EsPDF => Extension == ".PDF";
        public bool EsImagen => new[] { ".JPG", ".JPEG", ".PNG", ".GIF", ".BMP" }.Contains(Extension);
        public bool EsDocumento => new[] { ".DOC", ".DOCX", ".PDF", ".TXT" }.Contains(Extension);
    }
}
