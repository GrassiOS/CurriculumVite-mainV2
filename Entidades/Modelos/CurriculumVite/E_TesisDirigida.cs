namespace Entidades.Modelos.CurriculumVite
{
    public class E_TesisDirigida
    {
        public int IdTesis { get; set; }
        public int IdDocente { get; set; }
        public string? Autor { get; set; }
        public string? Titulo { get; set; }
        public string? Nivel { get; set; }
        public string? Universidad { get; set; }
        public int? Anio { get; set; }
        
        // Navigation property
        public virtual E_Docente? Docente { get; set; }
    }
}
