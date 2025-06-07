using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Entidades.Modelos.CurriculumVite;
using Datos.IRepositorios.CurriculumVite;

namespace Datos.Repositorios.CurriculumVite
{
    public class ProyectoRepositorio : IRepositorioProyecto
    {
        private readonly ContextoBD _context;

        public ProyectoRepositorio(ContextoBD context)
        {
            _context = context;
        }

        public async Task<IEnumerable<E_Proyecto>> GetAllAsync()
        {
            return await _context.Proyectos.ToListAsync();
        }

        public async Task<E_Proyecto> GetByIdAsync(int id)
        {
            return await _context.Proyectos.FindAsync(id);
        }

        public async Task AddAsync(E_Proyecto entity)
        {
            await _context.Proyectos.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(E_Proyecto entity)
        {
            try
            {
                var existingEntity = await _context.Proyectos.FindAsync(entity.IdProyecto);
                if (existingEntity != null)
                {
                    // Desconectar la entidad existente del contexto
                    _context.Entry(existingEntity).State = EntityState.Detached;
                }

                // Adjuntar la nueva entidad y marcarla como modificada
                _context.Entry(entity).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ProyectoExists(entity.IdProyecto))
                {
                    throw new KeyNotFoundException($"No se encontró el proyecto con ID {entity.IdProyecto}");
                }
                throw;
            }
        }

        private async Task<bool> ProyectoExists(int id)
        {
            return await _context.Proyectos.AnyAsync(e => e.IdProyecto == id);
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Proyectos.FindAsync(id);
            if (entity != null)
            {
                _context.Proyectos.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}