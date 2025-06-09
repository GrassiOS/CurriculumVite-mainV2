using Entidades.Generales;
using Entidades.DTO.CurriculumVite;
using Entidades.Modelos.CurriculumVite;

namespace Servicios.IRepositorios.CurriculumVite
{
    public interface ISDocenteRepositorio
    {
        Task<ResultadoAcciones<DocenteDTO>> InsertarDocente(DocenteDTO docenteDTO);
        Task<ResultadoAcciones> BorrarDocente(int idDocente);
        Task<ResultadoAcciones> ModificarDocente(DocenteDTO docenteDTO);
        Task<ResultadoAcciones<T>> ObtenerDocente<T>(int idDocente) where T : class;
        Task<IEnumerable<E_Docente>> ListarDocentes();
        Task<IEnumerable<E_Docente>> ListarDocentes(string criterioBusqueda);
        
        // Nuevos métodos para manejar la URL de la foto
        Task<ResultadoAcciones> ActualizarUrlFoto(int idDocente, string urlFoto);
        Task<ResultadoAcciones> EliminarUrlFoto(int idDocente);
        
        // Métodos para obtener catálogos
        Task<IEnumerable<SexoDTO>> ObtenerSexos();
        Task<IEnumerable<EstadoCivilDTO>> ObtenerEstadosCiviles();
        Task<IEnumerable<CategoriaDTO>> ObtenerCategorias();
        Task<IEnumerable<NombramientoDTO>> ObtenerNombramientos();
        Task<IEnumerable<EscolaridadDTO>> ObtenerEscolaridades();
        Task<IEnumerable<SNIDTO>> ObtenerNivelesSNI();
        Task<IEnumerable<PRODEPDTO>> ObtenerPRODEP();
    }
} 