using Entidades.Generales;
using Entidades.Modelos.PlanesDeEstudio.Materias;

namespace Datos.IRepositorios.PlnesDeEstudio
{
  public interface IMateriaRepositorio
  {
    public Task<ResultadoAcciones<E_Materia>> InsertarMateria(E_Materia Materia);
    public Task<ResultadoAcciones>BorrarMateria(int idMateria);
    public Task<ResultadoAcciones<E_Materia>> ModificarMateria(E_Materia materia);

    public Task<ResultadoAcciones<E_Materia>> ObtenerMateriaPorId(int idMateria);
    public Task<IEnumerable<E_Materia>> ObtenerTodasLasMaterias();
    public Task<IEnumerable<E_Materia>> ObtenerMateriaPorCriterio(string criterioBusqueda);

    public Task<bool> ExisteIdMateria(int idMateria);
    public Task<bool> ExisteClaveMateria(string claveMateria, int? idExcluido = null);    
  }
}