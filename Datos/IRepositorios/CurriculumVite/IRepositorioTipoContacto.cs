using System.Collections.Generic;
using System.Threading.Tasks;
using Entidades.Modelos.CurriculumVite;

namespace Datos.IRepositorios.CurriculumVite
{
    public interface IRepositorioTipoContacto
    {
        Task<IEnumerable<E_TipoContacto>> GetAllAsync();
        Task<E_TipoContacto> GetByIdAsync(int id);
        Task AddAsync(E_TipoContacto entity);
        Task UpdateAsync(E_TipoContacto entity);
        Task DeleteAsync(int id);
    }
}
