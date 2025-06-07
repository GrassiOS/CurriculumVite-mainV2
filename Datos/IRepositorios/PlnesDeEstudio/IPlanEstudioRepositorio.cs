using Entidades.Generales;
using Entidades.DTO.PlanesDeEstudio.PlanEstudios;
using Entidades.Modelos.PlanesDeEstudio.PlanEstudios;


namespace Datos.IRepositorios.PlnesDeEstudio
{
  public interface IPlanEstudioRepositorio
  {
    public Task<ResultadoAcciones<E_PlanEstudio>> InsertarPlanEstudio(E_PlanEstudio PlanEstudio);
    public Task<ResultadoAcciones> BorrarPlanEstudio(int idPlanEstudio);
    public Task<ResultadoAcciones<E_PlanEstudio>> ModificarPlanEstudio(E_PlanEstudio PlanEstudio);

    public Task<ResultadoAcciones<E_PlanEstudio>> ObtenerPlanEstudioPorId(int idPlanEstudio);
    public Task<IEnumerable<ListaPlanEstudiosDTO>> ObtenerTodosLosPlanesEstudio();
    public Task<IEnumerable<ListaPlanEstudiosDTO>> ObtenerPlanesEstudioPorCriterio(string criterioBusqueda);

    public Task<bool> ExisteIdPlanEstudio(int idPlanEstudio);
    //public Task<bool> ExisteClavePlanEstudio(string clavePlanEstudio, int? idExcluido = null);
    public Task<bool> ExistePlanEstudio(int idCarrera, string PlanEstudio, int? idExcluido = null);
  }
}
