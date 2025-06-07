using System.Collections.Generic;
using System.Threading.Tasks;
using Entidades.Modelos.CurriculumVite;

namespace Servicios.IRepositorios.CurriculumVite
{
    public interface ISRepositorioExperiencia
    {
        Task<IEnumerable<E_Experiencia>> GetAllAsync();
        Task<E_Experiencia> GetByIdAsync(int id);
        Task AddAsync(E_Experiencia entity);
        Task UpdateAsync(E_Experiencia entity);
        Task DeleteAsync(int id);
    }
}
