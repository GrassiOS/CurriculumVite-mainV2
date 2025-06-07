using AutoMapper;
using Entidades.Generales;
using Entidades.DTO.PlanesDeEstudio.PlanEstudios;
using Entidades.Modelos.PlanesDeEstudio.PlanEstudios;
using Negocios.Repositorios.PlanesDeEstudio;

namespace Servicios.Repositorios.PlanesDeEstudio
{
  public class PlanEstudioServicios(PlanEstudioNegocios planEstudiosNegocios, IMapper mapper)
  {
    private readonly PlanEstudioNegocios _planEstudioNegocios = planEstudiosNegocios;
    private readonly IMapper _mapper = mapper;

    public async Task<ResultadoAcciones> InsertarPlanEstudio(PlanEstudioDTO planEstudioDTO, int idCarrera)
    {
      E_PlanEstudio planEstudio = _mapper.Map<E_PlanEstudio>(planEstudioDTO);
      return await _planEstudioNegocios.InsertarPlanEstudio(planEstudio, idCarrera);
    }
    public async Task<ResultadoAcciones> BorrarPlanEstudio(int idPlanEstudio)
    {
      if (idPlanEstudio <= 0)
        return new ResultadoAcciones
        {
          Mensajes = { "El identificador de la planEstudio no es válido." },
          Resultado = false
        };

      return await _planEstudioNegocios.BorrarPlanEstudio(idPlanEstudio);
    }
    public async Task<ResultadoAcciones> ModificarPlanEstudio(PlanEstudioDTO planEstudioDTO, int idCarrera)
    {
      E_PlanEstudio planEstudio = _mapper.Map<E_PlanEstudio>(planEstudioDTO);
      return await _planEstudioNegocios.ModificarPlanEstudio(planEstudio, idCarrera);
    }

    public async Task<ResultadoAcciones<T>> ObtenerPlanEstudio<T>(int idPlanEstudio) where T : class
    {
      try
      {
        var planEstudio = await _planEstudioNegocios.ObtenerPlanEstudio(idPlanEstudio);

        if (!planEstudio.Resultado || planEstudio.Entidad == null)
        {
          return new ResultadoAcciones<T>
          {
            Resultado = false,
            Mensajes = planEstudio.Mensajes,
          };
        }

        // Mapeas la entidad al DTO
        var dtoMapeado = _mapper.Map<T>(planEstudio.Entidad);

        return new ResultadoAcciones<T>
        {
          Mensajes = planEstudio.Mensajes,
          Entidad = dtoMapeado,
          Resultado = true
        };
      }
      catch (Exception ex)
      {
        return new ResultadoAcciones<T>
        {
          Resultado = false,
          Mensajes = ["Error inesperado al procesar la solicitud. " + ex.Message + " - " + ex.InnerException.Message]
        };
      }
    }
    public async Task<IEnumerable<ListaPlanEstudiosDTO>> ObtenerPlanDeEstudioPorCriterio(string criterio)
    {
      var ListaPE = await _planEstudioNegocios.ObtenerPlanEstudioPorCriterio(criterio);
      var resultado = _mapper.Map<IEnumerable<ListaPlanEstudiosDTO>>(ListaPE);
      return resultado;
    }
    public async Task<IEnumerable<ListaPlanEstudiosDTO>> ListarPlanEstudios()
    {
      var planEstudio = await _planEstudioNegocios.ListarPlanEstudios();
      var resultado = _mapper.Map<IEnumerable<ListaPlanEstudiosDTO>>(planEstudio);

      return resultado;
    }
  }
}
