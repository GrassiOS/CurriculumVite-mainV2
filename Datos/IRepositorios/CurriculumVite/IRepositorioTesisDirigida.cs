using System.Collections.Generic;
using System.Threading.Tasks;
using Entidades.Modelos.CurriculumVite;

namespace Datos.IRepositorios.CurriculumVite
{
    public interface IRepositorioTesisDirigida
    {
        Task<IEnumerable<E_TesisDirigida>> GetAllAsync();
        Task<E_TesisDirigida> GetByIdAsync(int id);
        Task AddAsync(E_TesisDirigida entity);
        Task UpdateAsync(E_TesisDirigida entity);
        Task DeleteAsync(int id);
    }
}
