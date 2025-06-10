using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Entidades.Modelos.CurriculumVite;
using Servicios.IRepositorios.CurriculumVite;
using Entidades.Modelos.PlanesDeEstudio.Carreras;
using Servicios.Repositorios.PlanesDeEstudio;

namespace Presentacion.Areas.CV.Docentes
{
    public partial class ListarDocentes : ComponentBase
    {
        [Inject] private ISDocenteRepositorio DocenteServicios { get; set; } = default!;
        [Inject] private CarreraServicios CarreraServicios { get; set; } = default!;
        [Inject] private NavigationManager Navigation { get; set; } = default!;
        [Inject] private IJSRuntime JSRuntime { get; set; } = default!;

        private IEnumerable<E_Docente> docentes = new List<E_Docente>();
        private IEnumerable<E_Docente> docentesPaginados = new List<E_Docente>();
        private List<E_Carrera> carreras = new List<E_Carrera>();
        private string criterioBusqueda = "";
        private int _carreraSeleccionada;
        private int carreraSeleccionada
        {
            get => _carreraSeleccionada;
            set
            {
                if (_carreraSeleccionada != value)
                {
                    _carreraSeleccionada = value;
                    FiltrarDocentesAsync().ConfigureAwait(false);
                }
            }
        }
        private bool cargando = true;
        
        // Paginación
        private int paginaActual = 1;
        private int registrosPorPagina = 10;
        private int totalPaginas => (int)Math.Ceiling(docentes.Count() / (double)registrosPorPagina);

        protected override async Task OnInitializedAsync()
        {
            await CargarDatos();
        }

        protected override async Task OnParametersSetAsync()
        {
            ActualizarPaginacion();
        }

        private async Task CargarDatos()
        {
            try
            {
                cargando = true;
                StateHasChanged();
                
                // Cargar carreras
                carreras = (await CarreraServicios.ListarCarreras()).ToList();
                
                // Cargar docentes
                await CargarDocentes();
            }
            catch (Exception ex)
            {
                await JSRuntime.InvokeVoidAsync("console.error", "Error al cargar datos:", ex.Message);
            }
            finally
            {
                cargando = false;
                StateHasChanged();
            }
        }

        private async Task CargarDocentes()
        {
            if (string.IsNullOrEmpty(criterioBusqueda))
            {
                docentes = await DocenteServicios.ListarDocentes();
            }
            else
            {
                docentes = await DocenteServicios.ListarDocentes(criterioBusqueda);
            }

            // Aplicar filtro de carrera si está seleccionada
            if (carreraSeleccionada > 0)
            {
                docentes = docentes.Where(d => d.IdCarrera == carreraSeleccionada);
            }

            ActualizarPaginacion();
        }

        private void ActualizarPaginacion()
        {
            docentesPaginados = docentes
                .Skip((paginaActual - 1) * registrosPorPagina)
                .Take(registrosPorPagina);
        }

        private async Task BuscarDocentes()
        {
            paginaActual = 1; // Resetear a la primera página al buscar
            await CargarDocentes();
        }

        private async Task FiltrarDocentesAsync()
        {
            paginaActual = 1; // Resetear a la primera página al filtrar
            await CargarDocentes();
            StateHasChanged();
        }

        private void CambiarPagina(int numeroPagina)
        {
            if (numeroPagina >= 1 && numeroPagina <= totalPaginas)
            {
                paginaActual = numeroPagina;
                ActualizarPaginacion();
            }
        }

        private void AgregarDocente()
        {
            Navigation.NavigateTo("/CV/Docentes/Agregar");
        }

        private void VerDocente(int idDocente)
        {
            Navigation.NavigateTo($"/CV/Docentes/Ver/{idDocente}");
        }

        private void EditarDocente(int idDocente)
        {
            Navigation.NavigateTo($"/CV/Docentes/Editar/{idDocente}");
        }

        private async Task EliminarDocente(int idDocente)
        {
            var docente = docentes.FirstOrDefault(d => d.IdDocente == idDocente);
            if (docente == null) return;

            var confirmado = await JSRuntime.InvokeAsync<bool>("confirm", 
                $". ¿Estás seguro de que deseas eliminar al docente {docente.NombreDocente} {docente.PaternoDocente} {docente.MaternoDocente}?");

            if (confirmado)
            {
                try
                {
                    var resultado = await DocenteServicios.BorrarDocente(idDocente);
                    
                    if (resultado.Resultado)
                    {
                        await JSRuntime.InvokeVoidAsync("alert", ". Docente eliminado exitosamente");
                        await CargarDocentes();
                    }
                    else
                    {
                        var mensajes = string.Join(", ", resultado.Mensajes);
                        await JSRuntime.InvokeVoidAsync("alert", $". Error al eliminar: {mensajes}");
                    }
                }
                catch (Exception ex)
                {
                    await JSRuntime.InvokeVoidAsync("alert", $". Error inesperado: {ex.Message}");
                }
            }
        }

        private string GetIniciales(string nombre, string? apellido)
        {
            var iniciales = "";
            if (!string.IsNullOrEmpty(nombre))
                iniciales += nombre[0];
            if (!string.IsNullOrEmpty(apellido))
                iniciales += apellido[0];
            return iniciales.ToUpper();
        }

        private string GetGoogleDrivePreviewUrl(string url)
        {
            try
            {
                var fileId = url.Split("/d/")[1].Split("/")[0];
                return $"https://drive.google.com/file/d/{fileId}/preview";
            }
            catch
            {
                return url;
            }
        }
    }
} 