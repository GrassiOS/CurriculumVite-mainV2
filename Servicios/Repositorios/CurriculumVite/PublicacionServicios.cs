using System.Collections.Generic;
using System.Threading.Tasks;
using Entidades.Modelos.CurriculumVite;
using Servicios.IRepositorios.CurriculumVite;
using Datos.IRepositorios.CurriculumVite;

namespace Servicios.Repositorios.CurriculumVite
{
    public class PublicacionServicios : ISRepositorioPublicacion
    {
        private readonly IRepositorioPublicacion _repo;

        public PublicacionServicios(IRepositorioPublicacion repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<E_Publicacion>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<E_Publicacion> GetByIdAsync(int id)
        {
            return await _repo.GetByIdAsync(id);
        }

        public async Task AddAsync(E_Publicacion entity)
        {
            await _repo.AddAsync(entity);
        }

        public async Task UpdateAsync(E_Publicacion entity)
        {
            await _repo.UpdateAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            await _repo.DeleteAsync(id);
        }
    }
}
