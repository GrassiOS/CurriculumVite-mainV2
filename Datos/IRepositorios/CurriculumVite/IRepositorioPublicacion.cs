using System.Collections.Generic;
using System.Threading.Tasks;
using Entidades.Modelos.CurriculumVite;

namespace Datos.IRepositorios.CurriculumVite
{
    public interface IRepositorioPublicacion
    {
        Task<IEnumerable<E_Publicacion>> GetAllAsync();
        Task<E_Publicacion> GetByIdAsync(int id);
        Task AddAsync(E_Publicacion entity);
        Task UpdateAsync(E_Publicacion entity);
        Task DeleteAsync(int id);
    }
}
