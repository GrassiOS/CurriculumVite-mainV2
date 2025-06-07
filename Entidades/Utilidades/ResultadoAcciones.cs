using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.Generales
{
  public class ResultadoAcciones
  {
    public List<string> Mensajes { get; set; } = [];
    public bool Resultado { get; set; } = true;
  }

  public class ResultadoAcciones<T> : ResultadoAcciones
  {
    public T? Entidad { get; set; }
  }
}
