using Entidades.Generales;
using Entidades.Modelos.PlanesDeEstudio.Carreras;

namespace Datos.IRepositorios.PlnesDeEstudio
{
  public interface ICarreraRepositorio
  {
    public Task<ResultadoAcciones<E_Carrera>> InsertarCarrera(E_Carrera carrera);
    public Task<ResultadoAcciones> BorrarCarrera(int idCarrera);
    public Task<ResultadoAcciones<E_Carrera>> ModificarCarrera(E_Carrera carrera);

    public Task<ResultadoAcciones<E_Carrera>> ObtenerCarreraPorId(int idCarrera);
    public Task<IEnumerable<E_Carrera>> ObtenerTodasLasCarreras();
    public Task<IEnumerable<E_Carrera?>> ObtenerCarreraPorCriterio(string criterioBusqueda);

    public Task<bool> ExisteIdCarrera(int idCarrera);
    public Task<bool> ExisteClaveCarrera(string clave, int? idExcluido = null);
    public Task<bool> ExisteNombreCarrera(string nombreCarrera, int? idExcluido = null);
    public Task<bool> ExisteAliasCarrera(string aliasCarrera, int? idExcluido = null);
  }
}