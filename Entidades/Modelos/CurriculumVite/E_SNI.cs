namespace Entidades.Modelos.CurriculumVite
{
    public class E_SNI
    {
        public int IdNivelSNI { get; set; }
        public string NivelSNI { get; set; } = null!;
        
        // Navigation property
        public virtual ICollection<E_Docente>? Docentes { get; set; }
    }
} 