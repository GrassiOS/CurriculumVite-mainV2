namespace Entidades.DTO.CurriculumVite
{
    /// <summary>
    /// DTO para mostrar un resumen completo del profesor con estadísticas
    /// </summary>
    public class DocenteResumenDTO
    {
        public int IdDocente { get; set; }
        public string NombreCompleto { get; set; } = null!;
        public string? EmailInstitucional { get; set; }
        public string? TelefonoCelular { get; set; }
        public string? NombreCarrera { get; set; }
        public string? AliasCarrera { get; set; }
        public string? NombreCuerpoAcademico { get; set; }
        public string? NombreCategoria { get; set; }
        public string? NombreNombramiento { get; set; }
        public string? NivelSNI { get; set; }
        public string? TienePRODEP { get; set; }
        public bool EstadoActivo { get; set; }
        public DateTime? FechaIngreso { get; set; }
        
        // Estadísticas del CV
        public int TotalEducaciones { get; set; }
        public int TotalExperiencias { get; set; }
        public int TotalPublicaciones { get; set; }
        public int TotalProyectos { get; set; }
        public int TotalTesisDirigidas { get; set; }
        public int TotalDistinciones { get; set; }
        public int TotalDocumentos { get; set; }
        
        // Propiedades calculadas
        public string InicialNombre => !string.IsNullOrEmpty(NombreCompleto) ? 
            NombreCompleto.Split(' ').Take(2).Select(n => n[0]).Aggregate("", (a, b) => a + b) : "";
        public int AniosEnUABC => FechaIngreso.HasValue ? 
            DateTime.Now.Year - FechaIngreso.Value.Year : 0;
        public int CompletitudCV => CalcularCompletitudCV();
        
        private int CalcularCompletitudCV()
        {
            int puntos = 0;
            if (TotalEducaciones > 0) puntos += 20;
            if (TotalExperiencias > 0) puntos += 20;
            if (TotalPublicaciones > 0) puntos += 20;
            if (TotalProyectos > 0) puntos += 15;
            if (TotalTesisDirigidas > 0) puntos += 15;
            if (TotalDistinciones > 0) puntos += 10;
            return puntos;
        }
    }

    /// <summary>
    /// DTO para estadísticas generales del sistema
    /// </summary>
    public class EstadisticasGeneralesDTO
    {
        public int TotalDocentes { get; set; }
        public int DocentesActivos { get; set; }
        public int DocentesInactivos { get; set; }
        public int TotalCarreras { get; set; }
        public int TotalCuerposAcademicos { get; set; }
        
        // Estadísticas de CV
        public int TotalPublicaciones { get; set; }
        public int TotalProyectos { get; set; }
        public int TotalTesisDirigidas { get; set; }
        public int TotalDistinciones { get; set; }
        public int TotalDocumentos { get; set; }
        
        // Estadísticas por categorías
        public Dictionary<string, int> DocentesPorCategoria { get; set; } = new();
        public Dictionary<string, int> DocentesPorCarrera { get; set; } = new();
        public Dictionary<string, int> DocentesPorSNI { get; set; } = new();
        public Dictionary<string, int> DocentesPorPRODEP { get; set; } = new();
        
        // Promedios
        public double PromedioPublicacionesPorDocente => TotalDocentes > 0 ? 
            (double)TotalPublicaciones / TotalDocentes : 0;
        public double PromedioProyectosPorDocente => TotalDocentes > 0 ? 
            (double)TotalProyectos / TotalDocentes : 0;
        public double PorcentajeDocentesActivos => TotalDocentes > 0 ? 
            (double)DocentesActivos / TotalDocentes * 100 : 0;
    }

    /// <summary>
    /// DTO para búsqueda y filtrado de docentes
    /// </summary>
    public class BusquedaDocenteDTO
    {
        public string? Nombre { get; set; }
        public string? Email { get; set; }
        public int? IdCarrera { get; set; }
        public int? IdCategoria { get; set; }
        public int? IdCuerpoAcademico { get; set; }
        public int? IdNivelSNI { get; set; }
        public int? IdPRODEP { get; set; }
        public bool? EstadoActivo { get; set; }
        public DateTime? FechaIngresoDesde { get; set; }
        public DateTime? FechaIngresoHasta { get; set; }
        
        // Parámetros de paginación
        public int Pagina { get; set; } = 1;
        public int TamanioPagina { get; set; } = 10;
        public string CampoOrden { get; set; } = "NombreCompleto";
        public bool OrdenDescendente { get; set; } = false;
    }

    /// <summary>
    /// DTO para resultados paginados
    /// </summary>
    public class ResultadoPaginadoDTO<T>
    {
        public List<T> Items { get; set; } = new();
        public int TotalItems { get; set; }
        public int PaginaActual { get; set; }
        public int TotalPaginas { get; set; }
        public int TamanioPagina { get; set; }
        public bool TienePaginaAnterior => PaginaActual > 1;
        public bool TienePaginaSiguiente => PaginaActual < TotalPaginas;
    }

    /// <summary>
    /// DTO para exportación de CV completo
    /// </summary>
    public class CVCompletoDTO
    {
        public DocenteDTO Docente { get; set; } = null!;
        public List<ContactoDocenteDTO> Contactos { get; set; } = new();
        public List<EducacionDTO> Educaciones { get; set; } = new();
        public List<ExperienciaDTO> Experiencias { get; set; } = new();
        public List<PublicacionDTO> Publicaciones { get; set; } = new();
        public List<ProyectoDTO> Proyectos { get; set; } = new();
        public List<TesisDirigidaDTO> TesisDirigidas { get; set; } = new();
        public List<DistincionDTO> Distinciones { get; set; } = new();
        public List<DocumentoDTO> Documentos { get; set; } = new();
        
        // Metadatos del CV
        public DateTime FechaGeneracion { get; set; } = DateTime.Now;
        public string VersionSistema { get; set; } = "1.0";
        public int CompletitudPorcentaje { get; set; }
        
        public void CalcularCompletitud()
        {
            int puntos = 0;
            if (Educaciones.Any()) puntos += 20;
            if (Experiencias.Any()) puntos += 20;
            if (Publicaciones.Any()) puntos += 20;
            if (Proyectos.Any()) puntos += 15;
            if (TesisDirigidas.Any()) puntos += 15;
            if (Distinciones.Any()) puntos += 10;
            CompletitudPorcentaje = puntos;
        }
    }

    /// <summary>
    /// DTO para dashboards y reportes
    /// </summary>
    public class DashboardDocenteDTO
    {
        public int IdDocente { get; set; }
        public string NombreCompleto { get; set; } = null!;
        
        // Actividad reciente
        public List<ActividadRecienteDTO> ActividadesRecientes { get; set; } = new();
        
        // Métricas del año actual
        public int PublicacionesEsteAnio { get; set; }
        public int ProyectosActivos { get; set; }
        public int TesisEnProceso { get; set; }
        public int DocumentosSubidosEsteAnio { get; set; }
        
        // Indicadores de completitud
        public bool TieneFoto { get; set; }
        public bool TieneSemblanza { get; set; }
        public bool TieneContactosCompletos { get; set; }
        public bool TieneCVActualizado { get; set; }
        
        // Próximas fechas importantes
        public DateTime? VencimientoSNI { get; set; }
        public DateTime? VencimientoPRODEP { get; set; }
    }

    /// <summary>
    /// DTO para actividades recientes del docente
    /// </summary>
    public class ActividadRecienteDTO
    {
        public string Tipo { get; set; } = null!; // "Publicacion", "Proyecto", "Documento", etc.
        public string Titulo { get; set; } = null!;
        public DateTime Fecha { get; set; }
        public string Descripcion { get; set; } = null!;
        public string Icono { get; set; } = null!;
        public string Color { get; set; } = null!;
    }
} 