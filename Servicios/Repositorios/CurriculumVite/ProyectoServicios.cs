using System.Collections.Generic;
using System.Threading.Tasks;
using Entidades.Modelos.CurriculumVite;
using Servicios.IRepositorios.CurriculumVite;
using Datos.IRepositorios.CurriculumVite;

namespace Servicios.Repositorios.CurriculumVite
{
    public class ProyectoServicios : ISRepositorioProyecto
    {
        private readonly IRepositorioProyecto _repoDatos;

        public ProyectoServicios(IRepositorioProyecto repoDatos)
        {
            _repoDatos = repoDatos;
        }

        public async Task<IEnumerable<E_Proyecto>> GetAllAsync()
        {
            return await _repoDatos.GetAllAsync();
        }

        public async Task<E_Proyecto> GetByIdAsync(int id)
        {
            return await _repoDatos.GetByIdAsync(id);
        }

        public async Task AddAsync(E_Proyecto entity)
        {
            await _repoDatos.AddAsync(entity);
        }

        public async Task UpdateAsync(E_Proyecto entity)
        {
            await _repoDatos.UpdateAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            await _repoDatos.DeleteAsync(id);
        }
    }
}
