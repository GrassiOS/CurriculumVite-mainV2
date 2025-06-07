using System.Collections.Generic;
using System.Threading.Tasks;
using Entidades.Modelos.CurriculumVite;

namespace Datos.IRepositorios.CurriculumVite
{
    public interface IRepositorioProyecto
    {
        Task<IEnumerable<E_Proyecto>> GetAllAsync();
        Task<E_Proyecto> GetByIdAsync(int id);
        Task AddAsync(E_Proyecto entity);
        Task UpdateAsync(E_Proyecto entity);
        Task DeleteAsync(int id);
    }
}
