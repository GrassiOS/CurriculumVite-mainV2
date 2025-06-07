using System;

namespace Entidades.Modelos.CurriculumVite
{
    public class E_Documento
    {
        public int IdDocumento { get; set; }
        public int IdDocente { get; set; }
        public int? IdPublicacion { get; set; }
        public int? IdDistincion { get; set; }
        public int? IdProyecto { get; set; }
        public int? IdTesis { get; set; }
        public int? IdEducacion { get; set; }
        public string? Titulo { get; set; }
        public string Url { get; set; } = null!;
        public string? Descripcion { get; set; }
        public DateTime FechaSubida { get; set; }
        
        // Sin navigation properties para evitar nombres automáticos de columnas
    }
}
