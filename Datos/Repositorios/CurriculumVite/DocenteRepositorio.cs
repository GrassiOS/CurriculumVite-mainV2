using Entidades.Generales;
using Microsoft.EntityFrameworkCore;
using Datos.IRepositorios.CurriculumVite;
using Entidades.Modelos.CurriculumVite;

namespace Datos.Repositorios.CurriculumVite
{
    public class DocenteRepositorio(ContextoBD contextoBD) : IDocenteRepositorio
    {
        private readonly ContextoBD _contextoBD = contextoBD;

        public async Task<ResultadoAcciones<E_Docente>> InsertarDocente(E_Docente docente)
        {
            using var transaction = await _contextoBD.Database.BeginTransactionAsync();
            try
            {
                // Obtener el siguiente ID manualmente ya que la tabla no tiene IDENTITY
                var maxId = await _contextoBD.Docentes.MaxAsync(d => (int?)d.IdDocente) ?? 0;
                docente.IdDocente = maxId + 1;
                
                await _contextoBD.Docentes.AddAsync(docente);
                await _contextoBD.SaveChangesAsync();
                await transaction.CommitAsync();

                return new ResultadoAcciones<E_Docente>
                {
                    Entidad = docente,
                    Mensajes = { "El docente fue agregado correctamente." },
                    Resultado = true
                };
            }
            catch (DbUpdateException ex)
            {
                await transaction.RollbackAsync();
                return new ResultadoAcciones<E_Docente>
                {
                    Resultado = false,
                    Mensajes = {
                        "Error al guardar docente en la base de datos.",
                        ex.InnerException?.Message ?? ex.Message
                    }
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return new ResultadoAcciones<E_Docente>
                {
                    Resultado = false,
                    Mensajes = { "Error inesperado: " + ex.Message + "." }
                };
            }
        }

        public async Task<ResultadoAcciones> BorrarDocente(int idDocente)
        {
            using var transaction = await _contextoBD.Database.BeginTransactionAsync();
            try
            {
                var docenteBD = await _contextoBD.Docentes.FindAsync(idDocente);

                if (docenteBD == null)
                {
                    return new ResultadoAcciones
                    {
                        Resultado = false,
                        Mensajes = { $"El docente con identificador {idDocente} no fue encontrado." }
                    };
                }

                _contextoBD.Remove(docenteBD);
                await _contextoBD.SaveChangesAsync();
                await transaction.CommitAsync();

                return new ResultadoAcciones 
                { 
                    Resultado = true,
                    Mensajes = { "El docente fue eliminado correctamente." }
                };
            }
            catch (DbUpdateException ex)
            {
                await transaction.RollbackAsync();
                return new ResultadoAcciones
                {
                    Resultado = false,
                    Mensajes = {
                        "Error al tratar de borrar el docente de la base de datos.",
                        ex.InnerException?.Message ?? ex.Message
                    }
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return new ResultadoAcciones
                {
                    Resultado = false,
                    Mensajes = { "Error inesperado: " + ex.Message + "." }
                };
            }
        }

        public async Task<ResultadoAcciones<E_Docente>> ModificarDocente(E_Docente docente)
        {
            using var transaction = await _contextoBD.Database.BeginTransactionAsync();
            try
            {
                // Verificar si la entidad ya está siendo rastreada
                var entidadRastreada = _contextoBD.ChangeTracker.Entries<E_Docente>()
                    .FirstOrDefault(e => e.Entity.IdDocente == docente.IdDocente);

                if (entidadRastreada != null)
                {
                    // Si ya está siendo rastreada, desvincularla
                    _contextoBD.Entry(entidadRastreada.Entity).State = EntityState.Detached;
                }

                // Verificar que el docente existe
                var existe = await _contextoBD.Docentes
                    .AsNoTracking()
                    .AnyAsync(d => d.IdDocente == docente.IdDocente);

                if (!existe)
                {
                    return new ResultadoAcciones<E_Docente>
                    {
                        Resultado = false,
                        Mensajes = { "El docente que se quiere modificar no existe." }
                    };
                }

                // Actualizar la entidad usando Update
                _contextoBD.Docentes.Update(docente);
                await _contextoBD.SaveChangesAsync();
                await transaction.CommitAsync();

                return new ResultadoAcciones<E_Docente>
                {
                    Entidad = docente,
                    Mensajes = { "El docente fue modificado correctamente." },
                    Resultado = true
                };
            }
            catch (DbUpdateException ex)
            {
                await transaction.RollbackAsync();
                return new ResultadoAcciones<E_Docente>
                {
                    Resultado = false,
                    Mensajes = {
                        "Error al actualizar el docente en la base de datos.",
                        ex.InnerException?.Message ?? ex.Message
                    }
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return new ResultadoAcciones<E_Docente>
                {
                    Resultado = false,
                    Mensajes = { "Error inesperado: " + ex.Message }
                };
            }
        }

        public async Task<ResultadoAcciones<E_Docente>> ObtenerDocentePorId(int idDocente)
        {
            try
            {
                var docente = await _contextoBD.Docentes
                    .AsNoTracking()
                    .Include(d => d.Sexo)
                    .Include(d => d.EstadoCivil)
                    .Include(d => d.Categoria)
                    .Include(d => d.Nombramiento)
                    .Include(d => d.Escolaridad)
                    .Include(d => d.NivelSNI)
                    .Include(d => d.PRODEP)
                    .Include(d => d.Carrera)
                    .Include(d => d.CuerpoAcademico)
                    .FirstOrDefaultAsync(d => d.IdDocente == idDocente);

                if (docente == null)
                {
                    return new ResultadoAcciones<E_Docente>
                    {
                        Resultado = false,
                        Mensajes = { $"No se encontró el docente con el identificador {idDocente}." }
                    };
                }

                return new ResultadoAcciones<E_Docente>
                {
                    Entidad = docente,
                    Resultado = true
                };
            }
            catch (Exception ex)
            {
                return new ResultadoAcciones<E_Docente>
                {
                    Resultado = false,
                    Mensajes = { "Error al obtener docente: " + ex.Message + "." }
                };
            }
        }

        public async Task<IEnumerable<E_Docente>> ObtenerTodosLosDocentes()
        {
            return await _contextoBD.Docentes
                .AsNoTracking()
                .Include(d => d.Sexo)
                .Include(d => d.EstadoCivil)
                .Include(d => d.Categoria)
                .Include(d => d.Nombramiento)
                .Include(d => d.Escolaridad)
                .Include(d => d.NivelSNI)
                .Include(d => d.PRODEP)
                .Include(d => d.Carrera)
                .Include(d => d.CuerpoAcademico)
                .OrderBy(d => d.NombreDocente)
                .ThenBy(d => d.PaternoDocente)
                .ToListAsync();
        }

        public async Task<IEnumerable<E_Docente?>> ObtenerDocentePorCriterio(string criterio)
        {
            criterio = criterio?.ToLower() ?? string.Empty;

            return await _contextoBD.Docentes
                .AsNoTracking()
                .Include(d => d.Sexo)
                .Include(d => d.EstadoCivil)
                .Include(d => d.Categoria)
                .Include(d => d.Nombramiento)
                .Include(d => d.Escolaridad)
                .Include(d => d.NivelSNI)
                .Include(d => d.PRODEP)
                .Include(d => d.Carrera)
                .Include(d => d.CuerpoAcademico)
                .Where(d => d.NumeroEmpleado.ToLower().Contains(criterio) ||
                           d.NombreDocente.ToLower().Contains(criterio) ||
                           (d.PaternoDocente != null && d.PaternoDocente.ToLower().Contains(criterio)) ||
                           (d.MaternoDocente != null && d.MaternoDocente.ToLower().Contains(criterio)) ||
                           (d.EmailInstitucional != null && d.EmailInstitucional.ToLower().Contains(criterio)) ||
                           (d.RFC != null && d.RFC.ToLower().Contains(criterio)) ||
                           (d.CURP != null && d.CURP.ToLower().Contains(criterio)))
                .OrderBy(d => d.NombreDocente)
                .ThenBy(d => d.PaternoDocente)
                .ToListAsync();
        }

        public async Task<bool> ExisteNumeroEmpleado(string numeroEmpleado, int? idExcluido = null)
        {
            var query = _contextoBD.Docentes.Where(d => d.NumeroEmpleado == numeroEmpleado);

            if (idExcluido.HasValue)
            {
                query = query.Where(d => d.IdDocente != idExcluido.Value);
            }

            return await query.AnyAsync();
        }

        public async Task<bool> ExisteIdDocente(int idDocente)
        {
            return await _contextoBD.Docentes
                .AnyAsync(d => d.IdDocente == idDocente);
        }

        public async Task<bool> ExisteEmailInstitucional(string emailInstitucional, int? idExcluido = null)
        {
            if (string.IsNullOrEmpty(emailInstitucional))
                return false;

            var query = _contextoBD.Docentes.Where(d => d.EmailInstitucional == emailInstitucional);

            if (idExcluido.HasValue)
            {
                query = query.Where(d => d.IdDocente != idExcluido.Value);
            }

            return await query.AnyAsync();
        }

        public async Task<bool> ExisteCURP(string curp, int? idExcluido = null)
        {
            if (string.IsNullOrEmpty(curp))
                return false;

            var query = _contextoBD.Docentes.Where(d => d.CURP == curp);

            if (idExcluido.HasValue)
            {
                query = query.Where(d => d.IdDocente != idExcluido.Value);
            }

            return await query.AnyAsync();
        }

        public async Task<bool> ExisteRFC(string rfc, int? idExcluido = null)
        {
            if (string.IsNullOrEmpty(rfc))
                return false;

            var query = _contextoBD.Docentes.Where(d => d.RFC == rfc);

            if (idExcluido.HasValue)
            {
                query = query.Where(d => d.IdDocente != idExcluido.Value);
            }

            return await query.AnyAsync();
        }

        public async Task<ResultadoAcciones> ActualizarUrlFoto(int idDocente, string urlFoto)
        {
            using var transaction = await _contextoBD.Database.BeginTransactionAsync();
            try
            {
                var docente = await _contextoBD.Docentes.FindAsync(idDocente);
                if (docente == null)
                {
                    return new ResultadoAcciones
                    {
                        Resultado = false,
                        Mensajes = { $"No se encontró el docente con el identificador {idDocente}." }
                    };
                }

                docente.URLFoto = urlFoto;
                await _contextoBD.SaveChangesAsync();
                await transaction.CommitAsync();

                return new ResultadoAcciones
                {
                    Resultado = true,
                    Mensajes = { "La URL de la foto fue actualizada correctamente." }
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return new ResultadoAcciones
                {
                    Resultado = false,
                    Mensajes = { "Error al actualizar la URL de la foto: " + ex.Message }
                };
            }
        }
    }
} 