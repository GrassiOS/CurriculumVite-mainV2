using System.Collections.Generic;
using System.Threading.Tasks;
using Entidades.Modelos.CurriculumVite;

namespace Servicios.IRepositorios.CurriculumVite
{
    public interface ISRepositorioProyecto
    {
        Task<IEnumerable<E_Proyecto>> GetAllAsync();
        Task<E_Proyecto> GetByIdAsync(int id);
        Task AddAsync(E_Proyecto entity);
        Task UpdateAsync(E_Proyecto entity);
        Task DeleteAsync(int id);
    }
}
