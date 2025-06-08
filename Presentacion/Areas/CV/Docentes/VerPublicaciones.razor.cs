using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Entidades.DTO.CurriculumVite;
using Servicios.IRepositorios.CurriculumVite;

namespace Presentacion.Areas.CV.Docentes
{
    public partial class VerPublicaciones : ComponentBase
    {
        [Parameter] public int IdDocente { get; set; }

        [Inject] private ISRepositorioPublicacion PublicacionServicios { get; set; } = default!;
        [Inject] private ISDocenteRepositorio DocenteServicios { get; set; } = default!;
        [Inject] private NavigationManager NavigationManager { get; set; } = default!;
        [Inject] private IJSRuntime JSRuntime { get; set; } = default!;

        private List<PublicacionDTO> ListaPublicaciones = new();
        private List<PublicacionDTO> PublicacionesFiltradas = new();
        private string NombreDocente = "";
        private bool CargandoPublicaciones = true;

        // Filtros
        private string FiltroTexto = "";
        private string FiltroTipo = "";
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
                CargandoPublicaciones = true;
                StateHasChanged();

                // Cargar docente
                var resultadoDocente = await DocenteServicios.ObtenerDocente<DocenteDTO>(IdDocente);
                if (resultadoDocente.Resultado && resultadoDocente.Entidad != null)
                {
                    var docente = resultadoDocente.Entidad;
                    NombreDocente = $"{docente.NombreDocente} {docente.PaternoDocente} {docente.MaternoDocente}".Trim();
                }

                // Cargar publicaciones
                var publicaciones = await PublicacionServicios.GetAllAsync();
                if (publicaciones != null)
                {
                    ListaPublicaciones = publicaciones
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
                CargandoPublicaciones = false;
                StateHasChanged();
            }
        }

        private void AplicarFiltros()
        {
            var publicacionesFiltradas = ListaPublicaciones.AsEnumerable();

            // Filtro por texto
            if (!string.IsNullOrEmpty(FiltroTexto))
            {
                publicacionesFiltradas = publicacionesFiltradas.Where(p =>
                    (p.Titulo?.Contains(FiltroTexto, StringComparison.OrdinalIgnoreCase) ?? false) ||
                    (p.Autores?.Contains(FiltroTexto, StringComparison.OrdinalIgnoreCase) ?? false) ||
                    (p.Fuente?.Contains(FiltroTexto, StringComparison.OrdinalIgnoreCase) ?? false) ||
                    (p.TipoPublicacion?.Contains(FiltroTexto, StringComparison.OrdinalIgnoreCase) ?? false)
                );
            }

            // Filtro por tipo
            if (!string.IsNullOrEmpty(FiltroTipo))
            {
                publicacionesFiltradas = publicacionesFiltradas.Where(p => p.TipoPublicacion == FiltroTipo);
            }

            // Ordenar
            publicacionesFiltradas = OrdenarPor switch
            {
                "titulo" => publicacionesFiltradas.OrderBy(p => p.Titulo),
                "tipo" => publicacionesFiltradas.OrderBy(p => p.TipoPublicacion).ThenByDescending(p => p.Anio),
                _ => publicacionesFiltradas.OrderByDescending(p => p.Anio)
            };

            PublicacionesFiltradas = publicacionesFiltradas.ToList();
            StateHasChanged();
        }

        private void LimpiarFiltros()
        {
            FiltroTexto = "";
            FiltroTipo = "";
            OrdenarPor = "anio";
            AplicarFiltros();
        }

        private void RegresarAlPerfil()
        {
            NavigationManager.NavigateTo($"/CV/Docentes/Ver/{IdDocente}");
        }
    }
} 