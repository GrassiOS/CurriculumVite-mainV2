using Entidades.Generales;
using Microsoft.AspNetCore.Components;
using Presentacion.Helper;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entidades.DTO.PlanesDeEstudio.Materias;

namespace Presentacion.Areas.PlanesDeEstudio.Materias
{
  public partial class GestionMateria
  {
    private ValidationSummary? validationSummary;
    private List<string> errorMessages = new();
    
    [Parameter] public int? IdMateria { get; set; }
    private bool EsModificacion => IdMateria.HasValue;
    private MateriaDTO MateriaDTO { get; set; } = new();
    private ResultadoAcciones<MateriaDTO> resultadoAccion = new();
    private ResultadoAcciones resultados = new();

    private async Task HandleValidSubmit()
    {
      errorMessages.Clear();
      await GuardarMateria();
    }

    protected override async Task OnInitializedAsync()
    {
      if (EsModificacion && IdMateria.HasValue)
      {
        resultadoAccion = await MateriaServicios.ObtenerMateria<MateriaDTO>(IdMateria.Value);

        if (resultadoAccion.Resultado && resultadoAccion.Entidad != null)
        {
          MateriaDTO = resultadoAccion.Entidad;
        }
        else
        {
          errorMessages = resultadoAccion.Mensajes?.ToList() ?? new List<string>();
          await MostrarError("No se pudo cargar la materia", errorMessages);
        }
      }
    }

    private async Task GuardarMateria()
    {
      try
      {
        if (EsModificacion)
        {
          var res = await MateriaServicios.ModificarMateria(MateriaDTO);
          await ProcesarResultado(res, "modificaron");
        }
        else
        {
          resultados = await MateriaServicios.InsertarMateria(MateriaDTO);
          await ProcesarResultado(resultados, "registraron");
        }
      }
      catch (Exception ex)
      {
        errorMessages = new List<string> { ex.Message };
        await MostrarError("Error al guardar", errorMessages);
      }
    }

    private async Task ProcesarResultado(ResultadoAcciones resultado, string accion)
    {
      if (resultado.Resultado)
      {
        await jsRunTime.InvokeVoidAsync("Swal.fire", new
        {
          icon = "success",
          title = "Operación exitosa",
          html = $"Los datos de la materia <b>{MateriaDTO.NombreMateria.Trim()}</b> se {accion} correctamente.",
          confirmButtonText = "Aceptar"
        });

        if (!EsModificacion)
        {
          MateriaDTO = new MateriaDTO();
          StateHasChanged();
        }
        else
        {
          navigationManager.NavigateTo("ListarMaterias");
        }
      }
      else
      {
        errorMessages = resultado.Mensajes?.ToList() ?? new List<string>();
        await MostrarError("Error en la operación", errorMessages);
      }
    }

    private async Task MostrarError(string titulo, IEnumerable<string> mensajes)
    {
      await jsRunTime.InvokeVoidAsync("Swal.fire", new
      {
        icon = "error",
        title = titulo,
        html = string.Join("<br>", mensajes),
        confirmButtonText = "Entendido"
      });
    }
  }
}