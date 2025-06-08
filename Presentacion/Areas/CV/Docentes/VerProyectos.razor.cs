using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Entidades.DTO.CurriculumVite;
using Servicios.IRepositorios.CurriculumVite;

namespace Presentacion.Areas.CV.Docentes
{
    public partial class VerProyectos : ComponentBase
    {
        [Parameter] public int IdDocente { get; set; }

        [Inject] private ISRepositorioProyecto ProyectoServicios { get; set; } = default!;
        [Inject] private ISDocenteRepositorio DocenteServicios { get; set; } = default!;
        [Inject] private NavigationManager NavigationManager { get; set; } = default!;
        [Inject] private IJSRuntime JSRuntime { get; set; } = default!;

        private List<ProyectoDTO> ListaProyectos = new();
        private List<ProyectoDTO> ProyectosFiltrados = new();
        private string NombreDocente = "";
        private bool CargandoProyectos = true;

        // Filtros
        private string FiltroTexto = "";
        private string FiltroEstado = "";
        private string OrdenarPor = "periodo";

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
                CargandoProyectos = true;
                StateHasChanged();

                // Cargar docente
                var resultadoDocente = await DocenteServicios.ObtenerDocente<DocenteDTO>(IdDocente);
                if (resultadoDocente.Resultado && resultadoDocente.Entidad != null)
                {
                    var docente = resultadoDocente.Entidad;
                    NombreDocente = $"{docente.NombreDocente} {docente.PaternoDocente} {docente.MaternoDocente}".Trim();
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

            // Filtro por texto
            if (!string.IsNullOrEmpty(FiltroTexto))
            {
                proyectosFiltrados = proyectosFiltrados.Where(p =>
                    (p.Titulo?.Contains(FiltroTexto, StringComparison.OrdinalIgnoreCase) ?? false) ||
                    (p.Institucion?.Contains(FiltroTexto, StringComparison.OrdinalIgnoreCase) ?? false) ||
                    (p.Rol?.Contains(FiltroTexto, StringComparison.OrdinalIgnoreCase) ?? false) ||
                    (p.Financiamiento?.Contains(FiltroTexto, StringComparison.OrdinalIgnoreCase) ?? false)
                );
            }

            // Filtro por estado
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

            // Ordenar
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

        private void RegresarAlPerfil()
        {
            NavigationManager.NavigateTo($"/CV/Docentes/Ver/{IdDocente}");
        }
    }
} 