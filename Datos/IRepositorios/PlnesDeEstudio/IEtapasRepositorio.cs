using Entidades.Modelos.PlanesDeEstudio.Etapas;

namespace Datos.IRepositorios.PlnesDeEstudio
{
    public interface IEtapasRepositorio
    {
    public Task<IEnumerable<E_Etapa>> ObtenerTodasLasEtapas();
  }
}
