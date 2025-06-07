namespace Entidades.Modelos.CurriculumVite
{
    public class E_Sexo
    {
        public int IdSexo { get; set; }
        public string Sexo { get; set; } = null!;
        
        // Navigation property
        public virtual ICollection<E_Docente>? Docentes { get; set; }
    }
} 