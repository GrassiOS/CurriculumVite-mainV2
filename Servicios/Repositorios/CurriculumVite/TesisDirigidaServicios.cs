using System.Collections.Generic;
using System.Threading.Tasks;
using Entidades.Modelos.CurriculumVite;
using Servicios.IRepositorios.CurriculumVite;
using Datos.IRepositorios.CurriculumVite;

namespace Servicios.Repositorios.CurriculumVite
{
    public class TesisDirigidaServicios : ISRepositorioTesisDirigida
    {
        private readonly IRepositorioTesisDirigida _repo;

        public TesisDirigidaServicios(IRepositorioTesisDirigida repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<E_TesisDirigida>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<E_TesisDirigida> GetByIdAsync(int id)
        {
            return await _repo.GetByIdAsync(id);
        }

        public async Task AddAsync(E_TesisDirigida entity)
        {
            await _repo.AddAsync(entity);
        }

        public async Task UpdateAsync(E_TesisDirigida entity)
        {
            await _repo.UpdateAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            await _repo.DeleteAsync(id);
        }
    }
}
