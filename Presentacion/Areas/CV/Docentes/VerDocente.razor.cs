using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Entidades.DTO.CurriculumVite;
using Servicios.IRepositorios.CurriculumVite;
using AutoMapper;

namespace Presentacion.Areas.CV.Docentes
{
    public partial class VerDocente : ComponentBase
    {
        [Parameter] public int IdDocente { get; set; }

        [Inject] private ISDocenteRepositorio DocenteServicios { get; set; } = default!;
        [Inject] private ISRepositorioContactoDocente ContactoServicios { get; set; } = default!;
        [Inject] private ISRepositorioTipoContacto TipoContactoServicios { get; set; } = default!;
        [Inject] private ISRepositorioEducacion EducacionServicios { get; set; } = default!;
        [Inject] private ISRepositorioExperiencia ExperienciaServicios { get; set; } = default!;
        [Inject] private ISRepositorioPublicacion PublicacionServicios { get; set; } = default!;
        [Inject] private ISRepositorioProyecto ProyectoServicios { get; set; } = default!;
        [Inject] private ISRepositorioPDFGenerator PdfService { get; set; } = default!;
        [Inject] private IMapper Mapper { get; set; } = default!;
        [Inject] private NavigationManager Navigation { get; set; } = default!;
        [Inject] private IJSRuntime JSRuntime { get; set; } = default!;

        private DocenteDTO? docente;
        private List<ContactoDocenteDTO> contactosDocente = new();
        private List<EducacionDTO> educaciones = new();
        private List<ExperienciaDTO> experiencias = new();
        private List<PublicacionDTO> publicaciones = new();
        private List<ProyectoDTO> proyectos = new();

        private bool cargando = true;
        private bool mostrarModalContactos = false;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                await CargarDatosDocente();
            }
            catch (Exception ex)
            {
                await JSRuntime.InvokeVoidAsync("alert", $". Error al cargar datos: {ex.Message}");
            }
            finally
            {
                cargando = false;
                StateHasChanged();
            }
        }

        private async Task CargarDatosDocente()
        {
            var resultado = await DocenteServicios.ObtenerDocente<DocenteDTO>(IdDocente);
            if (resultado.Resultado && resultado.Entidad != null)
            {
                docente = resultado.Entidad;
                
                await CargarContactos();
                await CargarEducaciones();
                await CargarExperiencias();
                await CargarPublicaciones();
                await CargarProyectos();
                
                if (docente != null)
                {
                    docente.CantidadEducaciones = educaciones.Count;
                    docente.CantidadExperiencias = experiencias.Count;
                    docente.CantidadPublicaciones = publicaciones.Count;
                    docente.CantidadProyectos = proyectos.Count;
                }
            }
        }

        private async Task CargarContactos()
        {
            var contactosEntidades = await ContactoServicios.GetContactosByDocenteIdAsync(IdDocente);
            var tiposContacto = await TipoContactoServicios.GetAllAsync();
            var tiposContactoDto = Mapper.Map<List<TipoContactoDTO>>(tiposContacto);
            
            contactosDocente = new List<ContactoDocenteDTO>();
            
            foreach (var contactoEntidad in contactosEntidades)
            {
                var contactoDto = Mapper.Map<ContactoDocenteDTO>(contactoEntidad);
                contactoDto.TipoContacto = tiposContactoDto.FirstOrDefault(t => t.TipoContactoId == contactoDto.IdTipoContacto);
                contactoDto.NombreTipoContacto = contactoDto.TipoContacto?.Nombre;
                contactosDocente.Add(contactoDto);
            }
        }

        private async Task CargarEducaciones()
        {
            try
            {
                var todasEducaciones = await EducacionServicios.GetAllAsync();
                var educacionesEntidades = todasEducaciones.Where(e => ((dynamic)e).IdDocente == IdDocente);
                educaciones = Mapper.Map<List<EducacionDTO>>(educacionesEntidades);
            }
            catch (Exception)
            {
                educaciones = new List<EducacionDTO>();
            }
        }

        private async Task CargarExperiencias()
        {
            try
            {
                var todasExperiencias = await ExperienciaServicios.GetAllAsync();
                var experienciasEntidades = todasExperiencias.Where(e => ((dynamic)e).IdDocente == IdDocente);
                experiencias = Mapper.Map<List<ExperienciaDTO>>(experienciasEntidades);
            }
            catch (Exception)
            {
                experiencias = new List<ExperienciaDTO>();
            }
        }

        private async Task CargarPublicaciones()
        {
            try
            {
                var todasPublicaciones = await PublicacionServicios.GetAllAsync();
                var publicacionesEntidades = todasPublicaciones.Where(p => ((dynamic)p).IdDocente == IdDocente);
                publicaciones = Mapper.Map<List<PublicacionDTO>>(publicacionesEntidades);
            }
            catch (Exception)
            {
                publicaciones = new List<PublicacionDTO>();
            }
        }

        private async Task CargarProyectos()
        {
            try
            {
                var todosProyectos = await ProyectoServicios.GetAllAsync();
                var proyectosEntidades = todosProyectos.Where(p => ((dynamic)p).IdDocente == IdDocente);
                proyectos = Mapper.Map<List<ProyectoDTO>>(proyectosEntidades);
            }
            catch (Exception)
            {
                proyectos = new List<ProyectoDTO>();
            }
        }

        private void MostrarContactos()
        {
            mostrarModalContactos = true;
            StateHasChanged();
        }

        private void CerrarModalContactos()
        {
            mostrarModalContactos = false;
            StateHasChanged();
        }

        private async Task CopiarTexto(string texto)
        {
            await JSRuntime.InvokeVoidAsync("navigator.clipboard.writeText", texto);
            await JSRuntime.InvokeVoidAsync("alert", ". Texto copiado al portapapeles");
        }

        private bool EsUrlValida(string url)
        {
            return !string.IsNullOrEmpty(url) && 
                   (url.StartsWith("http://") || url.StartsWith("https://") || 
                    url.Contains("."));
        }

        private bool EsEmail(string texto)
        {
            return !string.IsNullOrEmpty(texto) && texto.Contains("@");
        }

        private bool EsTelefono(string texto)
        {
            return !string.IsNullOrEmpty(texto) && 
                   (texto.StartsWith("+") || texto.All(c => char.IsDigit(c) || c == ' ' || c == '-' || c == '(' || c == ')'));
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

        private void VolverALista()
        {
            Navigation.NavigateTo("/CV/Docentes");
        }

        private void EditarDocente()
        {
            Navigation.NavigateTo($"/CV/Docentes/Editar/{IdDocente}");
        }

        private void EditarContactos()
        {
            Navigation.NavigateTo($"/CV/Docentes/{IdDocente}/Contactos");
            CerrarModalContactos();
        }

        private void IrASeccion(string seccion)
        {
            var ruta = seccion switch
            {
                "publicaciones" => $"/CV/Docentes/{IdDocente}/Publicaciones?fromProfile=true",
                "educacion" => $"/CV/Docentes/{IdDocente}/Educacion?fromProfile=true", 
                "experiencia" => $"/CV/Docentes/{IdDocente}/Experiencia?fromProfile=true",
                "proyectos" => $"/CV/Docentes/{IdDocente}/Proyectos?fromProfile=true",
                _ => "/CV/Docentes"
            };
            Navigation.NavigateTo(ruta);
        }

        private async Task ImprimirCV()
        {
            try
            {
                if (docente == null)
                {
                    await JSRuntime.InvokeVoidAsync("alert", ". No se puede generar el CV sin datos del docente");
                    return;
                }

                var logos = await CargarLogos();
                var pdfBytes = PdfService.GenerarCVConContactos(docente, educaciones, experiencias, publicaciones, proyectos, new List<TesisDirigidaDTO>(), new List<DistincionDTO>(), contactosDocente, logos);
                var fileName = $"CV_{docente.NombreCompleto?.Replace(" ", "_") ?? "Docente"}_{DateTime.Now:yyyyMMdd}.pdf";
                
                await DescargarArchivo(pdfBytes, fileName, "application/pdf");
            }
            catch (Exception ex)
            {
                await JSRuntime.InvokeVoidAsync("alert", $". Error al generar PDF: {ex.Message}");
            }
        }

        private async Task DescargarCV()
        {
            try
            {
                if (docente == null)
                {
                    await JSRuntime.InvokeVoidAsync("alert", ". No se puede descargar el CV sin datos del docente");
                    return;
                }

                var logos = await CargarLogos();
                var pdfBytes = PdfService.GenerarCVConContactos(docente, educaciones, experiencias, publicaciones, proyectos, new List<TesisDirigidaDTO>(), new List<DistincionDTO>(), contactosDocente, logos);
                var fileName = $"CV_{docente.NombreCompleto?.Replace(" ", "_") ?? "Docente"}_{DateTime.Now:yyyyMMdd}.pdf";
                
                await DescargarArchivo(pdfBytes, fileName, "application/pdf");
            }
            catch (Exception ex)
            {
                await JSRuntime.InvokeVoidAsync("alert", $". Error al descargar CV: {ex.Message}");
            }
        }

        private async Task VerCVDemo()
        {
            try
            {
                var pdfBytes = PdfService.GenerarCVDemo();
                var fileName = $"CV_Demo_{DateTime.Now:yyyyMMdd}.pdf";
                
                await DescargarArchivo(pdfBytes, fileName, "application/pdf");
            }
            catch (Exception ex)
            {
                await JSRuntime.InvokeVoidAsync("alert", $". Error al generar CV demo: {ex.Message}");
            }
        }

        private async Task DescargarArchivo(byte[] contenido, string nombreArchivo, string tipoMime)
        {
            var contenidoBase64 = Convert.ToBase64String(contenido);
            await JSRuntime.InvokeVoidAsync("descargarArchivo", nombreArchivo, tipoMime, contenidoBase64);
        }

        private async Task<Dictionary<string, byte[]>> CargarLogos()
        {
            var logos = new Dictionary<string, byte[]>();
            
            try
            {
                var basePath = Path.Combine(Directory.GetCurrentDirectory(), "Servicios", "Repositorios", "CurriculumVite", "Imagenes");
                
                var uabcPath = Path.Combine(basePath, "logo_uabc.gif");
                if (File.Exists(uabcPath))
                {
                    logos["uabc"] = await File.ReadAllBytesAsync(uabcPath);
                    await JSRuntime.InvokeVoidAsync("console.log", $"Logo UABC cargado: {logos["uabc"].Length} bytes");
                }
                else
                {
                    await JSRuntime.InvokeVoidAsync("console.log", $"Logo UABC no encontrado en: {uabcPath}");
                }
                
                var fiadPath = Path.Combine(basePath, "logo_fiad.png");
                if (File.Exists(fiadPath))
                {
                    logos["fiad"] = await File.ReadAllBytesAsync(fiadPath);
                    await JSRuntime.InvokeVoidAsync("console.log", $"Logo FIAD cargado: {logos["fiad"].Length} bytes");
                }
                else
                {
                    await JSRuntime.InvokeVoidAsync("console.log", $"Logo FIAD no encontrado en: {fiadPath}");
                }
            }
            catch (Exception ex)
            {
                await JSRuntime.InvokeVoidAsync("console.log", $"Error cargando logos: {ex.Message}");
            }
            
            return logos;
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