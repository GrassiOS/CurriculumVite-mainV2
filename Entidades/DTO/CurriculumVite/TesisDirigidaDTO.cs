namespace Entidades.DTO.CurriculumVite
{
    public class TesisDirigidaDTO
    {
        public int IdTesis { get; set; }
        public int IdDocente { get; set; }
        public string? Autor { get; set; }
        public string? Titulo { get; set; }
        public string? Nivel { get; set; }
        public string? Universidad { get; set; }
        public int? Anio { get; set; }
        
        // Propiedades calculadas
        public string NombreDocente { get; set; } = null!;
        public string TituloCorto => Titulo?.Length > 100 ? Titulo[..97] + "..." : Titulo ?? "";
        public bool EsReciente => Anio.HasValue && Anio.Value >= DateTime.Now.Year - 5;
        public string AnioFormateado => Anio?.ToString() ?? "En proceso";
        public string AutorFormateado => !string.IsNullOrEmpty(Autor) ? Autor : "Autor no especificado";
        public string NivelFormateado => !string.IsNullOrEmpty(Nivel) ? Nivel : "Nivel no especificado";
    }
}
