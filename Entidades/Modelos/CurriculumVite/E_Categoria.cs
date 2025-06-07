namespace Entidades.Modelos.CurriculumVite
{
    public class E_Categoria
    {
        public int IdCategoria { get; set; }
        public string ClaveCategoria { get; set; } = null!;
        public string NombreCategoria { get; set; } = null!;
        
        // Navigation property
        public virtual ICollection<E_Docente>? Docentes { get; set; }
    }
} 