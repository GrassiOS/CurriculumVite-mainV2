using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Entidades.DTO.CurriculumVite;
using Servicios.IRepositorios.CurriculumVite;
using Servicios.IRepositorios;
using Presentacion.Components.Shared;

namespace Presentacion.Areas.CV.Docentes
{
    public partial class GestorProyectos : ComponentBase
    {
        [Parameter] public int IdDocente { get; set; }

        [Inject] private ISRepositorioProyecto ProyectoServicios { get; set; } = default!;
        [Inject] private ISRepositorioDocumento DocumentoServicios { get; set; } = default!;
        [Inject] private IDocumentRepository DocumentRepository { get; set; } = default!;
        [Inject] private ISDocenteRepositorio DocenteServicios { get; set; } = default!;
        [Inject] private IJSRuntime JSRuntime { get; set; } = default!;
        [Inject] private NavigationManager NavigationManager { get; set; } = default!;

        // Variables principales
        private List<ProyectoDTO> ListaProyectos = new();
        private List<ProyectoDTO> ProyectosFiltrados = new();
        private string NombreDocente = "";
        private string nombreCompletoDocente = "";
        private bool CargandoProyectos = true;
        private ModalProyecto? modalProyecto;
        private string numeroEmpleado = "";

        // Detectar si viene del perfil del docente
        private bool FromProfile = false;

        // Filtros
        private string FiltroTexto = "";
        private string FiltroEstado = "";
        private string OrdenarPor = "periodo";

        // Modal de confirmación
        private bool MostrarModalConfirmacion = false;
        private ProyectoDTO? ProyectoAEliminar;

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
                CargandoProyectos = true;
                StateHasChanged();

                // Cargar docente
                var resultadoDocente = await DocenteServicios.ObtenerDocente<DocenteDTO>(IdDocente);
                if (resultadoDocente.Resultado && resultadoDocente.Entidad != null)
                {
                    var docente = resultadoDocente.Entidad;
                    NombreDocente = $"{docente.NombreDocente} {docente.PaternoDocente} {docente.MaternoDocente}".Trim();
                    nombreCompletoDocente = $"{docente.NombreDocente} {docente.PaternoDocente} {docente.MaternoDocente}".Trim();
                    numeroEmpleado = docente.NumeroEmpleado ?? "";
                }

                // Cargar proyectos
                var proyectos = await ProyectoServicios.GetAllAsync();
                if (proyectos != null)
                {
                    ListaProyectos = proyectos
                        .Where(p => p.IdDocente == IdDocente)
                        .Select(p => new ProyectoDTO
                        {
                            IdProyecto = p.IdProyecto,
                            IdDocente = p.IdDocente,
                            Titulo = p.Titulo,
                            Rol = p.Rol,
                            Institucion = p.Institucion,
                            Financiamiento = p.Financiamiento,
                            PeriodoInicio = p.PeriodoInicio,
                            PeriodoFin = p.PeriodoFin,
                            NombreDocente = NombreDocente
                        })
                        .ToList();

                    AplicarFiltros();
                }
            }
            catch (Exception ex)
            {
                await JSRuntime.InvokeVoidAsync("alert", $"Error al cargar datos: {ex.Message}");
            }
            finally
            {
                CargandoProyectos = false;
                StateHasChanged();
            }
        }

        private void AplicarFiltros()
        {
            var proyectosFiltrados = ListaProyectos.AsEnumerable();

            if (!string.IsNullOrEmpty(FiltroTexto))
            {
                proyectosFiltrados = proyectosFiltrados.Where(p =>
                    (p.Titulo?.Contains(FiltroTexto, StringComparison.OrdinalIgnoreCase) ?? false) ||
                    (p.Institucion?.Contains(FiltroTexto, StringComparison.OrdinalIgnoreCase) ?? false) ||
                    (p.Rol?.Contains(FiltroTexto, StringComparison.OrdinalIgnoreCase) ?? false) ||
                    (p.Financiamiento?.Contains(FiltroTexto, StringComparison.OrdinalIgnoreCase) ?? false)
                );
            }

            if (!string.IsNullOrEmpty(FiltroEstado))
            {
                proyectosFiltrados = FiltroEstado switch
                {
                    "activo" => proyectosFiltrados.Where(p => p.EsActivo),
                    "finalizado" => proyectosFiltrados.Where(p => !p.EsActivo),
                    "financiado" => proyectosFiltrados.Where(p => p.TieneFinanciamiento),
                    _ => proyectosFiltrados
                };
            }

            proyectosFiltrados = OrdenarPor switch
            {
                "titulo" => proyectosFiltrados.OrderBy(p => p.Titulo),
                "institucion" => proyectosFiltrados.OrderBy(p => p.Institucion),
                "duracion" => proyectosFiltrados.OrderByDescending(p => p.DuracionAnios),
                _ => proyectosFiltrados.OrderByDescending(p => p.PeriodoFin).ThenByDescending(p => p.PeriodoInicio)
            };

            ProyectosFiltrados = proyectosFiltrados.ToList();
            StateHasChanged();
        }

        private void LimpiarFiltros()
        {
            FiltroTexto = "";
            FiltroEstado = "";
            OrdenarPor = "periodo";
            AplicarFiltros();
        }

        private async Task AbrirModalAgregar()
        {
            await modalProyecto.AbrirModal(null, IdDocente);
        }

        private async Task AbrirModalEditar(ProyectoDTO proyecto)
        {
            await modalProyecto.AbrirModal(proyecto);
        }

        private async Task OnProyectoGuardado()
        {
            await CargarDatos();
        }

        private void ConfirmarEliminar(ProyectoDTO proyecto)
        {
            ProyectoAEliminar = proyecto;
            MostrarModalConfirmacion = true;
            StateHasChanged();
        }

        private void CerrarModalConfirmacion()
        {
            MostrarModalConfirmacion = false;
            ProyectoAEliminar = null;
            StateHasChanged();
        }

        private async Task EliminarProyecto()
        {
            if (ProyectoAEliminar != null)
            {
                try
                {
                    var todosDocumentos = await DocumentoServicios.GetAllAsync();
                    var documentosProyecto = todosDocumentos
                        .Where(d => d.IdDocente == IdDocente && d.IdProyecto == ProyectoAEliminar.IdProyecto)
                        .ToList();

                    foreach (var documento in documentosProyecto)
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

                    await ProyectoServicios.DeleteAsync(ProyectoAEliminar.IdProyecto);
                    await JSRuntime.InvokeVoidAsync("alert", "Proyecto y documentos asociados eliminados exitosamente");
                    await CargarDatos();
                }
                catch (Exception ex)
                {
                    await JSRuntime.InvokeVoidAsync("alert", $"Error al eliminar: {ex.Message}");
                }
                finally
                {
                    CerrarModalConfirmacion();
                }
            }
        }

        private void RegresarAExperiencia()
        {
            NavigationManager.NavigateTo($"/CV/Docentes/{IdDocente}/Experiencia");
        }

        private void FinalizarGestion()
        {
            NavigationManager.NavigateTo($"/CV/Docentes/Ver/{IdDocente}");
        }

        private void VolverAPerfilDocente()
        {
            NavigationManager.NavigateTo($"/CV/Docentes/Ver/{IdDocente}");
        }

        private void VerDocumentosProyecto(ProyectoDTO proyecto)
        {
            NavigationManager.NavigateTo($"/CV/Docentes/{IdDocente}/Proyecto/{proyecto.IdProyecto}/Documentos");
        }
    }
} 