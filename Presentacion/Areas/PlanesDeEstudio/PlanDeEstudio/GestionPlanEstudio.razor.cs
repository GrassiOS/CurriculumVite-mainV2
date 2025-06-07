namespace Presentacion.Areas.Materias
{
  public partial class GestionPlanEstudio
  {
    //  [Parameter] public int? IdMateria { get; set; }
    //  private bool EsModificacion => IdMateria.HasValue;
    //  private MateriaDTO MateriaDTO { get; set; } = new();
    //  private ResultadoAcciones<MateriaDTO> resultadoAccion = new ResultadoAcciones<MateriaDTO>();
    //  private ResultadoAcciones resultados = new();
    //  private ResultadoAcciones res = new();

    //  protected override async Task OnInitializedAsync()
    //  {
    //    if (EsModificacion && IdMateria.HasValue)
    //    {
    //      resultadoAccion = await MateriaServicios.ObtenerMateria<MateriaDTO>(IdMateria.Value);

    //      if (resultadoAccion.Resultado && resultadoAccion.Entidad != null)
    //        MateriaDTO = resultadoAccion.Entidad;
    //      else
    //      {
    //        ResultadoAcciones r = new()
    //        {
    //          Mensajes = resultadoAccion.Mensajes,
    //          Resultado = false,
    //        };

    //        await jsRunTime.MsgError(r);
    //      }
    //    }
    //  }

    //  private async Task GuardarMateria()
    //  {
    //    if (EsModificacion)
    //    {
    //      res = await MateriaServicios.ModificarMateria(MateriaDTO);

    //      if (res.Resultado)
    //      {
    //        await jsRunTime.MsgExito($"Los datos de la carrera <br>{MateriaDTO.NombreMateria}</br> se modificaron correctamente.");
    //        navigationManager.NavigateTo("ListarMaterias");
    //      }
    //      else
    //        await jsRunTime.MsgError(res);
    //    }
    //    else
    //    {
    //      resultados = await MateriaServicios.InsertarMateria(MateriaDTO);

    //      if (resultados.Resultado)
    //      {
    //        await jsRunTime.MsgExito($"Los datos de la carrera <b>{MateriaDTO.NombreMateria.Trim()}</b> fueron registrados correctamente.");
    //        MateriaDTO = new MateriaDTO();
    //      }
    //      else
    //      {
    //        await jsRunTime.MsgError(resultados);
    //      }
    //    }
    //  }
  }
}
