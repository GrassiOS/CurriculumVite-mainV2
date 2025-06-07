using AutoMapper;
using Entidades.Generales;
using Entidades.DTO.PlanesDeEstudio.Materias;
using Entidades.Modelos.PlanesDeEstudio.Materias;
using Negocios.Repositorios.PlanesDeEstudio;

namespace Servicios.Repositorios.PlanesDeEstudio
{
  public class MateriaServicios(MateriaNegocios materiaNegocios, IMapper mapper)
  {
    private readonly MateriaNegocios _materiaNegocios = materiaNegocios;
    private readonly IMapper _mapper = mapper;

    public async Task<ResultadoAcciones> InsertarMateria(MateriaDTO materiaDTO)
    {
      E_Materia materia = _mapper.Map<E_Materia>(materiaDTO);
      return await _materiaNegocios.InsertarMateria(materia);
    }
    public async Task<ResultadoAcciones> BorrarMateria(int idMateria)
    {
      if (idMateria <= 0)
        return new ResultadoAcciones
        {
          Mensajes = { "El identificador de la materia no es válido." },
          Resultado = false
        };

      return await _materiaNegocios.BorrarMateria(idMateria);
    }
    public async Task<ResultadoAcciones> ModificarMateria(MateriaDTO materiaDTO)
    {
      E_Materia materia = _mapper.Map<E_Materia>(materiaDTO);
      return await _materiaNegocios.ModificarMateria(materia);
    }

    public async Task<ResultadoAcciones<T>> ObtenerMateria<T>(int idMateria) where T : class
    {
      try
      {
        var materia = await _materiaNegocios.ObtenerMateria(idMateria);

        if (!materia.Resultado || materia.Entidad == null)
        {
          return new ResultadoAcciones<T>
          {
            Resultado = false,
            Mensajes = materia.Mensajes,
          };
        }

        // Mapeas la entidad al DTO
        var dtoMapeado = _mapper.Map<T>(materia.Entidad);

        return new ResultadoAcciones<T>
        { 
          Mensajes = materia.Mensajes,
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

    public async Task<IEnumerable<E_Materia>> ListarMaterias()
    {
      return await _materiaNegocios.ListarMaterias();
    }
    public async Task<IEnumerable<E_Materia>> ListarMaterias(string criterioBusqueda)
    {
      return await _materiaNegocios.ListarMaterias(criterioBusqueda);
    }
  }
}
