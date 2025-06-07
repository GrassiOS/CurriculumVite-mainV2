using Entidades.DTO.CurriculumVite;

namespace Servicios.IRepositorios.CurriculumVite
{
    public interface ISRepositorioPDFGenerator
    {
        /// <summary>
        /// Genera un CV en formato PDF basado en la información del docente
        /// </summary>
        /// <param name="docente">Información del docente</param>
        /// <param name="educaciones">Lista de educaciones del docente</param>
        /// <param name="experiencias">Lista de experiencias del docente</param>
        /// <param name="publicaciones">Lista de publicaciones del docente</param>
        /// <param name="proyectos">Lista de proyectos del docente</param>
        /// <param name="tesisDirigidas">Lista de tesis dirigidas del docente</param>
        /// <param name="distinciones">Lista de distinciones del docente</param>
        /// <returns>Array de bytes del PDF generado</returns>
        byte[] GenerarCVPDF(
            DocenteDTO docente,
            List<EducacionDTO>? educaciones = null,
            List<ExperienciaDTO>? experiencias = null,
            List<PublicacionDTO>? publicaciones = null,
            List<ProyectoDTO>? proyectos = null,
            List<TesisDirigidaDTO>? tesisDirigidas = null,
            List<DistincionDTO>? distinciones = null);

        /// <summary>
        /// Genera un CV en formato PDF con datos del docente (método simplificado)
        /// </summary>
        /// <param name="docente">Información del docente</param>
        /// <param name="educaciones">Lista de educaciones del docente</param>
        /// <param name="experiencias">Lista de experiencias del docente</param>
        /// <param name="publicaciones">Lista de publicaciones del docente</param>
        /// <param name="proyectos">Lista de proyectos del docente</param>
        /// <param name="tesisDirigidas">Lista de tesis dirigidas del docente</param>
        /// <param name="distinciones">Lista de distinciones del docente</param>
        /// <returns>Array de bytes del PDF generado</returns>
        byte[] GenerarCV(
            DocenteDTO docente,
            List<EducacionDTO> educaciones,
            List<ExperienciaDTO> experiencias,
            List<PublicacionDTO> publicaciones,
            List<ProyectoDTO> proyectos,
            List<TesisDirigidaDTO> tesisDirigidas,
            List<DistincionDTO> distinciones);

        /// <summary>
        /// Genera un CV en formato PDF con contactos reales del docente (método mejorado)
        /// </summary>
        /// <param name="docente">Información del docente</param>
        /// <param name="educaciones">Lista de educaciones del docente</param>
        /// <param name="experiencias">Lista de experiencias del docente</param>
        /// <param name="publicaciones">Lista de publicaciones del docente</param>
        /// <param name="proyectos">Lista de proyectos del docente</param>
        /// <param name="tesisDirigidas">Lista de tesis dirigidas del docente</param>
        /// <param name="distinciones">Lista de distinciones del docente</param>
        /// <param name="contactos">Lista de contactos del docente</param>
        /// <returns>Array de bytes del PDF generado</returns>
        byte[] GenerarCVConContactos(
            DocenteDTO docente,
            List<EducacionDTO> educaciones,
            List<ExperienciaDTO> experiencias,
            List<PublicacionDTO> publicaciones,
            List<ProyectoDTO> proyectos,
            List<TesisDirigidaDTO> tesisDirigidas,
            List<DistincionDTO> distinciones,
            List<ContactoDocenteDTO> contactos,
            Dictionary<string, byte[]>? logos = null);

        /// <summary>
        /// Genera un CV demo para pruebas
        /// </summary>
        /// <returns>Array de bytes del PDF generado</returns>
        byte[] GenerarCVDemo();
    }
} 