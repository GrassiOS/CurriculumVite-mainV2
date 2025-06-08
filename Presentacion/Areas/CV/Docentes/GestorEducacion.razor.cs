using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Entidades.DTO.CurriculumVite;
using Servicios.IRepositorios.CurriculumVite;
using Presentacion.Components.Shared;
using Servicios.IRepositorios;
using Presentacion.Helper;
using Entidades.Generales;

namespace Presentacion.Areas.CV.Docentes
{
    public partial class GestorEducacion : ComponentBase
    {
        [Parameter] public int IdDocente { get; set; }

        [Inject] private ISRepositorioEducacion EducacionServicios { get; set; } = default!;
        [Inject] private ISRepositorioDocumento DocumentoServicios { get; set; } = default!;
        [Inject] private ISDocenteRepositorio DocenteServicios { get; set; } = default!;
        [Inject] private IDocumentRepository DocumentRepository { get; set; } = default!;
        [Inject] private IJSRuntime JSRuntime { get; set; } = default!;
        [Inject] private NavigationManager NavigationManager { get; set; } = default!;

        // Estado de carga
        private bool CargandoEducaciones = true;

        // Listas y datos
        private List<EducacionDTO> ListaEducaciones = new();
        private List<EducacionDTO> EducacionesFiltradas = new();
        private List<string> NivelesEducacion = new();
        private int CantidadDocumentos = 0;

        // Variables para el modal
        private ModalEducacion modalEducacion = default!;
        private EducacionDTO EducacionSeleccionada = new();
        private bool EsModoEdicion = false;

        // Filtros
        private string FiltroTexto = string.Empty;
        private string FiltroNivel = string.Empty;

        // Información del docente
        private string nombreCompletoDocente = string.Empty;
        private string numeroEmpleado = string.Empty;

        // Detección de parámetro fromProfile
        private bool FromProfile = false;

        protected override async Task OnInitializedAsync()
        {
            await DetectarFromProfile();
            
            try
            {
                await CargarDatosIniciales();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en OnInitializedAsync: {ex.Message}");
            }
            finally
            {
                CargandoEducaciones = false;
                StateHasChanged();
            }
        }

        private async Task DetectarFromProfile()
        {
            try
            {
                var uri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);
                if (uri.Query.Contains("fromProfile=true"))
                {
                    FromProfile = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al detectar fromProfile: {ex.Message}");
                FromProfile = false;
            }
            
            await Task.CompletedTask; // Para hacer el método async
        }

        private async Task CargarDatosIniciales()
        {
            try
            {
                // Cargar información del docente
                var resultadoDocente = await DocenteServicios.ObtenerDocente<DocenteDTO>(IdDocente);
                if (resultadoDocente.Resultado && resultadoDocente.Entidad != null)
                {
                    var docente = resultadoDocente.Entidad;
                    nombreCompletoDocente = $"{docente.NombreDocente} {docente.PaternoDocente} {docente.MaternoDocente}".Trim();
                    numeroEmpleado = docente.NumeroEmpleado ?? "";
                }

                // Cargar educaciones
                await CargarEducaciones();
            }
            catch (Exception ex)
            {
                try
                {
                    await JSRuntime.InvokeVoidAsync("console.error", $"Error al cargar datos iniciales: {ex.Message}");
                }
                catch
                {
                    // Ignorar errores de JSRuntime si el circuito está desconectado
                    Console.WriteLine($"Error al cargar datos iniciales: {ex.Message}");
                }
            }
        }

        private async Task CargarEducaciones()
        {
            try
            {
                var educacionesEntidades = await EducacionServicios.GetAllAsync();
                
                ListaEducaciones = educacionesEntidades
                    .Where(e => e.IdDocente == IdDocente)
                    .Select(e => new EducacionDTO
                    {
                        IdEducacion = e.IdEducacion,
                        IdDocente = e.IdDocente,
                        Nivel = e.Nivel,
                        Titulo = e.Titulo,
                        Institucion = e.Institucion,
                        Especialidad = e.Especialidad,
                        Pais = e.Pais,
                        AnioInicio = e.AnioInicio,
                        AnioFin = e.AnioFin
                    }).ToList();
                    
                // Obtener niveles únicos para el filtro
                NivelesEducacion = ListaEducaciones
                    .Where(e => !string.IsNullOrEmpty(e.Nivel))
                    .Select(e => e.Nivel!)
                    .Distinct()
                    .OrderBy(n => n)
                    .ToList();

                // Cargar cantidad de documentos
                await CargarCantidadDocumentos();
                
                FiltrarEducaciones();
            }
            catch (Exception ex)
            {
                try
                {
                    await JSRuntime.InvokeVoidAsync("console.error", $"Error al cargar educaciones: {ex.Message}");
                }
                catch
                {
                    // Ignorar errores de JSRuntime si el circuito está desconectado
                    Console.WriteLine($"Error al cargar educaciones: {ex.Message}");
                }
            }
        }

        private async Task CargarCantidadDocumentos()
        {
            try
            {
                var todosDocumentos = await DocumentoServicios.GetAllAsync();
                var documentosDocente = todosDocumentos.Where(d => d.IdDocente == IdDocente);
                CantidadDocumentos = documentosDocente.Count(d => d.IdEducacion.HasValue);
            }
            catch (Exception ex)
            {
                try
                {
                    await JSRuntime.InvokeVoidAsync("console.error", $"Error al cargar documentos: {ex.Message}");
                }
                catch
                {
                    // Ignorar errores de JSRuntime si el circuito está desconectado
                    Console.WriteLine($"Error al cargar documentos: {ex.Message}");
                }
            }
        }

        private void FiltrarEducaciones()
        {
            var query = ListaEducaciones.AsQueryable();

            if (!string.IsNullOrEmpty(FiltroTexto))
            {
                query = query.Where(e => 
                    (e.Titulo != null && e.Titulo.Contains(FiltroTexto, StringComparison.OrdinalIgnoreCase)) ||
                    (e.Institucion != null && e.Institucion.Contains(FiltroTexto, StringComparison.OrdinalIgnoreCase)) ||
                    (e.Especialidad != null && e.Especialidad.Contains(FiltroTexto, StringComparison.OrdinalIgnoreCase)));
            }

            if (!string.IsNullOrEmpty(FiltroNivel))
            {
                query = query.Where(e => e.Nivel == FiltroNivel);
            }

            EducacionesFiltradas = query.OrderByDescending(e => e.AnioFin ?? e.AnioInicio ?? 0).ToList();
            StateHasChanged();
        }

        private void LimpiarFiltros()
        {
            FiltroTexto = string.Empty;
            FiltroNivel = string.Empty;
            FiltrarEducaciones();
        }

        private async Task AbrirModalNuevaEducacion()
        {
            EducacionSeleccionada = new EducacionDTO { IdDocente = IdDocente };
            EsModoEdicion = false;
            await modalEducacion.AbrirModal(null, IdDocente);
        }

        private async Task EditarEducacion(EducacionDTO educacion)
        {
            EducacionSeleccionada = educacion;
            EsModoEdicion = true;
            await modalEducacion.AbrirModal(educacion);
        }

        private async Task OnEducacionGuardada()
        {
            await CargarEducaciones();
            // modalEducacion?.CerrarModal(); // Removido porque CerrarModal no es público
        }

        private async Task EliminarEducacion(EducacionDTO educacion)
        {
            var confirmado = await JSRuntime.InvokeAsync<bool>("SweetAlertHelper.showConfirmation",
                "¿Eliminar formación académica?", $"¿Está seguro de que desea eliminar esta educación: {educacion.Titulo ?? educacion.Nivel}? Si hay documentos asociados, también serán eliminados. Esta acción no se puede deshacer.");
            
            if (confirmado)
            {
                try
                {
                    // 1. Obtener todos los documentos asociados
                    var documentos = await DocumentoServicios.GetAllAsync();
                    var documentosEducacion = documentos.Where(d => d.IdEducacion == educacion.IdEducacion).ToList();

                    // 2. Eliminar cada documento de Drive y de la base de datos
                    foreach (var documento in documentosEducacion)
                    {
                        if (!string.IsNullOrEmpty(documento.Url))
                        {
                            try
                            {
                                // Extraer ID de Drive
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

                                // Eliminar de Drive si se encontró el ID
                                if (!string.IsNullOrEmpty(driveId))
                                {
                                    await DocumentRepository.EliminarDocumentoAsync(driveId);
                                }
                            }
                            catch (Exception ex)
                            {
                                await JSRuntime.InvokeVoidAsync("console.error", $"Error al eliminar archivo de Drive: {ex.Message}");
                                // Continuar con el siguiente documento
                            }
                        }

                        // Eliminar el documento de la base de datos
                        await DocumentoServicios.DeleteAsync(documento.IdDocumento);
                    }

                    // 3. Finalmente eliminar la educación
                    await EducacionServicios.DeleteAsync(educacion.IdEducacion);
                    await JSRuntime.MsgExito("Educación y documentos asociados eliminados exitosamente");
                    await CargarEducaciones();
                }
                catch (Exception ex)
                {
                    await JSRuntime.MsgError(new ResultadoAcciones { Mensajes = new List<string> { $"Error al eliminar: {ex.Message}" }, Resultado = false });
                }
            }
        }

        private void VerDocumentosEducacion(EducacionDTO educacion)
        {
            NavigationManager.NavigateTo($"/CV/Docentes/{IdDocente}/Educacion/{educacion.IdEducacion}/Documentos");
        }

        private async Task ExportarEducaciones()
        {
            // Implementar exportación
            await JSRuntime.MsgInfo("Función de exportación en desarrollo");
        }

        private string GetColorNivel(string nivel)
        {
            return nivel switch
            {
                "Licenciatura" => "#3B82F6",
                "Maestría" => "#8B5CF6", 
                "Doctorado" => "#DC2626",
                "Posdoctorado" => "#059669",
                "Especialidad" => "#F59E0B",
                _ => "#6B7280"
            };
        }

        // Métodos de navegación
        private void VolverAPerfilDocente()
        {
            NavigationManager.NavigateTo($"/CV/Docentes/Ver/{IdDocente}");
        }

        private void RegresarAPublicaciones()
        {
            NavigationManager.NavigateTo($"/CV/Docentes/{IdDocente}/Publicaciones");
        }

        private void ContinuarAExperiencia()
        {
            NavigationManager.NavigateTo($"/CV/Docentes/{IdDocente}/Experiencia");
        }
    }
} 