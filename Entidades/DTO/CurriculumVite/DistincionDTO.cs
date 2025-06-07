namespace Entidades.DTO.CurriculumVite
{
    public class DistincionDTO
    {
        public int IdDistincion { get; set; }
        public int IdDocente { get; set; }
        public string? Nombre { get; set; }
        public string? Institucion { get; set; }
        public string? Descripcion { get; set; }
        public int? Anio { get; set; }
        
        // Propiedades calculadas
        public string NombreDocente { get; set; } = null!;
        public string NombreFormateado => !string.IsNullOrEmpty(Nombre) ? Nombre : "Distinción sin nombre";
        public string InstitucionFormateada => !string.IsNullOrEmpty(Institucion) ? Institucion : "Institución no especificada";
        public bool EsReciente => Anio.HasValue && Anio.Value >= DateTime.Now.Year - 5;
        public string AnioFormateado => Anio?.ToString() ?? "Año no especificado";
        public bool TieneDescripcion => !string.IsNullOrEmpty(Descripcion);
        public string DescripcionCorta => Descripcion?.Length > 150 ? Descripcion[..147] + "..." : Descripcion ?? "";
    }
}
