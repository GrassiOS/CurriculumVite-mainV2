using Entidades.Generales;
using Microsoft.EntityFrameworkCore;
using Datos.IRepositorios.PlnesDeEstudio;
using Entidades.Modelos.PlanesDeEstudio.Materias;

namespace Datos.Repositorios.PlanesDeEstudio
{
  public class MateriaRepositorio(ContextoBD contextoBD) : IMateriaRepositorio
  {
    private readonly ContextoBD _contextoBD = contextoBD;

    public async Task<ResultadoAcciones<E_Materia>> InsertarMateria(E_Materia materia)
    {
      using var transaction = await _contextoBD.Database.BeginTransactionAsync();
      try
      {
        await _contextoBD.Materias.AddAsync(materia);
        await _contextoBD.SaveChangesAsync();
        await transaction.CommitAsync();

        return new ResultadoAcciones<E_Materia>
        {
          Entidad = materia,
          Mensajes = { "La materia fue agragada correctamentes." },
          Resultado = true
        };
      }
      catch (DbUpdateException ex)
      {
        await transaction.RollbackAsync();
        return new ResultadoAcciones<E_Materia>
        {
          Resultado = false,
          Mensajes = { "Error al guardar materia en la base de datos.",  ex.InnerException?.Message ?? ex.Message }
        };
      }
      catch (Exception ex)
      {
        await transaction.RollbackAsync();
        return new ResultadoAcciones<E_Materia>
        {
          Resultado = false,
          Mensajes = { "Error inesperado: " + ex.Message + "." }
        };
      }
    }
    // todo: Falta validar este método
    public async Task<ResultadoAcciones> BorrarMateria(int idMateria)
    {
      using var transaction = await _contextoBD.Database.BeginTransactionAsync();
      try
      {
        var materiaEnBD = await _contextoBD.Materias.FindAsync(idMateria);

        if (materiaEnBD == null)
        {
          return new ResultadoAcciones
          {
            Resultado = false,
            Mensajes = { $"La materia con identificador {idMateria} no encontrada." }
          };
        }

        _contextoBD.Remove(materiaEnBD);
        await _contextoBD.SaveChangesAsync();
        await transaction.CommitAsync();

        return new ResultadoAcciones { Resultado = true };
      }
      catch (DbUpdateException ex)
      {
        await transaction.RollbackAsync();
        return new ResultadoAcciones
        {
          Resultado = false,
          Mensajes = {
                        "Error al tratar de borrar la materia de base de datos.",
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
    public async Task<ResultadoAcciones<E_Materia>> ModificarMateria(E_Materia materia)
    {
      using var transaction = await _contextoBD.Database.BeginTransactionAsync();
      try
      {
        var materiaEnBD = await _contextoBD.Materias.FirstOrDefaultAsync(m => m.IdMateria == materia.IdMateria);

        if (materiaEnBD == null)
        {
          return new ResultadoAcciones<E_Materia>
          {
            Resultado = false,
            Mensajes = { "La materia que se quiere borrar no existe." }
          };
        }

        _contextoBD.Entry(materiaEnBD).CurrentValues.SetValues(materia);
        await _contextoBD.SaveChangesAsync();
        await transaction.CommitAsync();

        return new ResultadoAcciones<E_Materia>
        {
          Entidad = materiaEnBD,
          Resultado = true
        };
      }
      catch (DbUpdateException ex)
      {
        await transaction.RollbackAsync();
        return new ResultadoAcciones<E_Materia>
        {
          Resultado = false,
          Mensajes = {
                        "Error al actualizar la materia en la base de datos.",
                        ex.InnerException?.Message ?? ex.Message
                    }
        };
      }
      catch (Exception ex)
      {
        await transaction.RollbackAsync();
        return new ResultadoAcciones<E_Materia>
        {
          Resultado = false,
          Mensajes = { "Error inesperado: " + ex.Message }
        };
      }
    }

    public async Task<ResultadoAcciones<E_Materia>> ObtenerMateriaPorId(int idMateria)
    {
      try
      {
        var materia = await _contextoBD.Materias
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.IdMateria == idMateria);

        if (materia == null)
        {
          return new ResultadoAcciones<E_Materia>
          {
            Resultado = false,
            Mensajes = { $"No se encontró la materia con el identificador {idMateria}." }
          };
        }

        return new ResultadoAcciones<E_Materia>
        {
          Entidad = materia,
          Resultado = true
        };
      }
      catch (Exception ex)
      {
        return new ResultadoAcciones<E_Materia>
        {
          Resultado = false,
          Mensajes = { "Error al obtener materia: " + ex.Message + "." }
        };
      }
    }
    public async Task<IEnumerable<E_Materia>> ObtenerTodasLasMaterias()
    {
      return await _contextoBD.Materias
         .AsNoTracking()
         .OrderBy(m => m.ClaveMateria)
         .ToListAsync();
    }
    public async Task<IEnumerable<E_Materia>> ObtenerMateriaPorCriterio(string criterioBusqueda)
    {
      return await _contextoBD.Materias
          .Where(m => m.ClaveMateria.ToLower().Contains(criterioBusqueda) ||
                      m.NombreMateria.ToLower().Contains(criterioBusqueda))
          .OrderBy(m => m.NombreMateria)
          .ToListAsync();
    }
    
    public async Task<bool> ExisteIdMateria(int idMateria)
    {
      if (idMateria < 0)
        return false;

      return await _contextoBD.Materias.AsNoTracking().AnyAsync(c => c.IdMateria == idMateria);
    }
    public async Task<bool> ExisteClaveMateria(string claveMateria, int? idExcluido = null)
    {
      var materia = _contextoBD.Materias.Where(m => m.ClaveMateria == claveMateria);

      if (idExcluido.HasValue)
      {
        materia = materia.Where(m => m.IdMateria != idExcluido.Value);
      }

      return await materia.AnyAsync();
    }    
  }
}