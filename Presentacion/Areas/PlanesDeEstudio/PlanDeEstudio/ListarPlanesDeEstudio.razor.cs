using Entidades.Modelos;
using Presentacion.Helper;
using Servicios;

namespace Presentacion.Areas.PlanEstudio
{
  //partial class ListarPlanesDeEstudio
  //{
  //  private IEnumerable<E_PlanEstudio> LstPlanEstudio { get; set; } = [];
  //  private string CriterioBusqueda { get; set; } = string.Empty;
  //  protected override async Task OnInitializedAsync()
  //  {
  //    LstPlanEstudio = await planEstudioEsrvicios.ListarPlanEstudio();
  //  }
  //  private async Task BuscarCarrera()
  //  {
  //    LstPlanEstudio = await planEstudioEsrvicios.ListarPlanEstudio(CriterioBusqueda);
  //  }
  //  private async Task BorrarCarrera(int idCarrera)
  //  {
  //    try
  //    {
  //      bool confirmacion = await servicioSweetAlerta.ShowConfirmation("¿Está seguro de borrar la planEstudio?", "Esta acción no se podrá deshacer.");

  //      if (confirmacion)
  //      {
  //        await planEstudioServicios.BorrarCarrera(idCarrera);
  //        await jsRunTime.MsgExito("La planEstudio fue borrada correctamente.");
  //        LstPlanEstudio = await planEstudioServicios.ListarPlanEstudio();
  //      }
  //      else
  //        await servicioSweetAlerta.ShowAlert("Acción cancelada", "Has aceptado no borrar", "error");

  //    }
  //    catch (Exception)
  //    {
  //      await servicioSweetAlerta.ShowAlert("ERROR", "LA CARRERA NO PUDO SER BORRADA", "error");
  //    }
  //  }
  //}
}
