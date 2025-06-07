using Microsoft.AspNetCore.Components;

namespace Presentacion.Areas.PlanesDeEstudio.Materias
{
  public partial class AgregarPracticaTallerMateria
  {
    [Parameter] public int? IdMateria { get; set; }
    [Parameter] public int? IdTipo { get; set; }
  }
}
