using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Entidades.Modelos.CurriculumVite;
using Datos.IRepositorios.CurriculumVite;

namespace Datos.Repositorios.CurriculumVite
{
    public class ExperienciaRepositorio : IRepositorioExperiencia
    {
        private readonly ContextoBD _context;

        public ExperienciaRepositorio(ContextoBD context)
        {
            _context = context;
        }

        public async Task<IEnumerable<E_Experiencia>> GetAllAsync()
        {
            return await _context.Experiencias.ToListAsync();
        }

        public async Task<E_Experiencia> GetByIdAsync(int id)
        {
            return await _context.Experiencias.FindAsync(id);
        }

        public async Task AddAsync(E_Experiencia entity)
        {
            await _context.Experiencias.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(E_Experiencia entity)
        {
            try
            {
                var existingEntity = await _context.Experiencias.FindAsync(entity.IdExperiencia);
                if (existingEntity != null)
                {
                    // Desconectar la entidad existente del contexto
                    _context.Entry(existingEntity).State = EntityState.Detached;
                    
                    // Adjuntar la nueva entidad y marcarla como modificada
                    _context.Entry(entity).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }
                else
                {
                    throw new KeyNotFoundException($"No se encontró la experiencia con ID {entity.IdExperiencia}");
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ExperienciaExists(entity.IdExperiencia))
                {
                    throw new KeyNotFoundException($"No se encontró la experiencia con ID {entity.IdExperiencia}");
                }
                throw;
            }
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Experiencias.FindAsync(id);
            if (entity != null)
            {
                _context.Experiencias.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        private async Task<bool> ExperienciaExists(int id)
        {
            return await _context.Experiencias.AnyAsync(e => e.IdExperiencia == id);
        }
    }
}