using System.Collections.Generic;
using System.Threading.Tasks;
using Entidades.Modelos.CurriculumVite;

namespace Datos.IRepositorios.CurriculumVite
{
    public interface IRepositorioEducacion
    {
        Task<IEnumerable<E_Educacion>> GetAllAsync();
        Task<E_Educacion> GetByIdAsync(int id);
        Task AddAsync(E_Educacion entity);
        Task UpdateAsync(E_Educacion entity);
        Task DeleteAsync(int id);
    }
}
