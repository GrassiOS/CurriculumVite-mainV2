using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Entidades.Modelos.CurriculumVite;
using Datos.IRepositorios.CurriculumVite;

namespace Datos.Repositorios.CurriculumVite
{
    public class TipoContactoRepositorio : IRepositorioTipoContacto
    {
        private readonly ContextoBD _context;

        public TipoContactoRepositorio(ContextoBD context)
        {
            _context = context;
        }

        public async Task<IEnumerable<E_TipoContacto>> GetAllAsync()
        {
            return await _context.TipoContactos.ToListAsync();
        }

        public async Task<E_TipoContacto> GetByIdAsync(int id)
        {
            return await _context.TipoContactos.FindAsync(id);
        }

        public async Task AddAsync(E_TipoContacto entity)
        {
            await _context.TipoContactos.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(E_TipoContacto entity)
        {
            _context.TipoContactos.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.TipoContactos.FindAsync(id);
            if (entity != null)
            {
                _context.TipoContactos.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
