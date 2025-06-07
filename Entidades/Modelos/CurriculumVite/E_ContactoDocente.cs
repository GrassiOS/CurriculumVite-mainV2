namespace Entidades.Modelos.CurriculumVite
{
    public class E_ContactoDocente
    {
        public int IdContacto { get; set; }
        public int IdDocente { get; set; }
        public int IdTpoContacto { get; set; }
        public string Url { get; set; } = null!;
        
        // Navigation properties
        public virtual E_Docente? Docente { get; set; }
        public virtual E_TipoContacto? TipoContacto { get; set; }
    }
}
