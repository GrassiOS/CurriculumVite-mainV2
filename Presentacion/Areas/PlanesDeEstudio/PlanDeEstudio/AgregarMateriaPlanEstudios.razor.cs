using Entidades.DTO.PlanesDeEstudio.PlanEstudios;
using Entidades.Generales;
using Entidades.Modelos.PlanesDeEstudio.Materias;
using Microsoft.AspNetCore.Components;

namespace Presentacion.Areas.PlanesDeEstudio.PlanDeEstudio
{
  public partial class AgregarMateriaPlanEstudios
  {
    [Parameter]
    public int idPlanEstudio { get; set; }

    private ListaPlanEstudiosDTO? LstPlanEstudio = new ListaPlanEstudiosDTO();
    private ResultadoAcciones<PlanEstudioDTO> planDeEstudioBuscado = new ResultadoAcciones<PlanEstudioDTO>();
    private PlanEstudioDTO planDeEstudio = new PlanEstudioDTO();

    private string criterioBusqueda { get; set; } = string.Empty;
    private string materiaAsiganda { get; set; } = string.Empty;
    private int idEtapa = 1;
    private int idArea = 1;
    private int Semestre = 1;

    private IEnumerable<E_Materia> LstMaterias { get; set; } = new List<E_Materia>();

    private string msg = string.Empty;

    protected override async Task OnInitializedAsync()
    {
      planDeEstudioBuscado = await planEstudioServicios.ObtenerPlanEstudio<PlanEstudioDTO>(idPlanEstudio);
      planDeEstudio = planDeEstudioBuscado.Entidad;
    }

    private async Task BuscarMateria()
    {
      LstMaterias = await materiaServicios.ListarMaterias(criterioBusqueda);
    }

    private void AgregaMateriaCarrera(int idMateria)
    {
      materiaAsiganda = $"{LstMaterias.First(m => m.IdMateria == idMateria).ClaveMateria}: " +
                        $"{LstMaterias.First(m => m.IdMateria == idMateria).NombreMateria}";
      LstMaterias = new List<E_Materia>();
    }
  }
}
