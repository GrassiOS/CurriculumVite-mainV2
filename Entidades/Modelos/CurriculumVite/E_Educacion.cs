namespace Entidades.Modelos.CurriculumVite
{
    public class E_Educacion
    {
        public int IdEducacion { get; set; }
        public int IdDocente { get; set; }
        public string Nivel { get; set; } = null!;
        public string? Titulo { get; set; }
        public string? Institucion { get; set; }
        public string? Especialidad { get; set; }
        public string? Pais { get; set; }
        public int? AnioInicio { get; set; }
        public int? AnioFin { get; set; }
        
        // Sin navigation properties como en E_Publicacion
    }
}
