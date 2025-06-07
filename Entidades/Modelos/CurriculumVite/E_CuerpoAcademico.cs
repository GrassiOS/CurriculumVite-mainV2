namespace Entidades.Modelos.CurriculumVite
{
    public class E_CuerpoAcademico
    {
        public int CuerpoAcademicoId { get; set; }
        public string Nombre { get; set; } = null!;
        public string? Descripcion { get; set; }
        
        // Navigation property
        public virtual ICollection<E_Docente>? Docentes { get; set; }
    }
} 