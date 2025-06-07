using Entidades.Generales;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;
using Datos.IRepositorios.PlnesDeEstudio;
using Entidades.DTO.PlanesDeEstudio.PlanEstudios;
using Entidades.Modelos.PlanesDeEstudio.PlanEstudios;

namespace Negocios.Repositorios.PlanesDeEstudio
{
  public class PlanEstudioNegocios(IPlanEstudioRepositorio planEstudioRepositorio)
  {
    private readonly IPlanEstudioRepositorio _planEstudioRepositorio = planEstudioRepositorio;

    public async Task<ResultadoAcciones> InsertarPlanEstudio(E_PlanEstudio planEstudio, int idCarrera)
    {
      if (planEstudio == null)
      {
        return new ResultadoAcciones
        {
          Mensajes = { "La mplanEstudio no tiene los datos necesarios para agregarla al sistema." },
          Resultado = false
        };
      }

      var resultadoValidacion = await ValidarPlanEstudio(planEstudio, idCarrera);
      if (!resultadoValidacion.Resultado)
        return resultadoValidacion;

      try
      {
        await _planEstudioRepositorio.InsertarPlanEstudio(planEstudio);
        return resultadoValidacion;
      }
      catch (Exception ex)
      {
        return new ResultadoAcciones
        {
          Mensajes = { "Ocurrió un error inesperado al insertar la mplanEstudio: ", ex.Message + "." },
          Resultado = false
        };
      }
    }
    public async Task<ResultadoAcciones> BorrarPlanEstudio(int idPlanEstudio)
    {
      if (idPlanEstudio <= 0)
      {
        return new ResultadoAcciones
        {
          Mensajes = { "El identidicador de la mplanEstudio no es válido." },
          Resultado = false
        };
      }

      try
      {
        await _planEstudioRepositorio.BorrarPlanEstudio(idPlanEstudio);
        return new ResultadoAcciones { Resultado = true };
      }
      catch (KeyNotFoundException ex)
      {
        return new ResultadoAcciones
        {
          Mensajes = { ex.Message },
          Resultado = false
        };
      }
      catch (Exception ex)
      {
        return new ResultadoAcciones
        {
          Mensajes = { "Ocurrió un error inesperado al borrar la mplanEstudio", ex.Message + "." },
          Resultado = false
        };
      }
    }
    public async Task<ResultadoAcciones> ModificarPlanEstudio(E_PlanEstudio planEstudio, int idCarrera)
    {
      if (planEstudio == null || planEstudio.IdPlanEstudio <= 0)
      {
        return new ResultadoAcciones
        {
          Mensajes = { "Los datos de la mplanEstudio son inválidos." },
          Resultado = false
        };
      }

      var resultadoValidacion = await ValidarPlanEstudio(planEstudio, idCarrera, esModificacion: true);

      if (!resultadoValidacion.Resultado)
        return resultadoValidacion;

      try
      {
        await _planEstudioRepositorio.ModificarPlanEstudio(planEstudio);
        return resultadoValidacion;
      }
      catch (KeyNotFoundException ex)
      {
        return new ResultadoAcciones
        {
          Mensajes = { ex.Message },
          Resultado = false
        };
      }
      catch (Exception ex)
      {
        return new ResultadoAcciones
        {
          Mensajes = { "Ocurrió un error inesperado al modificar la mplanEstudio", ex.Message + "." },
          Resultado = false
        };
      }
    }

    public async Task<ResultadoAcciones<E_PlanEstudio>> ObtenerPlanEstudio(int idPlanEstudio)
    {
      if (idPlanEstudio <= 0)
      {
        return new ResultadoAcciones<E_PlanEstudio>
        {
          Mensajes = { "El identificador de la mplanEstudio no es válido." },
          Resultado = false
        };
      }

      try
      {
        var mplanEstudio = await _planEstudioRepositorio.ObtenerPlanEstudioPorId(idPlanEstudio);

        if (mplanEstudio == null)
        {
          return new ResultadoAcciones<E_PlanEstudio>
          {
            Mensajes = { $"No se encontró la mplanEstudio con identificador {idPlanEstudio} ." },
            Resultado = true
          };
        }

        return new ResultadoAcciones<E_PlanEstudio>
        {
          Entidad = mplanEstudio.Entidad,
          Resultado = true
        };
      }
      catch (Exception)
      {
        return new ResultadoAcciones<E_PlanEstudio>
        {
          Mensajes = { "Error al acceder a los datos de la mplanEstudio." },
          Resultado = false
        };
      }
    }
   

    public async Task<IEnumerable<ListaPlanEstudiosDTO>> ListarPlanEstudios()
    {
      try
      {
        return await _planEstudioRepositorio.ObtenerTodosLosPlanesEstudio();
      }
      catch (Exception)
      {
        return [];
      }
    }
    public async Task<IEnumerable<ListaPlanEstudiosDTO>> ObtenerPlanEstudioPorCriterio(string criterioBusqueda)
    {
      try
      {

        return await _planEstudioRepositorio.ObtenerPlanesEstudioPorCriterio(criterioBusqueda);
      }
      catch (Exception)
      {
        return [];
      }
    }

    public async Task<ResultadoAcciones> ValidarPlanEstudio(E_PlanEstudio planEstudio, int idCarrera, bool esModificacion = false)
    {
      ResultadoAcciones resultado = new();

      if (!Regex.IsMatch(planEstudio.PlanEstudio, @"^\d{4}-[124]$"))
      {
        resultado.Mensajes.Add("El formato debe ser AAAA-D donde AAAA es el año y D es 1, 2 o 4.\n");
        resultado.Resultado = false;
      }

      if (esModificacion)
      {
        await ValidarUnicidadPlanEstudio(resultado, idCarrera, planEstudio.PlanEstudio, planEstudio.IdPlanEstudio);
      }
      else
      {
        await ValidarUnicidadPlanEstudio(resultado, idCarrera, planEstudio.PlanEstudio);
      }
      return resultado;
    }

    private async Task ValidarUnicidadPlanEstudio(ResultadoAcciones resultado, int idCarrera, string planEstudio, int? idExcluido = null)
    {
      bool existe = idExcluido.HasValue
          ? await _planEstudioRepositorio.ExistePlanEstudio(idCarrera, planEstudio, idExcluido.Value)
          : await _planEstudioRepositorio.ExistePlanEstudio(idCarrera, planEstudio);

      if (existe)
      {
        resultado.Mensajes.Add($"El plan de estudios <b>{planEstudio}</b> ya está asignado a esa carrera.\n");
        resultado.Resultado = false;
      }
    }

    
  }
}