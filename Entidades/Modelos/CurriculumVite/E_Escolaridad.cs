namespace Entidades.Modelos.CurriculumVite
{
    public class E_Escolaridad
    {
        public int IdEscolaridad { get; set; }
        public string ClaveEscolaridad { get; set; } = null!;
        public string NombreEscolaridad { get; set; } = null!;
        
        // Navigation property
        public virtual ICollection<E_Docente>? Docentes { get; set; }
    }
} 