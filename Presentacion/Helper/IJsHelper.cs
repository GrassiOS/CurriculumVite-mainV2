using Entidades.Generales;
using Microsoft.JSInterop;

namespace Presentacion.Helper
{
  public static class IJsHelper
  {
    public static async ValueTask MsgExito(this IJSRuntime jsRuntime, string mensaje)
    {
      await jsRuntime.InvokeVoidAsync("ShowToastr", "success", mensaje);
    }
    public static async ValueTask MsgError(this IJSRuntime jsRuntime, ResultadoAcciones resultado)
    {
      if (resultado?.Mensajes != null && resultado.Mensajes.Count != 0)
      {
        var mensajeCompleto = string.Join("<br>", resultado.Mensajes);
        await jsRuntime.InvokeVoidAsync("ShowToastr", "error", mensajeCompleto);
      }
    }
    public static async ValueTask MsgInfo(this IJSRuntime jsRuntime, string mensaje)
    {
      await jsRuntime.InvokeVoidAsync("ShowToastr", "info", mensaje);
    }
    public static async ValueTask MsgPrecaucion(this IJSRuntime jsRuntime, string mensaje)
    {
      await jsRuntime.InvokeVoidAsync("ShowToastr", "warning", mensaje);
    }
  }
}
