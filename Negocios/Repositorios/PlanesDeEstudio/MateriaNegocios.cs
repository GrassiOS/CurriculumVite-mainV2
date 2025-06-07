using Entidades.Generales;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;
using Datos.IRepositorios.PlnesDeEstudio;
using Entidades.Modelos.PlanesDeEstudio.Materias;

namespace Negocios.Repositorios.PlanesDeEstudio
{
  public class MateriaNegocios(IMateriaRepositorio materiaRepositorio)
  {
    private readonly IMateriaRepositorio _materiaRepositorio = materiaRepositorio;

    public async Task<ResultadoAcciones> InsertarMateria(E_Materia materia)
    {
      if (materia == null)
      {
        return new ResultadoAcciones
        {
          Mensajes = { "La materia no tiene los datos necesarios para agregarla al sistema." },
          Resultado = false
        };
      }

      var resultadoValidacion = await ValidarMateria(materia);
      if (!resultadoValidacion.Resultado)
        return resultadoValidacion;

      try
      {
        await _materiaRepositorio.InsertarMateria(materia);
        return resultadoValidacion;
      }
      catch (Exception ex)
      {
        return new ResultadoAcciones
        {
          Mensajes = { "Ocurrió un error inesperado al insertar la materia: ", ex.Message + "." },
          Resultado = false
        };
      }
    }
    public async Task<ResultadoAcciones> BorrarMateria(int idMateria)
    {
      if (idMateria <= 0)
      {
        return new ResultadoAcciones
        {
          Mensajes = { "El identidicador de la materia no es válido." },
          Resultado = false
        };
      }

      try
      {
        await _materiaRepositorio.BorrarMateria(idMateria);
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
          Mensajes = { "Ocurrió un error inesperado al borrar la materia", ex.Message + "." },
          Resultado = false
        };
      }
    }
    public async Task<ResultadoAcciones> ModificarMateria(E_Materia materia)
    {
      if (materia == null || materia.IdMateria <= 0)
      {
        return new ResultadoAcciones
        {
          Mensajes = { "Los datos de la materia son inválidos." },
          Resultado = false
        };
      }

      var resultadoValidacion = await ValidarMateria(materia, esModificacion: true);

      if (!resultadoValidacion.Resultado)
        return resultadoValidacion;

      try
      {
        await _materiaRepositorio.ModificarMateria(materia);
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
          Mensajes = { "Ocurrió un error inesperado al modificar la materia", ex.Message + "." },
          Resultado = false
        };
      }
    }

    public async Task<ResultadoAcciones<E_Materia>> ObtenerMateria(int idMateria)
    {
      if (idMateria <= 0)
      {
        return new ResultadoAcciones<E_Materia>
        {
          Mensajes = { "El identificador de la materia no es válido." },
          Resultado = false
        };
      }

      try
      {
        var materia = await _materiaRepositorio.ObtenerMateriaPorId(idMateria);

        if (materia == null)
        {
          return new ResultadoAcciones<E_Materia>
          {
            Mensajes = { $"No se encontró la materia con identificador {idMateria} ." },
            Resultado = true
          };
        }

        return new ResultadoAcciones<E_Materia>
        {
          Entidad = materia.Entidad,
          Resultado = true
        };
      }
      catch (Exception)
      {
        return new ResultadoAcciones<E_Materia>
        {
          Mensajes = { "Error al acceder a los datos de la materia." },
          Resultado = false
        };
      }
    }
    public async Task<IEnumerable<E_Materia>> ListarMaterias()
    {
      try
      {
        return await _materiaRepositorio.ObtenerTodasLasMaterias();
      }
      catch (Exception)
      {
        return [];
      }
    }
    public async Task<IEnumerable<E_Materia>> ListarMaterias(string criterioBusqueda)
    {
      try
      {
        return await _materiaRepositorio.ObtenerMateriaPorCriterio(criterioBusqueda);
      }
      catch (Exception)
      {
        return [];
      }
    }

    public async Task<ResultadoAcciones> ValidarMateria(E_Materia materia, bool esModificacion = false)
    {
      ResultadoAcciones resultado = new();

      if (string.IsNullOrWhiteSpace(materia.ClaveMateria))
      {
        resultado.Mensajes.Add("La clave de la materia es requerida.\n");
        resultado.Resultado = false;
      }
      else if (!Regex.IsMatch(materia.ClaveMateria, @"^\d{1,6}$"))
      {
        resultado.Mensajes.Add("La clave debe tener menos de 6 dígitos.\n");
        resultado.Resultado = false;
      }      

      if (esModificacion)
      {
        await ValidarUnicidadClave(resultado, materia.ClaveMateria, materia.IdMateria);
      }
      else
      {
        await ValidarUnicidadClave(resultado, materia.ClaveMateria);
      }

      ValidarHoras_Y_Creditos(resultado, materia);
      return resultado;
    }

    private async Task ValidarUnicidadClave(ResultadoAcciones resultado, string claveMateria, int? idExcluido = null)
    {
      bool existe = idExcluido.HasValue
          ? await _materiaRepositorio.ExisteClaveMateria(claveMateria, idExcluido.Value)
          : await _materiaRepositorio.ExisteClaveMateria(claveMateria);

      if (existe)
      {
        resultado.Mensajes.Add($"La clave <b>{claveMateria}</b> ya está asignada aotra materia.\n");
        resultado.Resultado = false;
      }
    }
    
    private static void ValidarHoras_Y_Creditos(ResultadoAcciones resultado, E_Materia materia)
    {
      // Validar que todos los valores estén entre 1 y 10
      if (materia.HC < 0 || materia.HC > 10 || materia.HL < 0 || materia.HL > 10 || materia.HT < 0 || materia.HT> 10 ||
          materia.HCL < 0 || materia.HCL > 10 || materia.HPC < 0 || materia.HPC > 10 || materia.HE < 0 || materia.HE > 10
         )
      {
        resultado.Mensajes.Add("Todos los valores deben estar entre 0 y 10");
        resultado.Resultado = false;
      }

      // Validar que HC sea el doble de HE
      if (materia.HC != materia.HE)
      {
        resultado.Mensajes.Add("\"Las horas HC deben ser iguales a las HE");
        resultado.Resultado = false;
      }
            
      int sumaHoras = materia.HC + materia.HL + materia.HT + materia.HCL + materia.HPC + materia.HE;
      if (sumaHoras != materia.CR)
      {
        resultado.Mensajes.Add($"La suma de las horas (HL, HT, HCL, FPC, HE) debe ser igual a los créditos. Suma actual: {sumaHoras}, Créditos: {materia.CR}");
        resultado.Resultado = false;
      }      
    }
  }
}