using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Entidades.DTO.CurriculumVite;
using Servicios.IRepositorios.CurriculumVite;
using Servicios.IRepositorios;
using Presentacion.Components.Shared;
using Presentacion.Helper;
using Entidades.Generales;

namespace Presentacion.Areas.CV.Docentes
{
    public partial class GestorPublicaciones : ComponentBase
    {
        [Parameter] public int IdDocente { get; set; }
        [Parameter] public bool FromProfile { get; set; }

        [Inject] private ISRepositorioPublicacion PublicacionServicios { get; set; } = default!;
        [Inject] private ISRepositorioDocumento DocumentoServicios { get; set; } = default!;
        [Inject] private ISDocenteRepositorio DocenteServicios { get; set; } = default!;
        [Inject] private IDocumentRepository DocumentRepository { get; set; } = default!;
        [Inject] private IJSRuntime JSRuntime { get; set; } = default!;
        [Inject] private NavigationManager NavigationManager { get; set; } = default!;

        // Variables principales
        private List<PublicacionDTO> ListaPublicaciones = new();
        private List<PublicacionDTO> PublicacionesFiltradas = new();
        private string nombreCompletoDocente = "";
        private string numeroEmpleado = "";
        private string filtroTitulo = "";
        private string filtroTipo = "";
        private int? filtroAnio;
        private bool cargandoDatos = true;
        private bool CargandoPublicaciones => cargandoDatos;
        private Dictionary<int, int> DocumentosPorPublicacion = new();
        private int CantidadDocumentos = 0;
        private ModalPublicacion? ModalPublicacionRef;

        private readonly string[] TiposPublicacion = {
            "Artículo de Revista",
            "Libro",
            "Capítulo de Libro",
            "Memoria de Congreso",
            "Tesis",
            "Reporte Técnico",
            "Otro"
        };

        protected override async Task OnInitializedAsync()
        {
            try
            {
                await CargarDatosDocente();
                await CargarPublicaciones();
                await CargarConteoDocumentos();
            }
            catch (Exception ex)
            {
                await JSRuntime.MsgError(new ResultadoAcciones { Mensajes = new List<string> { $"Error al cargar datos: {ex.Message}" }, Resultado = false });
            }
            finally
            {
                cargandoDatos = false;
                StateHasChanged();
            }
        }

        private async Task CargarDatosDocente()
        {
            var resultadoDocente = await DocenteServicios.ObtenerDocente<DocenteDTO>(IdDocente);
            if (resultadoDocente.Resultado && resultadoDocente.Entidad != null)
            {
                var docente = resultadoDocente.Entidad;
                nombreCompletoDocente = $"{docente.NombreDocente} {docente.PaternoDocente} {docente.MaternoDocente}".Trim();
                numeroEmpleado = docente.NumeroEmpleado ?? "";
            }
        }

        private async Task CargarPublicaciones()
        {
            var publicacionesEntidades = await PublicacionServicios.GetAllAsync();

            ListaPublicaciones = publicacionesEntidades
                .Where(p => p.IdDocente == IdDocente)
                .Select(p => new PublicacionDTO
                {
                    IdPublicacion = p.IdPublicacion,
                    IdDocente = p.IdDocente,
                    Titulo = p.Titulo,
                    TipoPublicacion = p.TipoPublicacion,
                    Autores = p.Autores,
                    Fuente = p.Fuente,
                    Anio = p.Anio,
                    Enlace = p.Enlace,
                    NombreDocente = nombreCompletoDocente
                }).ToList();

            PublicacionesFiltradas = ListaPublicaciones;
        }

        private async Task CargarConteoDocumentos()
        {
            try
            {
                var todosDocumentos = await DocumentoServicios.GetAllAsync();
                var documentosDocente = todosDocumentos.Where(d => d.IdDocente == IdDocente);
                CantidadDocumentos = documentosDocente.Count(d => d.IdPublicacion.HasValue);

                DocumentosPorPublicacion = documentosDocente
                    .Where(d => d.IdPublicacion.HasValue)
                    .GroupBy(d => d.IdPublicacion!.Value)
                    .ToDictionary(g => g.Key, g => g.Count());
            }
            catch (Exception ex)
            {
                await JSRuntime.InvokeVoidAsync("console.error", $"Error al cargar documentos: {ex.Message}");
            }
        }

        private void FiltrarPublicaciones()
        {
            PublicacionesFiltradas = ListaPublicaciones.Where(p =>
                (string.IsNullOrEmpty(filtroTitulo) ||
                 (p.Titulo?.Contains(filtroTitulo, StringComparison.OrdinalIgnoreCase) ?? false) ||
                 (p.Autores?.Contains(filtroTitulo, StringComparison.OrdinalIgnoreCase) ?? false) ||
                 (p.Fuente?.Contains(filtroTitulo, StringComparison.OrdinalIgnoreCase) ?? false)) &&
                (string.IsNullOrEmpty(filtroTipo) || p.TipoPublicacion == filtroTipo)
            ).ToList();
            StateHasChanged();
        }

        private void LimpiarFiltros()
        {
            filtroTitulo = string.Empty;
            filtroTipo = string.Empty;
            FiltrarPublicaciones();
        }

        private async Task AbrirModalEditar(PublicacionDTO publicacion)
        {
            try
            {
                await JSRuntime.InvokeVoidAsync("console.log", $"Abriendo modal editar publicación ID: {publicacion.IdPublicacion}");

                if (ModalPublicacionRef is not null)
                {
                    await ModalPublicacionRef.AbrirModal(publicacion);
                }
                else
                {
                    await JSRuntime.InvokeVoidAsync("console.error", "ModalPublicacionRef es null");
                    await JSRuntime.MsgError(new ResultadoAcciones { Mensajes = new List<string> { "No se pudo acceder al formulario de publicación" }, Resultado = false });
                }
            }
            catch (Exception ex)
            {
                await JSRuntime.InvokeVoidAsync("console.error", $"Error al abrir modal editar: {ex.Message}");
                await JSRuntime.MsgError(new ResultadoAcciones { Mensajes = new List<string> { $"Error al abrir el formulario de edición: {ex.Message}" }, Resultado = false });
            }
        }

        private async Task AbrirModalNuevo()
        {
            try
            {
                if (ModalPublicacionRef is not null)
                {
                    await ModalPublicacionRef.AbrirModal(null, IdDocente);
                }
                else
                {
                    await JSRuntime.InvokeVoidAsync("console.error", "ModalPublicacionRef es null");
                    await JSRuntime.MsgError(new ResultadoAcciones { Mensajes = new List<string> { "No se pudo acceder al formulario de publicación" }, Resultado = false });
                }
            }
            catch (Exception ex)
            {
                await JSRuntime.InvokeVoidAsync("console.error", $"Error al abrir modal nuevo: {ex.Message}");
                await JSRuntime.MsgError(new ResultadoAcciones { Mensajes = new List<string> { $"Error al abrir el formulario: {ex.Message}" }, Resultado = false });
            }
        }

        private async Task OnPublicacionGuardada(PublicacionDTO publicacion)
        {
            try
            {
                await JSRuntime.InvokeVoidAsync("console.log", "Recargando datos después de guardar publicación");
                await CargarPublicaciones();
                await CargarConteoDocumentos();
                StateHasChanged();
            }
            catch (Exception ex)
            {
                await JSRuntime.InvokeVoidAsync("console.error", $"Error al recargar datos: {ex.Message}");
                await JSRuntime.MsgPrecaucion($"La publicación se guardó, pero hubo un error al recargar la lista: {ex.Message}");
            }
        }

        private async Task ConfirmarEliminar(PublicacionDTO publicacion)
        {
            var confirmado = await JSRuntime.InvokeAsync<bool>("SweetAlertHelper.showConfirmation",
                "¿Eliminar publicación?", $"¿Está seguro de que desea eliminar la publicación '{publicacion.Titulo}'? Esta acción no se puede deshacer.");

            if (confirmado)
            {
                await EliminarPublicacion(publicacion.IdPublicacion);
            }
        }

        private async Task EliminarPublicacion(int idPublicacion)
        {
            try
            {
                var documentos = await DocumentoServicios.GetDocumentosByPublicacionAsync(idPublicacion);

                foreach (var documento in documentos)
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(documento.Url))
                        {
                            string? driveId = null;
                            try
                            {
                                var uri = new Uri(documento.Url);
                                if (uri.Host.Contains("drive.google.com"))
                                {
                                    if (uri.AbsolutePath.Contains("/file/d/"))
                                    {
                                        driveId = uri.AbsolutePath.Split("/file/d/")[1].Split('/')[0];
                                    }
                                    else if (uri.Query.Contains("id="))
                                    {
                                        driveId = System.Web.HttpUtility.ParseQueryString(uri.Query).Get("id");
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                await JSRuntime.InvokeVoidAsync("console.error", $"Error al extraer ID de Drive: {ex.Message}");
                            }

                            if (!string.IsNullOrEmpty(driveId))
                            {
                                await DocumentRepository.EliminarDocumentoAsync(driveId);
                            }
                        }

                        await DocumentoServicios.DeleteAsync(documento.IdDocumento);
                    }
                    catch (Exception docEx)
                    {
                        await JSRuntime.InvokeVoidAsync("console.error", $"Error al eliminar documento {documento.IdDocumento}: {docEx.Message}");
                    }
                }

                await PublicacionServicios.DeleteAsync(idPublicacion);
                await CargarPublicaciones();
                await CargarConteoDocumentos();
                await JSRuntime.MsgExito("Publicación y sus documentos eliminados correctamente");
            }
            catch (Exception ex)
            {
                await JSRuntime.MsgError(new ResultadoAcciones { Mensajes = new List<string> { $"Error al eliminar publicación: {ex.Message}" }, Resultado = false });
            }
        }

        private void VerDocumentos(int idPublicacion)
        {
            NavigationManager.NavigateTo($"/CV/Docentes/{IdDocente}/Publicaciones/{idPublicacion}/Documentos");
        }

        private async Task ExportarPublicaciones()
        {
            await JSRuntime.MsgInfo("Funcionalidad de exportación en desarrollo");
        }

        private void VolverAExperiencia()
        {
            NavigationManager.NavigateTo($"/CV/Docentes/{IdDocente}/Experiencia");
        }

        private void ContinuarAProyectos()
        {
            NavigationManager.NavigateTo($"/CV/Docentes/{IdDocente}/Proyectos");
        }

        private void FinalizarGestion()
        {
            NavigationManager.NavigateTo($"/CV/Docentes/Ver/{IdDocente}");
        }

        private void VolverAPerfilDocente()
        {
            NavigationManager.NavigateTo($"/CV/Docentes/Ver/{IdDocente}");
        }
    }
} 