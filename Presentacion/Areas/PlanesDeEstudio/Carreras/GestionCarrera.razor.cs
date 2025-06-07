using Entidades.DTO.PlanesDeEstudio.Carreras;
using Entidades.Generales;
using Microsoft.AspNetCore.Components;
using Presentacion.Helper;
using Servicios;

namespace Presentacion.Areas.PlanesDeEstudio.Carreras
{
  public partial class GestionCarrera
  {
    [Parameter] public int? IdCarrera { get; set; }
    private bool EsModificacion => IdCarrera.HasValue;
    private CarreraDTO CarreraDTO { get; set; } = new();
    private ResultadoAcciones<CarreraDTO> resultadoAccion = new();
    private ResultadoAcciones resultados = new();
    protected override async Task OnInitializedAsync()
    {
      if (EsModificacion && IdCarrera.HasValue)
      {
        resultadoAccion = await CarreraServicios.ObtenerCarrera<CarreraDTO>(IdCarrera.Value);

        if (resultadoAccion.Resultado && resultadoAccion.Entidad != null)
          CarreraDTO = resultadoAccion.Entidad;
        else
        {
          ResultadoAcciones r = new()
          {
            Mensajes = resultadoAccion.Mensajes,
            Resultado = false,
          };

          await jsRunTime.MsgError(r);
        }
      }
    }

    private async Task GuardarCarrera()
    {
      resultados = EsModificacion
          ? await CarreraServicios.ModificarCarrera(CarreraDTO)
          : await CarreraServicios.InsertarCarrera(CarreraDTO);

      await HandleOperationResult(resultados);
    }

    private async Task HandleOperationResult(ResultadoAcciones resultado)
    {
      if (resultado.Resultado)
      {
        await jsRunTime.MsgExito($"Los datos de la carrera <br>{CarreraDTO.NombreCarrera}</br> se grabaron correctamente.");
        if (EsModificacion) 
          navigationManager.NavigateTo("ListarCarreras");
        else
          CarreraDTO = new();
      }
      else
      {
        await jsRunTime.MsgError(resultado);

      }
    }
  }
}