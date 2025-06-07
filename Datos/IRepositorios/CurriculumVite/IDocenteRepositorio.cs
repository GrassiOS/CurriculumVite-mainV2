using Entidades.Generales;
using Entidades.Modelos.CurriculumVite;

namespace Datos.IRepositorios.CurriculumVite
{
    public interface IDocenteRepositorio
    {
        Task<ResultadoAcciones<E_Docente>> InsertarDocente(E_Docente docente);
        Task<ResultadoAcciones> BorrarDocente(int idDocente);
        Task<ResultadoAcciones<E_Docente>> ModificarDocente(E_Docente docente);
        Task<ResultadoAcciones<E_Docente>> ObtenerDocentePorId(int idDocente);
        Task<IEnumerable<E_Docente>> ObtenerTodosLosDocentes();
        Task<IEnumerable<E_Docente?>> ObtenerDocentePorCriterio(string criterio);
        
        // Métodos de validación
        Task<bool> ExisteNumeroEmpleado(string numeroEmpleado, int? idExcluido = null);
        Task<bool> ExisteIdDocente(int idDocente);
        Task<bool> ExisteEmailInstitucional(string emailInstitucional, int? idExcluido = null);
        Task<bool> ExisteCURP(string curp, int? idExcluido = null);
        Task<bool> ExisteRFC(string rfc, int? idExcluido = null);

        // Método específico para actualizar la URL de la foto
        Task<ResultadoAcciones> ActualizarUrlFoto(int idDocente, string urlFoto);
    }
} 