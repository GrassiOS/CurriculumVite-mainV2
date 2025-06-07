namespace Entidades.Modelos.CurriculumVite
{
    public class E_Nombramiento
    {
        public int IdNombramiento { get; set; }
        public string Nombramiento { get; set; } = null!;
        
        // Navigation property
        public virtual ICollection<E_Docente>? Docentes { get; set; }
    }
} 