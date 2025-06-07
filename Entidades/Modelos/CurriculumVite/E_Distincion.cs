using System;

namespace Entidades.Modelos.CurriculumVite
{
    public class E_Distincion
    {
        public int IdDistincion { get; set; }
        public int IdDocente { get; set; }
        public string? Nombre { get; set; }
        public string? Institucion { get; set; }
        public string? Descripcion { get; set; }
        public int? Anio { get; set; }
        
        // Navigation property
        public virtual E_Docente? Docente { get; set; }
    }
}
