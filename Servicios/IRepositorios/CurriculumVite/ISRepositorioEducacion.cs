using System.Collections.Generic;
using System.Threading.Tasks;
using Entidades.Modelos.CurriculumVite;

namespace Servicios.IRepositorios.CurriculumVite
{
    public interface ISRepositorioEducacion
    {
        Task<IEnumerable<E_Educacion>> GetAllAsync();
        Task<E_Educacion> GetByIdAsync(int id);
        Task AddAsync(E_Educacion entity);
        Task UpdateAsync(E_Educacion entity);
        Task DeleteAsync(int id);
    }
}
