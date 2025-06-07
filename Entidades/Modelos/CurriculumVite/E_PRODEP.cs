namespace Entidades.Modelos.CurriculumVite
{
    public class E_PRODEP
    {
        public int IdPRODEP { get; set; }
        public string TienePRODEP { get; set; } = null!;
        
        // Navigation property
        public virtual ICollection<E_Docente>? Docentes { get; set; }
    }
} 