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
    public partial class GestorExperiencia : ComponentBase
    {
        [Parameter] public int IdDocente { get; set; }

        [Inject] private ISRepositorioExperiencia ExperienciaServicios { get; set; } = default!;
        [Inject] private ISRepositorioDocumento DocumentoServicios { get; set; } = default!;
        [Inject] private IDocumentRepository DocumentRepository { get; set; } = default!;
        [Inject] private ISDocenteRepositorio DocenteServicios { get; set; } = default!;
        [Inject] private IJSRuntime JSRuntime { get; set; } = default!;
        [Inject] private NavigationManager NavigationManager { get; set; } = default!;

        // Variables principales
        private List<ExperienciaDTO> ListaExperiencia = new();
        private List<ExperienciaDTO> ExperienciasFiltradas = new();
        private string NombreDocente = "";
        private string nombreCompletoDocente = "";
        private bool CargandoExperiencia = true;
        private ModalExperiencia modalExperiencia = default!;
        private string numeroEmpleado = "";

        // Detectar si viene del perfil del docente
        private bool FromProfile = false;

        // Filtros
        private string FiltroTexto = "";
        private string FiltroEstado = "";
        private string OrdenarPor = "fecha";

        // Modal de confirmación
        private bool MostrarModalConfirmacion = false;
        private ExperienciaDTO? ExperienciaAEliminar;

        protected override async Task OnInitializedAsync()
        {
            // Detectar si viene del perfil del docente
            var uri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);
            FromProfile = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(uri.Query).ContainsKey("fromProfile");

            await CargarDatos();
        }

        protected override async Task OnParametersSetAsync()
        {
            if (IdDocente > 0)
            {
                await CargarDatos();
            }
        }

        private async Task CargarDatos()
        {
            try
            {
                CargandoExperiencia = true;
                StateHasChanged();

                // Cargar docente
                var resultadoDocente = await DocenteServicios.ObtenerDocente<DocenteDTO>(IdDocente);
                if (resultadoDocente.Resultado && resultadoDocente.Entidad != null)
                {
                    var docente = resultadoDocente.Entidad;
                    NombreDocente = $"{docente.NombreDocente} {docente.PaternoDocente} {docente.MaternoDocente}".Trim();
                    nombreCompletoDocente = NombreDocente;
                    numeroEmpleado = docente.NumeroEmpleado ?? "";
                }

                // Cargar experiencias
                var experiencias = await ExperienciaServicios.GetAllAsync();
                if (experiencias != null)
                {
                    ListaExperiencia = experiencias
                        .Where(e => e.IdDocente == IdDocente)
                        .Select(e => new ExperienciaDTO
                        {
                            IdExperiencia = e.IdExperiencia,
                            IdDocente = e.IdDocente,
                            Puesto = e.Puesto,
                            Institucion = e.Institucion,
                            Descripcion = e.Descripcion,
                            FechaInicio = e.FechaInicio,
                            FechaFin = e.FechaFin,
                            NombreDocente = NombreDocente
                        })
                        .ToList();

                    AplicarFiltros();
                }
            }
            catch (Exception ex)
            {
                await JSRuntime.MsgError(new ResultadoAcciones { Mensajes = new List<string> { $"Error al cargar datos: {ex.Message}" }, Resultado = false });
            }
            finally
            {
                CargandoExperiencia = false;
                StateHasChanged();
            }
        }

        private void AplicarFiltros()
        {
            var experienciasFiltradas = ListaExperiencia.AsEnumerable();

            if (!string.IsNullOrEmpty(FiltroTexto))
            {
                experienciasFiltradas = experienciasFiltradas.Where(e =>
                    (e.Puesto?.Contains(FiltroTexto, StringComparison.OrdinalIgnoreCase) ?? false) ||
                    (e.Institucion?.Contains(FiltroTexto, StringComparison.OrdinalIgnoreCase) ?? false) ||
                    (e.Descripcion?.Contains(FiltroTexto, StringComparison.OrdinalIgnoreCase) ?? false)
                );
            }

            if (!string.IsNullOrEmpty(FiltroEstado))
            {
                experienciasFiltradas = FiltroEstado switch
                {
                    "actual" => experienciasFiltradas.Where(e => e.EsActual),
                    "pasada" => experienciasFiltradas.Where(e => !e.EsActual),
                    _ => experienciasFiltradas
                };
            }

            experienciasFiltradas = OrdenarPor switch
            {
                "puesto" => experienciasFiltradas.OrderBy(e => e.Puesto),
                "institucion" => experienciasFiltradas.OrderBy(e => e.Institucion),
                "duracion" => experienciasFiltradas.OrderByDescending(e => e.DuracionMeses),
                _ => experienciasFiltradas.OrderByDescending(e => e.FechaInicio ?? DateTime.MinValue)
            };

            ExperienciasFiltradas = experienciasFiltradas.ToList();
            StateHasChanged();
        }

        private void LimpiarFiltros()
        {
            FiltroTexto = "";
            FiltroEstado = "";
            OrdenarPor = "fecha";
            AplicarFiltros();
        }

        private async Task AbrirModalAgregar()
        {
            await modalExperiencia.AbrirModal(null, IdDocente);
        }

        private async Task AbrirModalEditar(ExperienciaDTO experiencia)
        {
            await modalExperiencia.AbrirModal(experiencia);
        }

        private async Task OnExperienciaGuardada()
        {
            await CargarDatos();
        }

        private void ConfirmarEliminar(ExperienciaDTO experiencia)
        {
            ExperienciaAEliminar = experiencia;
            MostrarModalConfirmacion = true;
            StateHasChanged();
        }

        private void CerrarModalConfirmacion()
        {
            MostrarModalConfirmacion = false;
            ExperienciaAEliminar = null;
            StateHasChanged();
        }

        private async Task EliminarExperiencia()
        {
            if (ExperienciaAEliminar != null)
            {
                try
                {
                    var todosDocumentos = await DocumentoServicios.GetAllAsync();
                    var documentosExperiencia = todosDocumentos
                        .Where(d => d.IdDocente == IdDocente &&
                                   !string.IsNullOrEmpty(d.Descripcion) &&
                                   d.Descripcion.Contains($"ID Experiencia: {ExperienciaAEliminar.IdExperiencia}"))
                        .ToList();

                    foreach (var documento in documentosExperiencia)
                    {
                        if (!string.IsNullOrEmpty(documento.Url))
                        {
                            try
                            {
                                string? driveId = null;
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

                                if (!string.IsNullOrEmpty(driveId))
                                {
                                    await DocumentRepository.EliminarDocumentoAsync(driveId);
                                }
                            }
                            catch (Exception ex)
                            {
                                await JSRuntime.InvokeVoidAsync("console.error", $"Error al eliminar archivo de Drive: {ex.Message}");
                            }
                        }

                        await DocumentoServicios.DeleteAsync(documento.IdDocumento);
                    }

                    await ExperienciaServicios.DeleteAsync(ExperienciaAEliminar.IdExperiencia);
                    await JSRuntime.MsgExito("Experiencia eliminada exitosamente");
                    await CargarDatos();
                }
                catch (Exception ex)
                {
                    await JSRuntime.MsgError(new ResultadoAcciones { Mensajes = new List<string> { $"Error al eliminar: {ex.Message}" }, Resultado = false });
                }
                finally
                {
                    CerrarModalConfirmacion();
                }
            }
        }

        private void RegresarAEducacion()
        {
            NavigationManager.NavigateTo($"/CV/Docentes/{IdDocente}/Educacion");
        }

        private void ContinuarAPublicaciones()
        {
            NavigationManager.NavigateTo($"/CV/Docentes/{IdDocente}/Publicaciones");
        }

        private void FinalizarGestion()
        {
            NavigationManager.NavigateTo($"/CV/Docentes/Ver/{IdDocente}");
        }

        private void VolverAPerfilDocente()
        {
            NavigationManager.NavigateTo($"/CV/Docentes/Ver/{IdDocente}");
        }

        private void VerDocumentosExperiencia(ExperienciaDTO experiencia)
        {
            NavigationManager.NavigateTo($"/CV/Docentes/{IdDocente}/Experiencia/{experiencia.IdExperiencia}/Documentos");
        }

        private async Task ExportarExperiencias()
        {
            await JSRuntime.MsgInfo("Función de exportación en desarrollo");
        }
    }
} 