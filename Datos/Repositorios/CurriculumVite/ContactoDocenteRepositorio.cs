using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Entidades.Modelos.CurriculumVite;
using Datos.IRepositorios.CurriculumVite;

namespace Datos.Repositorios.CurriculumVite
{
    public class ContactoDocenteRepositorio : IRepositorioContactoDocente
    {
        private readonly ContextoBD _context;

        public ContactoDocenteRepositorio(ContextoBD context)
        {
            _context = context;
        }

        public async Task<IEnumerable<E_ContactoDocente>> GetAllAsync()
        {
            return await _context.ContactoDocentes
                .AsNoTracking()
                .Include(c => c.TipoContacto)
                .Include(c => c.Docente)
                .ToListAsync();
        }

        public async Task<E_ContactoDocente> GetByIdAsync(int id)
        {
            return await _context.ContactoDocentes
                .Include(c => c.TipoContacto)
                .Include(c => c.Docente)
                .FirstOrDefaultAsync(c => c.IdContacto == id);
        }

        public async Task AddAsync(E_ContactoDocente entity)
        {
            await _context.ContactoDocentes.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(E_ContactoDocente entity)
        {
            try
            {
                // Verificar si la entidad ya está siendo rastreada
                var entidadRastreada = _context.ChangeTracker.Entries<E_ContactoDocente>()
                    .FirstOrDefault(e => e.Entity.IdContacto == entity.IdContacto);

                if (entidadRastreada != null)
                {
                    // Si ya está siendo rastreada, desvincularla
                    _context.Entry(entidadRastreada.Entity).State = EntityState.Detached;
                }

                // Verificar que el contacto existe
                var existe = await _context.ContactoDocentes
                    .AsNoTracking()
                    .AnyAsync(c => c.IdContacto == entity.IdContacto);

                if (!existe)
                {
                    throw new KeyNotFoundException($"No se encontró el contacto con ID {entity.IdContacto}");
                }

                // Actualizar la entidad
                _context.Entry(entity).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Verificar si la entidad aún existe
                var existe = await _context.ContactoDocentes
                    .AsNoTracking()
                    .AnyAsync(c => c.IdContacto == entity.IdContacto);

                if (!existe)
                {
                    throw new KeyNotFoundException($"No se encontró el contacto con ID {entity.IdContacto}");
                }
                throw;
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                // Buscar la entidad sin tracking para evitar problemas de concurrencia
                var entity = await _context.ContactoDocentes
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c => c.IdContacto == id);

                if (entity != null)
                {
                    // Verificar si ya está siendo rastreada
                    var entidadRastreada = _context.ChangeTracker.Entries<E_ContactoDocente>()
                        .FirstOrDefault(e => e.Entity.IdContacto == id);

                    if (entidadRastreada != null)
                    {
                        // Si ya está siendo rastreada, eliminarla directamente
                        _context.ContactoDocentes.Remove(entidadRastreada.Entity);
                    }
                    else
                    {
                        // Si no está siendo rastreada, adjuntarla y eliminarla
                        _context.ContactoDocentes.Attach(entity);
                        _context.ContactoDocentes.Remove(entity);
                    }

                    await _context.SaveChangesAsync();
                }
                else
                {
                    throw new KeyNotFoundException($"No se encontró el contacto con ID {id}");
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                // La entidad ya fue eliminada por otro proceso
                throw new InvalidOperationException($"El contacto con ID {id} ya fue eliminado por otro proceso");
            }
        }

        public async Task<IEnumerable<E_ContactoDocente>> GetContactosByDocenteIdAsync(int idDocente)
        {
            return await _context.ContactoDocentes
                .AsNoTracking()
                .Include(c => c.TipoContacto)
                .Where(c => c.IdDocente == idDocente)
                .OrderBy(c => c.TipoContacto.Nombre)
                .ToListAsync();
        }

        public async Task<bool> ExisteContactoConTipoParaDocenteAsync(int idDocente, int idTipoContacto)
        {
            return await _context.ContactoDocentes
                .AnyAsync(c => c.IdDocente == idDocente && c.IdTpoContacto == idTipoContacto);
        }
    }
}
