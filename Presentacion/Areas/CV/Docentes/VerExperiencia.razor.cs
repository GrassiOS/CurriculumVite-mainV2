using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Entidades.DTO.CurriculumVite;
using Servicios.IRepositorios.CurriculumVite;

namespace Presentacion.Areas.CV.Docentes
{
    public partial class VerExperiencia : ComponentBase
    {
        [Parameter] public int IdDocente { get; set; }

        [Inject] private ISRepositorioExperiencia ExperienciaServicios { get; set; } = default!;
        [Inject] private ISDocenteRepositorio DocenteServicios { get; set; } = default!;
        [Inject] private NavigationManager NavigationManager { get; set; } = default!;
        [Inject] private IJSRuntime JSRuntime { get; set; } = default!;

        private List<ExperienciaDTO> ListaExperiencia = new();
        private List<ExperienciaDTO> ExperienciaFiltrada = new();
        private string NombreDocente = "";
        private bool CargandoExperiencia = true;

        // Filtros
        private string FiltroTexto = "";
        private string FiltroEstado = "";
        private string OrdenarPor = "fecha";

        protected override async Task OnInitializedAsync()
        {
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
                }

                // Cargar experiencia
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
                await JSRuntime.InvokeVoidAsync("alert", $"Error al cargar datos: {ex.Message}");
            }
            finally
            {
                CargandoExperiencia = false;
                StateHasChanged();
            }
        }

        private void AplicarFiltros()
        {
            var experienciaFiltrada = ListaExperiencia.AsEnumerable();

            // Filtro por texto
            if (!string.IsNullOrEmpty(FiltroTexto))
            {
                experienciaFiltrada = experienciaFiltrada.Where(e =>
                    (e.Puesto?.Contains(FiltroTexto, StringComparison.OrdinalIgnoreCase) ?? false) ||
                    (e.Institucion?.Contains(FiltroTexto, StringComparison.OrdinalIgnoreCase) ?? false) ||
                    (e.Descripcion?.Contains(FiltroTexto, StringComparison.OrdinalIgnoreCase) ?? false)
                );
            }

            // Filtro por estado
            if (!string.IsNullOrEmpty(FiltroEstado))
            {
                experienciaFiltrada = FiltroEstado switch
                {
                    "actual" => experienciaFiltrada.Where(e => e.EsActual),
                    "anterior" => experienciaFiltrada.Where(e => !e.EsActual),
                    _ => experienciaFiltrada
                };
            }

            // Ordenar
            experienciaFiltrada = OrdenarPor switch
            {
                "institucion" => experienciaFiltrada.OrderBy(e => e.Institucion).ThenByDescending(e => e.FechaInicio),
                "duracion" => experienciaFiltrada.OrderByDescending(e => e.DuracionMeses),
                _ => experienciaFiltrada.OrderByDescending(e => e.FechaInicio ?? DateTime.MinValue)
            };

            ExperienciaFiltrada = experienciaFiltrada.ToList();
            StateHasChanged();
        }

        private void LimpiarFiltros()
        {
            FiltroTexto = "";
            FiltroEstado = "";
            OrdenarPor = "fecha";
            AplicarFiltros();
        }

        private void RegresarAlPerfil()
        {
            NavigationManager.NavigateTo($"/CV/Docentes/Ver/{IdDocente}");
        }
    }
} 