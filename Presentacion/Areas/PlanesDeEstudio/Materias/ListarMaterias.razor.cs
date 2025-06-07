using Entidades.Modelos.PlanesDeEstudio.Materias;
using Presentacion.Helper;

namespace Presentacion.Areas.PlanesDeEstudio.Materias
{
  partial class ListarMaterias
  {
    private IEnumerable<E_Materia> LstMaterias { get; set; } = new List<E_Materia>();
    private string criterioBusqueda { get; set; } = string.Empty;
    
    protected override async Task OnInitializedAsync()
    {
      LstMaterias = await MateriaServicios.ListarMaterias();
    }
    private async Task BuscarMateria()
    {
      LstMaterias = await MateriaServicios.ListarMaterias(criterioBusqueda);
    }
    private async Task BorrarMateria(int idMateria)
    {
      try
      {
        bool confirmacion = await servicioSweetAlerta.ShowConfirmation("¿Está seguro de borrar la materia?", "Esta acción no se podrá deshacer.");

        if (confirmacion)
        {
          await MateriaServicios.BorrarMateria(idMateria);
          await jsRunTime.MsgExito("La materia fue borrada correctamente.");
          LstMaterias = await MateriaServicios.ListarMaterias();
        }
        else
          await servicioSweetAlerta.ShowAlert("Acción cancelada", "Has aceptado no borrar", "error");

      }
      catch (Exception)
      {
        await servicioSweetAlerta.ShowAlert("ERROR", "LA MATERIA NO PUDO SER BORRADA", "error");
      }
    }
  }
}
