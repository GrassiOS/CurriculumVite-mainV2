namespace Entidades.Modelos.CurriculumVite
{
    public class E_EstadoCivil
    {
        public int IdEstadoCivil { get; set; }
        public string EstadoCivil { get; set; } = null!;
        
        // Navigation property
        public virtual ICollection<E_Docente>? Docentes { get; set; }
    }
} 