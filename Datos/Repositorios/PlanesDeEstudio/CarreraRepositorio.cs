using Entidades.Generales;
using Microsoft.EntityFrameworkCore;
using Datos.IRepositorios.PlnesDeEstudio;
using Entidades.Modelos.PlanesDeEstudio.Carreras;

namespace Datos.Repositorios.PlanesDeEstudio
{
  public class CarreraRepositorio(ContextoBD contextoBD) : ICarreraRepositorio
  {
    private readonly ContextoBD _contextoBD = contextoBD;

    public async Task<ResultadoAcciones<E_Carrera>> InsertarCarrera(E_Carrera carrera)
    {
      using var transaction = await _contextoBD.Database.BeginTransactionAsync();
      try
      {
        await _contextoBD.CarrerasPlanEstudio.AddAsync(carrera);
        await _contextoBD.SaveChangesAsync();
        await transaction.CommitAsync();

        return new ResultadoAcciones<E_Carrera>
        {
          Entidad = carrera,
          Mensajes = { "La carrera fue agragada correctamentes." },
          Resultado = true
        };
      }
      catch (DbUpdateException ex)
      {
        await transaction.RollbackAsync();
        return new ResultadoAcciones<E_Carrera>
        {
          Resultado = false,
          Mensajes = {
                        "Error al guardar carrera en la base de datos.",
                        ex.InnerException?.Message ?? ex.Message
                    }
        };
      }
      catch (Exception ex)
      {
        await transaction.RollbackAsync();
        return new ResultadoAcciones<E_Carrera>
        {
          Resultado = false,
          Mensajes = { "Error inesperado: " + ex.Message + "." }
        };
      }
    }
    public async Task<ResultadoAcciones> BorrarCarrera(int idCarrera)
    {
      using var transaction = await _contextoBD.Database.BeginTransactionAsync();
      try
      {
        var carreraBD = await _contextoBD.CarrerasPlanEstudio.FindAsync(idCarrera);

        if (carreraBD == null)
        {
          return new ResultadoAcciones
          {
            Resultado = false,
            Mensajes = { $"La carrera con identificador {idCarrera} no encontrada." }
          };
        }

        _contextoBD.Remove(carreraBD);
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
                        "Error al tratar de borrar la carrera de base de datos.",
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
    public async Task<ResultadoAcciones<E_Carrera>> ModificarCarrera(E_Carrera carrera)
    {
      using var transaction = await _contextoBD.Database.BeginTransactionAsync();
      try
      {
        var carreraEnBD = await _contextoBD.CarrerasPlanEstudio.FirstOrDefaultAsync(c => c.IdCarrera == carrera.IdCarrera);

        if (carreraEnBD == null)
        {
          return new ResultadoAcciones<E_Carrera>
          {
            Resultado = false,
            Mensajes = { "La carrera que se quiere borrar no existe." }
          };
        }

        _contextoBD.Entry(carreraEnBD).CurrentValues.SetValues(carrera);
        await _contextoBD.SaveChangesAsync();
        await transaction.CommitAsync();

        return new ResultadoAcciones<E_Carrera>
        {
          Entidad = carreraEnBD,
          Resultado = true
        };
      }
      catch (DbUpdateException ex)
      {
        await transaction.RollbackAsync();
        return new ResultadoAcciones<E_Carrera>
        {
          Resultado = false,
          Mensajes = {
                        "Error al actualizar la carrera en la base de datos.",
                        ex.InnerException?.Message ?? ex.Message
                    }
        };
      }
      catch (Exception ex)
      {
        await transaction.RollbackAsync();
        return new ResultadoAcciones<E_Carrera>
        {
          Resultado = false,
          Mensajes = { "Error inesperado: " + ex.Message }
        };
      }
    }

    public async Task<ResultadoAcciones<E_Carrera>> ObtenerCarreraPorId(int idCarrera)
    {
      try
      {
        var carrera = await _contextoBD.CarrerasPlanEstudio
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.IdCarrera == idCarrera);

        if (carrera == null)
        {
          return new ResultadoAcciones<E_Carrera>
          {
            Resultado = false,
            Mensajes = { $"No se encontró la carrera con el identificador {idCarrera}." }
          };
        }

        return new ResultadoAcciones<E_Carrera>
        {
          Entidad = carrera,
          Resultado = true
        };
      }
      catch (Exception ex)
      {
        return new ResultadoAcciones<E_Carrera>
        {
          Resultado = false,
          Mensajes = { "Error al obtener carrera: " + ex.Message + "." }
        };
      }
    }
    public async Task<IEnumerable<E_Carrera>> ObtenerTodasLasCarreras()
    {
      return await _contextoBD.CarrerasPlanEstudio
          .AsNoTracking()
          .OrderBy(c => c.ClaveCarrera)
          .ToListAsync();
    }
    public async Task<IEnumerable<E_Carrera?>> ObtenerCarreraPorCriterio(string criterio)
    {
      criterio = criterio?.ToLower() ?? string.Empty;

      return await _contextoBD.CarrerasPlanEstudio
          .Where(c => c.ClaveCarrera.ToLower().Contains(criterio) ||
                       c.NombreCarrera.ToLower().Contains(criterio))
          .OrderBy(c => c.ClaveCarrera)
          .ToListAsync();
    }


    public async Task<bool> ExisteClaveCarrera(string claveCarrera, int? idExcluido = null)
    {
      var carrera = _contextoBD.CarrerasPlanEstudio.Where(c => c.ClaveCarrera == claveCarrera);

      if (idExcluido.HasValue)
      {
        carrera = carrera.Where(c => c.IdCarrera != idExcluido.Value);
      }

      return await carrera.AnyAsync();
    }

    public async Task<bool> ExisteIdCarrera(int idCarrera)
    {
      return await _contextoBD.CarrerasPlanEstudio
          .AnyAsync(c => c.IdCarrera == idCarrera);
    }

    public async Task<bool> ExisteNombreCarrera(string nombreCarrera, int? idExcluido = null)
    {
      var carrera = _contextoBD.CarrerasPlanEstudio.Where(c => c.NombreCarrera == nombreCarrera);

      if (idExcluido.HasValue)
      {
        carrera = carrera.Where(c => c.IdCarrera != idExcluido.Value);
      }

      return await carrera.AnyAsync();
    }

    public async Task<bool> ExisteAliasCarrera(string aliasCarrera, int? idExcluido = null)
    {
      var carrera = _contextoBD.CarrerasPlanEstudio.Where(c => c.AliasCarrera == aliasCarrera);

      if (idExcluido.HasValue)
      {
        carrera = carrera.Where(c => c.IdCarrera != idExcluido.Value);
      }

      return await carrera.AnyAsync();
    }
  }
}