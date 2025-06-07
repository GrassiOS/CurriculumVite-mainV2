using System.Collections.Generic;
using System.Threading.Tasks;
using Entidades.Modelos.CurriculumVite;

namespace Servicios.IRepositorios.CurriculumVite
{
    public interface ISRepositorioDocumento
    {
        Task<IEnumerable<E_Documento>> GetAllAsync();
        Task<E_Documento> GetByIdAsync(int id);
        Task AddAsync(E_Documento entity);
        Task UpdateAsync(E_Documento entity);
        Task DeleteAsync(int id);
        Task<IEnumerable<E_Documento>> GetDocumentosByPublicacionAsync(int idPublicacion);
        Task<E_Documento> CreateDocumentoForPublicacionAsync(int idDocente, int idPublicacion, string titulo, string url, string descripcion);
        Task<E_Documento> CreateDocumentoForEducacionAsync(int idDocente, int idEducacion, string titulo, string url, string descripcion);
    }
}
