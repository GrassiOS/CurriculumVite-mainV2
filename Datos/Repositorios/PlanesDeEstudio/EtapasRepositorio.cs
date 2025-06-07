using Datos.IRepositorios.PlnesDeEstudio;
using Entidades.Modelos.PlanesDeEstudio.Etapas;
using Microsoft.EntityFrameworkCore;

namespace Datos.Repositorios.PlanesDeEstudio
{
  public class EtapasRepositorio(ContextoBD contextoBD) : IEtapasRepositorio
  {
    private readonly ContextoBD _contextoBD = contextoBD;
    public async Task<IEnumerable<E_Etapa>> ObtenerTodasLasEtapas()
    {
      return await _contextoBD.Etapas
          .AsNoTracking()          
          .ToListAsync();
    }
  }
}
