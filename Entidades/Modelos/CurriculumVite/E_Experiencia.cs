using System;

namespace Entidades.Modelos.CurriculumVite
{
    public class E_Experiencia
    {
        public int IdExperiencia { get; set; }
        public int IdDocente { get; set; }
        public string? Puesto { get; set; }
        public string? Institucion { get; set; }
        public string? Descripcion { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        
        // Sin navigation properties como en E_Educacion
    }
}
