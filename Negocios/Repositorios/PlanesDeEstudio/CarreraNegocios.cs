using Entidades.Generales;
using System.Text.RegularExpressions;
using Datos.IRepositorios.PlnesDeEstudio;
using Entidades.Modelos.PlanesDeEstudio.Carreras;

namespace Negocios.Repositorios.PlanesDeEstudio
{
  public class CarreraNegocios(ICarreraRepositorio carreraRepositorio)
  {
    private readonly ICarreraRepositorio _carreraRepositorio = carreraRepositorio;

    public async Task<ResultadoAcciones> InsertarCarrera(E_Carrera carrera)
    {
      if (carrera == null)
      {
        return new ResultadoAcciones
        {
          Mensajes = { "La carrera no tiene los datos necesarios para agregarla al sistema." },
          Resultado = false
        };
      }

      var resultadoValidacion = await ValidarCarrera(carrera);
      if (!resultadoValidacion.Resultado)
        return resultadoValidacion;

      try
      {
        await _carreraRepositorio.InsertarCarrera(carrera);
        return resultadoValidacion;
      }
      catch (Exception ex)
      {
        return new ResultadoAcciones
        {
          Mensajes = { "Ocurrió un error inesperado al insertar la carrera: ", ex.Message + "." },
          Resultado = false
        };
      }
    }
    public async Task<ResultadoAcciones> BorrarCarrera(int idCarrera)
    {
      if (idCarrera <= 0)
      {
        return new ResultadoAcciones
        {
          Mensajes = { "El identidicador de la carrera no es válido." },
          Resultado = false
        };
      }

      try
      {
        await _carreraRepositorio.BorrarCarrera(idCarrera);
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
          Mensajes = { "Ocurrió un error inesperado al borrar la carrera", ex.Message + "." },
          Resultado = false
        };
      }
    }
    public async Task<ResultadoAcciones> ModificarCarrera(E_Carrera carrera)
    {
      if (carrera == null || carrera.IdCarrera <= 0)
      {
        return new ResultadoAcciones
        {
          Mensajes = { "Los datos de la carrera son inválidos." },
          Resultado = false
        };
      }

      var resultadoValidacion = await ValidarCarrera(carrera, esModificacion: true);

      if (!resultadoValidacion.Resultado)
        return resultadoValidacion;

      try
      {
        await _carreraRepositorio.ModificarCarrera(carrera);
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
          Mensajes = { "Ocurrió un error inesperado al modificar la carrera", ex.Message + "." },
          Resultado = false
        };
      }
    }

    public async Task<ResultadoAcciones<E_Carrera>> ObtenerCarrera(int idCarrera)
    {
      if (idCarrera <= 0)
      {
        return new ResultadoAcciones<E_Carrera>
        {
          Mensajes = { "El identificador de la carrera no es válido." },
          Resultado = false
        };
      }

      try
      {
        var carrera = await _carreraRepositorio.ObtenerCarreraPorId(idCarrera);

        if (carrera == null)
        {
          return new ResultadoAcciones<E_Carrera>
          {
            Mensajes = { $"No se encontró la carrera con identificador {idCarrera} ." },
            Resultado = true
          };
        }

        return new ResultadoAcciones<E_Carrera>
        {
          Entidad = carrera.Entidad,
          Resultado = true
        };
      }
      catch (Exception)
      {
        return new ResultadoAcciones<E_Carrera>
        {
          Mensajes = { "Error al acceder a los datos de la carrera." },
          Resultado = false
        };
      }
    }
    public async Task<IEnumerable<E_Carrera>> ListarCarreras()
    {
      try
      {
        return await _carreraRepositorio.ObtenerTodasLasCarreras();
      }
      catch (Exception)
      {
        return [];
      }
    }
    public async Task<IEnumerable<E_Carrera>> ListarCarreras(string criterioBusqueda)
    {
      try
      {
        return await _carreraRepositorio.ObtenerCarreraPorCriterio(criterioBusqueda);
      }
      catch (Exception)
      {
        return [];
      }
    }

    public async Task<ResultadoAcciones> ValidarCarrera(E_Carrera carrera, bool esModificacion = false)
    {
      ResultadoAcciones resultado = new();

      ValidarClaveCarrera(resultado, carrera.ClaveCarrera);
      ValidarNombreCarrera(resultado, carrera.NombreCarrera);
      ValidarAliasCarrera(resultado, carrera.AliasCarrera);

      if (esModificacion)
      {
        await ValidarUnicidadClave(resultado, carrera.ClaveCarrera, carrera.IdCarrera);
        await ValidarUnicidadNombre(resultado, carrera.NombreCarrera, carrera.IdCarrera);
        await ValidarUnicidadAlias(resultado, carrera.AliasCarrera, carrera.IdCarrera);
      }
      else
      {
        await ValidarUnicidadClave(resultado, carrera.ClaveCarrera);
        await ValidarUnicidadNombre(resultado, carrera.NombreCarrera);
        await ValidarUnicidadAlias(resultado, carrera.AliasCarrera);
      }

      return resultado;
    }
    private static void ValidarClaveCarrera(ResultadoAcciones resultado, string claveCarrera)
    {
      if (string.IsNullOrWhiteSpace(claveCarrera))
      {
        resultado.Mensajes.Add("La clave de la carrera es requerida.\n");
        resultado.Resultado = false;
      }
      else if (!Regex.IsMatch(claveCarrera, @"^\d{3}$"))
      {
        resultado.Mensajes.Add("La clave debe tener 3 dígitos.\n");
        resultado.Resultado = false;
      }
    }
    private async Task ValidarUnicidadClave(ResultadoAcciones resultado, string claveCarrera, int? idExcluido = null)
    {
      try
      {
        bool existe = idExcluido.HasValue
                  ? await _carreraRepositorio.ExisteClaveCarrera(claveCarrera, idExcluido.Value)
                  : await _carreraRepositorio.ExisteClaveCarrera(claveCarrera);

        if (existe)
        {
          resultado.Mensajes.Add($"La clave <b>{claveCarrera}</b> ya está asignada aotra carrera.\n");
          resultado.Resultado = false;
        }
      }
      catch (Exception)
      {
        resultado.Mensajes.Add($"Error al tratar de acceder a la base d edatos: Cominiquese cono los desarrolladores.");
        resultado.Resultado = false;
      }

    }
    private static void ValidarNombreCarrera(ResultadoAcciones resultado, string nombreCarrera)
    {
      if (string.IsNullOrWhiteSpace(nombreCarrera))
      {
        resultado.Mensajes.Add("El nombre de la carrera es requerido.\n");
        resultado.Resultado = false;
      }
      else if (!Regex.IsMatch(nombreCarrera, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ\s]+$"))
      {
        resultado.Mensajes.Add("El nombre contiene caracteres no permitidos.\n");
        resultado.Resultado = false;
      }
      else if (nombreCarrera.Length > 100)
      {
        resultado.Mensajes.Add("El nombre no puede exceder 100 caracteres.\n");
        resultado.Resultado = false;
      }
    }
    private static void ValidarAliasCarrera(ResultadoAcciones resultado, string aliasCarrera)
    {
      if (string.IsNullOrWhiteSpace(aliasCarrera))
      {
        resultado.Mensajes.Add("El alias de la carrera es requerido.\n");
        resultado.Resultado = false;
      }
      else if (!Regex.IsMatch(aliasCarrera, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ\s]+$"))
      {
        resultado.Mensajes.Add("El alias de la carrera contiene caracteres no permitidos.\n");
        resultado.Resultado = false;
      }
      else if (aliasCarrera.Length > 100)
      {
        resultado.Mensajes.Add("El alias no puede exceder 100 caracteres.\n");
        resultado.Resultado = false;
      }
    }
    private async Task ValidarUnicidadNombre(ResultadoAcciones resultado, string nombreCarrera, int? idExcluido = null)
    {
      try
      {
        bool existe = idExcluido.HasValue
                  ? await _carreraRepositorio.ExisteNombreCarrera(nombreCarrera, idExcluido)
                  : await _carreraRepositorio.ExisteNombreCarrera(nombreCarrera);
        if (existe)
        {
          resultado.Mensajes.Add($"El nombre <b>{nombreCarrera}</b> ya está asignado a otra carrera.\n");
          resultado.Resultado = false;
        }
      }
      catch (Exception)
      {
        resultado.Mensajes.Add($"No se pudo conectar a la base de datos: Comuníquese con los desarrolladores.</br>n");
        resultado.Resultado = false;
      }
    }
    private async Task ValidarUnicidadAlias(ResultadoAcciones resultado, string aliasCarrera, int? idExcluido = null)
    {
      try
      {
        bool existe = idExcluido.HasValue
         ? await _carreraRepositorio.ExisteAliasCarrera(aliasCarrera, idExcluido)
         : await _carreraRepositorio.ExisteAliasCarrera(aliasCarrera);

        if (existe)
        {
          resultado.Mensajes.Add($"El alias <b>{aliasCarrera}</b> ya está asignado a otra carrera.\n");
          resultado.Resultado = false;
        }
      }
      catch (Exception)
      {
        resultado.Mensajes.Add($"\nNo se pudo conectar a la base de datos: Comuníquese con los desarrolladores.");
        resultado.Resultado = false;
      }
    }
  }
}