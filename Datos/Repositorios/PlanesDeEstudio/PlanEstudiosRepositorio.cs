using Entidades.Generales;
using Microsoft.EntityFrameworkCore;
using Datos.IRepositorios.PlnesDeEstudio;
using Entidades.DTO.PlanesDeEstudio.PlanEstudios;
using Entidades.Modelos.PlanesDeEstudio.PlanEstudios;

namespace Datos.Repositorios.PlanesDeEstudio
{
  public class PlanEstudioRepositorio(ContextoBD contextoBD) : IPlanEstudioRepositorio
  {
    private readonly ContextoBD _contextoBD = contextoBD;

    public async Task<ResultadoAcciones<E_PlanEstudio>> InsertarPlanEstudio(E_PlanEstudio planEstudio)
    {

      using var transaction = await _contextoBD.Database.BeginTransactionAsync();
      try
      {
        await _contextoBD.PlanEstudios.AddAsync(planEstudio);
        await _contextoBD.SaveChangesAsync();
        await transaction.CommitAsync();

        return new ResultadoAcciones<E_PlanEstudio>
        {
          Entidad = planEstudio,
          Mensajes = { "El plan de estudio fue registrado correctamente." },
          Resultado = true
        };
      }
      catch (DbUpdateException ex)
      {
        await transaction.RollbackAsync();
        return new ResultadoAcciones<E_PlanEstudio>
        {
          Resultado = false,
          Mensajes = { "Error al guardar plan de estudio en la base de datos.", ex.InnerException?.Message ?? ex.Message }
        };
      }
      catch (Exception ex)
      {
        await transaction.RollbackAsync();
        return new ResultadoAcciones<E_PlanEstudio>
        {
          Resultado = false,
          Mensajes = { "Error inesperado: " + ex.Message + "." }
        };
      }
    }
    // todo: Falta validar este método
    public async Task<ResultadoAcciones> BorrarPlanEstudio(int idPlanEstudio)
    {
      using var transaction = await _contextoBD.Database.BeginTransactionAsync();
      try
      {
        var planEstudioEnBD = await _contextoBD.PlanEstudios.FindAsync(idPlanEstudio);

        if (planEstudioEnBD == null)
        {
          return new ResultadoAcciones
          {
            Resultado = false,
            Mensajes = { $"El plan de estudio con identificador {idPlanEstudio} no fue encontrado." }
          };
        }

        _contextoBD.Remove(planEstudioEnBD);
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
                        "Error al tratar de borrar el plan de estudio de la base de datos.",
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
    public async Task<ResultadoAcciones<E_PlanEstudio>> ModificarPlanEstudio(E_PlanEstudio planEstudio)
    {
      using var transaction = await _contextoBD.Database.BeginTransactionAsync();
      try
      {
        var planEstudioEnBD = await _contextoBD.PlanEstudios.FirstOrDefaultAsync(pe => pe.IdPlanEstudio == planEstudio.IdPlanEstudio);

        if (planEstudioEnBD == null)
        {
          return new ResultadoAcciones<E_PlanEstudio>
          {
            Resultado = false,
            Mensajes = { "El plan de estudio que se quiere borrar no existe." }
          };
        }

        _contextoBD.Entry(planEstudioEnBD).CurrentValues.SetValues(planEstudio);
        await _contextoBD.SaveChangesAsync();
        await transaction.CommitAsync();

        return new ResultadoAcciones<E_PlanEstudio>
        {
          Entidad = planEstudioEnBD,
          Resultado = true
        };
      }
      catch (DbUpdateException ex)
      {
        await transaction.RollbackAsync();
        return new ResultadoAcciones<E_PlanEstudio>
        {
          Resultado = false,
          Mensajes = {
                        "Error al actualizar el plan de estudio en la base de datos.",
                        ex.InnerException?.Message ?? ex.Message
                    }
        };
      }
      catch (Exception ex)
      {
        await transaction.RollbackAsync();
        return new ResultadoAcciones<E_PlanEstudio>
        {
          Resultado = false,
          Mensajes = { "Error inesperado: " + ex.Message }
        };
      }
    }

    public async Task<ResultadoAcciones<E_PlanEstudio>> ObtenerPlanEstudioPorId(int idPlanEstudio)
    {
      try
      {
        var planEstudio = await _contextoBD.PlanEstudios
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.IdPlanEstudio == idPlanEstudio);

        if (planEstudio == null)
        {
          return new ResultadoAcciones<E_PlanEstudio>
          {
            Resultado = false,
            Mensajes = { $"No se encontró el plan de estudio con el identificador {idPlanEstudio}." }
          };
        }

        return new ResultadoAcciones<E_PlanEstudio>
        {
          Entidad = planEstudio,
          Resultado = true
        };
      }
      catch (Exception ex)
      {
        return new ResultadoAcciones<E_PlanEstudio>
        {
          Resultado = false,
          Mensajes = { "Error al obtener materia: " + ex.Message + "." }
        };
      }
    }
    public async Task<IEnumerable<ListaPlanEstudiosDTO>> ObtenerTodosLosPlanesEstudio()
    {
      return await _contextoBD.PlanEstudios
     .AsNoTracking()
     .OrderBy(pe => pe.Carrera.ClaveCarrera) // Ordenar por ClaveCarrera de la entidad relacionada
     .Select(pe => new ListaPlanEstudiosDTO
     {
       IdPlanEstudio = pe.IdPlanEstudio,
       NombreCarrera = pe.Carrera.NombreCarrera,
       ClaveCarrera = pe.Carrera.ClaveCarrera,
       PlanEstudio = pe.PlanEstudio,
       EstadoPlanEstudio = pe.EstadoPlanEstudio
     })
     .ToListAsync();
    }
    public async Task<IEnumerable<ListaPlanEstudiosDTO>> ObtenerPlanesEstudioPorCriterio(string criterio)
    {
      criterio = criterio?.ToLower() ?? string.Empty;

      return await _contextoBD.PlanEstudios
          .AsNoTracking()
          .Where(pe =>
              pe.Carrera.NombreCarrera.Contains(criterio) ||
              pe.Carrera.ClaveCarrera.Contains(criterio) ||
              pe.PlanEstudio.Contains(criterio))
          .OrderBy(pe => pe.Carrera.ClaveCarrera)
          .Select(pe => new ListaPlanEstudiosDTO
          {
            IdPlanEstudio = pe.IdPlanEstudio,
            NombreCarrera = pe.Carrera.NombreCarrera,
            ClaveCarrera = pe.Carrera.ClaveCarrera,
            PlanEstudio = pe.PlanEstudio,
            EstadoPlanEstudio = pe.EstadoPlanEstudio
          })
          .ToListAsync();
    }

    public async Task<bool> ExisteIdPlanEstudio(int idPlanEstudio)
    {
      if (idPlanEstudio < 0)
        return false;

      return await _contextoBD.PlanEstudios.AsNoTracking().AnyAsync(pe => pe.IdPlanEstudio == idPlanEstudio);
    }
    public async Task<bool> ExistePlanEstudio(int idCarrera, string PlanEstudio, int? idExcluido = null)
    {
      //todo: evisar la logica para que no inserte valores repetidos en conjunto del plan e idCarrera
      var planEstudios = _contextoBD.PlanEstudios.Where(pe => pe.PlanEstudio == PlanEstudio && pe.IdCarrera == idCarrera);

      if (idExcluido.HasValue)
      {
        planEstudios = planEstudios.Where(pe => pe.IdPlanEstudio != idExcluido.Value);
      }

      return await planEstudios.AnyAsync();
    }
  }
}