using AutoMapper;
using Entidades.Generales;
using Entidades.DTO.PlanesDeEstudio.Carreras;
using Entidades.Modelos.PlanesDeEstudio.Carreras;
using Negocios.Repositorios.PlanesDeEstudio;

namespace Servicios.Repositorios.PlanesDeEstudio
{
  public class CarreraServicios(CarreraNegocios carreraNegocios, IMapper mapper)
  {
    private readonly CarreraNegocios _carreraNegocios = carreraNegocios;
    private readonly IMapper _mapper = mapper;

    public async Task<ResultadoAcciones> InsertarCarrera(CarreraDTO carreraDTO)
    {
      E_Carrera carrera = _mapper.Map<E_Carrera>(carreraDTO);
      return await _carreraNegocios.InsertarCarrera(carrera);
    }
    public async Task<ResultadoAcciones> BorrarCarrera(int idCarrera)
    {
      if (idCarrera <= 0)
        return new ResultadoAcciones
        {
          Mensajes = { "El identificador de la carrera no es válido." },
          Resultado = false
        };      
    
      return await _carreraNegocios.BorrarCarrera(idCarrera);;
    }
    public async Task<ResultadoAcciones> ModificarCarrera(CarreraDTO carreraDTO)
    {
      E_Carrera carrera = _mapper.Map<E_Carrera>(carreraDTO);
      return await _carreraNegocios.ModificarCarrera(carrera);
    }

    public async Task<ResultadoAcciones<T>> ObtenerCarrera<T>(int idCarrera) where T : class
    {
      try
      {
        var carrera = await _carreraNegocios.ObtenerCarrera(idCarrera);

        if (!carrera.Resultado || carrera.Entidad == null)
        {
          return new ResultadoAcciones<T>
          {
            Resultado = false,
            Mensajes = carrera.Mensajes,
          };
        }

        // Mapeas la entidad al DTO
        var dtoMapeado = _mapper.Map<T>(carrera.Entidad);

        return new ResultadoAcciones<T>
        {
          Mensajes = carrera.Mensajes,
          Entidad = dtoMapeado,
          Resultado = true
        };
      }
      catch (Exception ex)
      {
        return new ResultadoAcciones<T>
        {
          Mensajes = ["Error inesperado al procesar la solicitud. " + ex.Message + " - " + ex.InnerException.Message],
          Resultado = false
        };
      }
    }

    public async Task<IEnumerable<E_Carrera>> ListarCarreras()
    {
      return await _carreraNegocios.ListarCarreras();
    }
    public async Task<IEnumerable<E_Carrera>> ListarCarreras(string criterioBusqueda)
    {
      return await _carreraNegocios.ListarCarreras(criterioBusqueda);
    }
  }
}
