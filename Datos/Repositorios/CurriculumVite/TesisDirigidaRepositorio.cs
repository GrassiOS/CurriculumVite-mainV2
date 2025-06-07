using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Entidades.Modelos.CurriculumVite;
using Datos.IRepositorios.CurriculumVite;

namespace Datos.Repositorios.CurriculumVite
{
    public class TesisDirigidaRepositorio : IRepositorioTesisDirigida
    {
        private readonly ContextoBD _context;

        public TesisDirigidaRepositorio(ContextoBD context)
        {
            _context = context;
        }

        public async Task<IEnumerable<E_TesisDirigida>> GetAllAsync()
        {
            return await _context.TesisDirigidas.ToListAsync();
        }

        public async Task<E_TesisDirigida> GetByIdAsync(int id)
        {
            return await _context.TesisDirigidas.FindAsync(id);
        }

        public async Task AddAsync(E_TesisDirigida entity)
        {
            await _context.TesisDirigidas.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(E_TesisDirigida entity)
        {
            _context.TesisDirigidas.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.TesisDirigidas.FindAsync(id);
            if (entity != null)
            {
                _context.TesisDirigidas.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
