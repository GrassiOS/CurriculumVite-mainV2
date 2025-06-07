namespace Entidades.Modelos.CurriculumVite
{
    public class E_Proyecto
    {
        public int IdProyecto { get; set; }
        public int IdDocente { get; set; }
        public string? Titulo { get; set; }
        public string? Rol { get; set; }
        public string? Institucion { get; set; }
        public string? Financiamiento { get; set; }
        public int? PeriodoInicio { get; set; }
        public int PeriodoFin { get; set; }
        
        // Sin navigation properties como en E_Educacion y E_Experiencia
    }
}
