using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Entidades.DTO.CurriculumVite;
using Servicios.IRepositorios.CurriculumVite;
using Presentacion.Helper;
using Entidades.Generales;

namespace Presentacion.Areas.CV.Docentes
{
    public partial class VerEducacion : ComponentBase
    {
        [Parameter] public int IdDocente { get; set; }

        [Inject] private ISRepositorioEducacion EducacionServicios { get; set; } = default!;
        [Inject] private ISDocenteRepositorio DocenteServicios { get; set; } = default!;
        [Inject] private NavigationManager NavigationManager { get; set; } = default!;
        [Inject] private IJSRuntime JSRuntime { get; set; } = default!;

        private List<EducacionDTO> ListaEducacion = new();
        private List<EducacionDTO> EducacionFiltrada = new();
        private string NombreDocente = "";
        private bool CargandoEducacion = true;

        // Filtros
        private string FiltroTexto = "";
        private string FiltroNivel = "";
        private string OrdenarPor = "anio";

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
                CargandoEducacion = true;
                StateHasChanged();

                // Cargar docente
                var resultadoDocente = await DocenteServicios.ObtenerDocente<DocenteDTO>(IdDocente);
                if (resultadoDocente.Resultado && resultadoDocente.Entidad != null)
                {
                    var docente = resultadoDocente.Entidad;
                    NombreDocente = $"{docente.NombreDocente} {docente.PaternoDocente} {docente.MaternoDocente}".Trim();
                }

                // Cargar educación
                var educacion = await EducacionServicios.GetAllAsync();
                if (educacion != null)
                {
                    ListaEducacion = educacion
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
                            AnioFin = e.AnioFin,
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
                CargandoEducacion = false;
                StateHasChanged();
            }
        }

        private void AplicarFiltros()
        {
            var educacionFiltrada = ListaEducacion.AsEnumerable();

            // Filtro por texto
            if (!string.IsNullOrEmpty(FiltroTexto))
            {
                educacionFiltrada = educacionFiltrada.Where(e =>
                    (e.Titulo?.Contains(FiltroTexto, StringComparison.OrdinalIgnoreCase) ?? false) ||
                    (e.Institucion?.Contains(FiltroTexto, StringComparison.OrdinalIgnoreCase) ?? false) ||
                    (e.Especialidad?.Contains(FiltroTexto, StringComparison.OrdinalIgnoreCase) ?? false) ||
                    (e.Nivel?.Contains(FiltroTexto, StringComparison.OrdinalIgnoreCase) ?? false) ||
                    (e.Pais?.Contains(FiltroTexto, StringComparison.OrdinalIgnoreCase) ?? false)
                );
            }

            // Filtro por nivel
            if (!string.IsNullOrEmpty(FiltroNivel))
            {
                educacionFiltrada = educacionFiltrada.Where(e => e.Nivel == FiltroNivel);
            }

            // Ordenar
            educacionFiltrada = OrdenarPor switch
            {
                "nivel" => educacionFiltrada.OrderBy(e => GetOrdenNivel(e.Nivel)).ThenByDescending(e => e.AnioFin),
                "institucion" => educacionFiltrada.OrderBy(e => e.Institucion).ThenByDescending(e => e.AnioFin),
                _ => educacionFiltrada.OrderByDescending(e => e.AnioFin).ThenByDescending(e => e.AnioInicio)
            };

            EducacionFiltrada = educacionFiltrada.ToList();
            StateHasChanged();
        }

        private void LimpiarFiltros()
        {
            FiltroTexto = "";
            FiltroNivel = "";
            OrdenarPor = "anio";
            AplicarFiltros();
        }

        private void RegresarAlPerfil()
        {
            NavigationManager.NavigateTo($"/CV/Docentes/Ver/{IdDocente}");
        }

        private string GetColorNivel(string? nivel)
        {
            return nivel?.ToLower() switch
            {
                var n when n?.Contains("doctorado") == true => "#8B5CF6",
                var n when n?.Contains("maestr") == true => "#4BB463",
                var n when n?.Contains("licenciatur") == true => "#F4BF3A",
                var n when n?.Contains("ingenier") == true => "#F4BF3A",
                var n when n?.Contains("especializaci") == true => "#2D6B3C",
                _ => "#6B7280"
            };
        }

        private int GetOrdenNivel(string? nivel)
        {
            return nivel?.ToLower() switch
            {
                var n when n?.Contains("doctorado") == true => 4,
                var n when n?.Contains("maestr") == true => 3,
                var n when n?.Contains("especializaci") == true => 2,
                var n when n?.Contains("licenciatur") == true => 1,
                var n when n?.Contains("ingenier") == true => 1,
                _ => 0
            };
        }
    }
} 