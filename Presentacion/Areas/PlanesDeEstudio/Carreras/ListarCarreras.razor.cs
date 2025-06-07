using Entidades.Modelos.PlanesDeEstudio.Carreras;
using Presentacion.Helper;

namespace Presentacion.Areas.PlanesDeEstudio.Carreras
{
  partial class ListarCarreras
  {
    private IEnumerable<E_Carrera> LstCarreras { get; set; } = [];
    private string CriterioBusqueda { get; set; } = string.Empty;
    protected override async Task OnInitializedAsync()
    {
      LstCarreras = await carreraServicios.ListarCarreras();
    }    
    private async Task BuscarCarrera()
    {
      LstCarreras = await carreraServicios.ListarCarreras(CriterioBusqueda);
    }
    private async Task BorrarCarrera(int idCarrera)
    {
      try
      {
        bool confirmacion = await servicioSweetAlerta.ShowConfirmation("¿Está seguro de borrar la carrera?", "Esta acción no se podrá deshacer.");

        if (confirmacion)
        {
          await carreraServicios.BorrarCarrera(idCarrera);
          await jsRunTime.MsgExito("La carrera fue borrada correctamente.");
          LstCarreras = await carreraServicios.ListarCarreras();
        }
        else
          await servicioSweetAlerta.ShowAlert("Acción cancelada", "Has aceptado no borrar", "error");

      }
      catch (Exception)
      {
        await servicioSweetAlerta.ShowAlert("ERROR", "LA CARRERA NO PUDO SER BORRADA", "error");
      }
    }
  }
}
