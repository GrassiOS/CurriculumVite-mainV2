
using Microsoft.JSInterop;

namespace Presentacion.Helper
{
   public class SweetAlertServicios
   {
      private readonly IJSRuntime _runtime;

      public SweetAlertServicios(IJSRuntime runtime)
      {
         _runtime = runtime;
      }

      public async Task ShowAlert(string titulo, string texto, string icono)
      {
         await _runtime.InvokeVoidAsync("SweetAlertHelper.showAlert", titulo, texto, icono);
      }
      public async Task<bool> ShowConfirmation(string titulo, string texto)
      {
         var result = await _runtime.InvokeAsync<bool>("SweetAlertHelper.showConfirmation", titulo, texto);
         return result;
      }
   }
}
