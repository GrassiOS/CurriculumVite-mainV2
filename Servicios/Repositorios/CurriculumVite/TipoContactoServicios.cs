using System.Collections.Generic;
using System.Threading.Tasks;
using Entidades.Modelos.CurriculumVite;
using Servicios.IRepositorios.CurriculumVite;   
using Datos.IRepositorios.CurriculumVite;          

namespace Servicios.Repositorios.CurriculumVite
{
    public class TipoContactoServicios : ISRepositorioTipoContacto
    {
        private readonly IRepositorioTipoContacto _repo;
        public TipoContactoServicios(IRepositorioTipoContacto repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<E_TipoContacto>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<E_TipoContacto> GetByIdAsync(int id)
        {
            return await _repo.GetByIdAsync(id);
        }

        public async Task AddAsync(E_TipoContacto entity)
        {
            await _repo.AddAsync(entity);
        }

        public async Task UpdateAsync(E_TipoContacto entity)
        {
            await _repo.UpdateAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            await _repo.DeleteAsync(id);
        }
    }
}
