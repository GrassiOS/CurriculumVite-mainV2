using System.Collections.Generic;
using System.Threading.Tasks;
using Entidades.Modelos.CurriculumVite;
using Servicios.IRepositorios.CurriculumVite;
using Datos.IRepositorios.CurriculumVite;

namespace Servicios.Repositorios.CurriculumVite
{
    public class ContactoDocenteServicios : ISRepositorioContactoDocente
    {
        private readonly IRepositorioContactoDocente _repoDatos;

        public ContactoDocenteServicios(IRepositorioContactoDocente repoDatos)
        {
            _repoDatos = repoDatos;
        }

        public async Task<IEnumerable<E_ContactoDocente>> GetAllAsync()
        {
            return await _repoDatos.GetAllAsync();
        }

        public async Task<E_ContactoDocente> GetByIdAsync(int id)
        {
            return await _repoDatos.GetByIdAsync(id);
        }

        public async Task AddAsync(E_ContactoDocente entity)
        {
            await _repoDatos.AddAsync(entity);
        }

        public async Task UpdateAsync(E_ContactoDocente entity)
        {
            await _repoDatos.UpdateAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            await _repoDatos.DeleteAsync(id);
        }

        public async Task<IEnumerable<E_ContactoDocente>> GetContactosByDocenteIdAsync(int idDocente)
        {
            return await _repoDatos.GetContactosByDocenteIdAsync(idDocente);
        }

        public async Task<bool> ExisteContactoConTipoParaDocenteAsync(int idDocente, int idTipoContacto)
        {
            return await _repoDatos.ExisteContactoConTipoParaDocenteAsync(idDocente, idTipoContacto);
        }
    }
}