using System.Collections.Generic;
using System.Threading.Tasks;
using Entidades.Modelos.CurriculumVite;

namespace Datos.IRepositorios.CurriculumVite
{
    public interface IRepositorioDistincion
    {
        Task<IEnumerable<E_Distincion>> GetAllAsync();
        Task<E_Distincion> GetByIdAsync(int id);
        Task AddAsync(E_Distincion entity);
        Task UpdateAsync(E_Distincion entity);
        Task DeleteAsync(int id);
    }
}
