using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Entidades.Modelos.CurriculumVite;
using Datos.IRepositorios.CurriculumVite;

namespace Datos.Repositorios.CurriculumVite
{
    public class DistincionRepositorio : IRepositorioDistincion
    {
        private readonly ContextoBD _context;

        public DistincionRepositorio(ContextoBD context)
        {
            _context = context;
        }

        public async Task<IEnumerable<E_Distincion>> GetAllAsync()
        {
            return await _context.Distinciones.ToListAsync();
        }

        public async Task<E_Distincion> GetByIdAsync(int id)
        {
            return await _context.Distinciones.FindAsync(id);
        }

        public async Task AddAsync(E_Distincion entity)
        {
            await _context.Distinciones.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(E_Distincion entity)
        {
            _context.Distinciones.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Distinciones.FindAsync(id);
            if (entity != null)
            {
                _context.Distinciones.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
