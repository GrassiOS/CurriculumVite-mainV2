using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Entidades.Modelos.CurriculumVite;
using Datos.IRepositorios.CurriculumVite;

namespace Datos.Repositorios.CurriculumVite
{
    public class PublicacionRepositorio : IRepositorioPublicacion
    {
        private readonly ContextoBD _context;

        public PublicacionRepositorio(ContextoBD context)
        {
            _context = context;
        }

        public async Task<IEnumerable<E_Publicacion>> GetAllAsync()
        {
            return await _context.Publicaciones.ToListAsync();
        }

        public async Task<E_Publicacion> GetByIdAsync(int id)
        {
            return await _context.Publicaciones.FindAsync(id);
        }

        public async Task AddAsync(E_Publicacion entity)
        {
            await _context.Publicaciones.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(E_Publicacion entity)
        {
            try
            {
                var existingEntity = await _context.Publicaciones.FindAsync(entity.IdPublicacion);
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
                if (!await PublicacionExists(entity.IdPublicacion))
                {
                    throw new KeyNotFoundException($"No se encontró la publicación con ID {entity.IdPublicacion}");
                }
                throw;
            }
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Publicaciones.FindAsync(id);
            if (entity != null)
            {
                _context.Publicaciones.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        private async Task<bool> PublicacionExists(int id)
        {
            return await _context.Publicaciones.AnyAsync(e => e.IdPublicacion == id);
        }
    }
}
