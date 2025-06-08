using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using Entidades.Modelos.CurriculumVite;
using Entidades.DTO.CurriculumVite;
using Entidades.Modelos.PlanesDeEstudio.Carreras;
using Servicios.IRepositorios.CurriculumVite;
using Servicios.Repositorios.PlanesDeEstudio;
using Entidades.Utilidades;
using Servicios.IRepositorios;
using Entidades.Generales;

namespace Presentacion.Areas.CV.Docentes
{
    public partial class GestionDocente : ComponentBase
    {
        [Parameter] public int? IdDocente { get; set; }

        [Inject] private ISDocenteRepositorio DocenteServicios { get; set; } = default!;
        [Inject] private IDocumentRepository DocumentRepository { get; set; } = default!;
        [Inject] private CarreraServicios CarreraServicios { get; set; } = default!;
        [Inject] private NavigationManager Navigation { get; set; } = default!;
        [Inject] private IJSRuntime JSRuntime { get; set; } = default!;

        private DocenteDTO docente = new();
        private bool cargando = true;
        private bool guardando = false;
        private bool estaActivo = true;
        private bool tieneProdep = false;
        private List<E_Carrera> carreras = new();
        private string? mensajeError;
        private IBrowserFile? fotoSeleccionada;
        private bool mostrarMensaje;
        private string tipoMensaje = "info";
        private string mensaje = "";

        private bool EsEdicion => IdDocente.HasValue && IdDocente.Value > 0;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                cargando = true;
                carreras = (await CarreraServicios.ListarCarreras()).ToList();

                if (EsEdicion && IdDocente.HasValue)
                {
                    var resultado = await DocenteServicios.ObtenerDocente<DocenteDTO>(IdDocente.Value);
                    if (resultado.Resultado && resultado.Entidad != null)
                    {
                        docente = resultado.Entidad;
                        estaActivo = docente.EstadoDocente == 1;
                        tieneProdep = docente.IdPRODEP == 1;
                    }
                    else
                    {
                        mostrarMensaje = true;
                        tipoMensaje = "danger";
                        mensaje = "No se encontró el docente solicitado";
                        Navigation.NavigateTo("/CV/Docentes");
                    }
                }
            }
            catch (Exception ex)
            {
                mostrarMensaje = true;
                tipoMensaje = "danger";
                mensaje = $"Error al cargar la información: {ex.Message}";
            }
            finally
            {
                cargando = false;
            }
        }

        private async Task GuardarDocente()
        {
            try
            {
                guardando = true;
                StateHasChanged();

                // Actualizar los campos de estado
                docente.EstadoDocente = estaActivo ? 1 : 0;
                docente.IdPRODEP = tieneProdep ? 1 : 2;

                if (!EsEdicion)
                {
                    // 1. Crear el docente primero
                    await JSRuntime.InvokeVoidAsync("console.log", "Creando nuevo docente...");
                    var resultadoInsercion = await DocenteServicios.InsertarDocente(docente);
                    
                    if (!resultadoInsercion.Resultado || resultadoInsercion.Entidad == null)
                    {
                        mostrarMensaje = true;
                        tipoMensaje = "danger";
                        mensaje = string.Join("\n", resultadoInsercion.Mensajes);
                        return;
                    }

                    var nuevoDocente = resultadoInsercion.Entidad;

                    // 2. Si hay foto seleccionada, subirla y actualizar el docente
                    if (fotoSeleccionada != null)
                    {
                        await JSRuntime.InvokeVoidAsync("console.log", "Subiendo foto...");
                        
                        using var stream = fotoSeleccionada.OpenReadStream(maxAllowedSize: 50 * 1024 * 1024);
                        using var ms = new MemoryStream();
                        await stream.CopyToAsync(ms);
                        var fileData = ms.ToArray();

                        var uploadRequest = new DocumentUploadRequest
                        {
                            FileData = fileData,
                            FileName = fotoSeleccionada.Name,
                            FileType = fotoSeleccionada.ContentType,
                            EmpleadoNombre = $"{docente.NombreDocente} {docente.PaternoDocente}".Trim(),
                            NumeroEmpleado = docente.NumeroEmpleado ?? "Sin Número",
                            TipoDocumento = DocumentType.General
                        };

                        var response = await DocumentRepository.SubirDocumentoAsync(uploadRequest);
                        
                        if (!string.IsNullOrEmpty(response.Url))
                        {
                            await JSRuntime.InvokeVoidAsync("console.log", "Foto subida. URL recibida: " + response.Url);
                            
                            // Actualizar el docente con la URL de la foto
                            nuevoDocente.URLFoto = response.Url;
                            
                            var resultadoUpdate = await DocenteServicios.ModificarDocente(nuevoDocente);
                            if (!resultadoUpdate.Resultado)
                            {
                                await JSRuntime.InvokeVoidAsync("console.log", "Error al guardar URL de foto: " + string.Join(", ", resultadoUpdate.Mensajes));
                                mostrarMensaje = true;
                                tipoMensaje = "warning";
                                mensaje = "Docente creado pero hubo un error al guardar la URL de la foto";
                                return;
                            }
                            
                            await JSRuntime.InvokeVoidAsync("console.log", "URL de foto guardada exitosamente en la base de datos");
                        }
                        else
                        {
                            await JSRuntime.InvokeVoidAsync("console.log", "Error: No se recibió URL de la foto");
                            mostrarMensaje = true;
                            tipoMensaje = "warning";
                            mensaje = "Docente creado pero no se pudo subir la foto";
                            return;
                        }
                    }

                    await JSRuntime.InvokeVoidAsync("alert", "Docente creado exitosamente");
                    Navigation.NavigateTo($"/CV/Docentes/Ver/{nuevoDocente.IdDocente}");
                }
                else
                {
                    // Primero subir la foto si hay una seleccionada
                    if (fotoSeleccionada != null)
                    {
                        await SubirFotoSeleccionada(docente.IdDocente);
                    }
                    else
                    {
                        // Si no hay foto nueva, actualizar el docente directamente
                        var resultado = await DocenteServicios.ModificarDocente(docente);
                        if (resultado.Resultado)
                        {
                            await JSRuntime.InvokeVoidAsync("alert", "Docente actualizado exitosamente");
                            Navigation.NavigateTo($"/CV/Docentes/Ver/{docente.IdDocente}");
                        }
                        else
                        {
                            mostrarMensaje = true;
                            tipoMensaje = "danger";
                            mensaje = string.Join("\n", resultado.Mensajes);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                mostrarMensaje = true;
                tipoMensaje = "danger";
                mensaje = $"Error al guardar: {ex.Message}";
                await JSRuntime.InvokeVoidAsync("console.log", "Error general: " + ex.Message);
            }
            finally
            {
                guardando = false;
                StateHasChanged();
            }
        }

        private async Task SubirFotoSeleccionada(int idDocente)
        {
            try
            {
                if (fotoSeleccionada == null) return;

                await JSRuntime.InvokeVoidAsync("console.log", "Subiendo foto en modo edición...");

                // 1. Si hay una foto existente, obtener su ID y eliminarla
                if (!string.IsNullOrEmpty(docente.URLFoto))
                {
                    var fileId = ExtractFileIdFromUrl(docente.URLFoto);
                    if (!string.IsNullOrEmpty(fileId))
                    {
                        await JSRuntime.InvokeVoidAsync("console.log", $"Eliminando foto anterior con ID: {fileId}");
                        var eliminado = await DocumentRepository.EliminarDocumentoAsync(fileId);
                        if (!eliminado)
                        {
                            await JSRuntime.InvokeVoidAsync("console.log", "Advertencia: No se pudo eliminar la foto anterior. ID: " + fileId);
                        }
                    }
                    else
                    {
                        await JSRuntime.InvokeVoidAsync("console.log", "No se pudo extraer el ID del archivo de la URL: " + docente.URLFoto);
                    }
                }

                // 2. Subir la nueva foto
                using var stream = fotoSeleccionada.OpenReadStream(maxAllowedSize: 50 * 1024 * 1024);
                using var ms = new MemoryStream();
                await stream.CopyToAsync(ms);
                var fileData = ms.ToArray();

                var uploadRequest = new DocumentUploadRequest
                {
                    FileData = fileData,
                    FileName = fotoSeleccionada.Name,
                    FileType = fotoSeleccionada.ContentType,
                    EmpleadoNombre = $"{docente.NombreDocente} {docente.PaternoDocente}".Trim(),
                    NumeroEmpleado = docente.NumeroEmpleado ?? "Sin Número",
                    TipoDocumento = DocumentType.General
                };

                var response = await DocumentRepository.SubirDocumentoAsync(uploadRequest);
                if (!string.IsNullOrEmpty(response.Url))
                {
                    await JSRuntime.InvokeVoidAsync("console.log", "Foto subida en edición. URL recibida: " + response.Url);
                    
                    // 3. Actualizar el docente con la URL de la foto
                    docente.URLFoto = response.Url;
                    var resultadoUpdate = await DocenteServicios.ModificarDocente(docente);
                    
                    if (!resultadoUpdate.Resultado)
                    {
                        await JSRuntime.InvokeVoidAsync("console.log", "Error al guardar URL de foto en edición: " + string.Join(", ", resultadoUpdate.Mensajes));
                        mostrarMensaje = true;
                        tipoMensaje = "warning";
                        mensaje = "Error al guardar la URL de la foto";
                    }
                    else
                    {
                        await JSRuntime.InvokeVoidAsync("console.log", "URL de foto guardada exitosamente en la base de datos (edición)");
                        await JSRuntime.InvokeVoidAsync("alert", "Docente actualizado exitosamente");
                        Navigation.NavigateTo($"/CV/Docentes/Ver/{idDocente}");
                    }
                }
                else
                {
                    mostrarMensaje = true;
                    tipoMensaje = "warning";
                    mensaje = "No se pudo subir la foto del docente";
                    await JSRuntime.InvokeVoidAsync("console.log", "Error: No se obtuvo URL de la foto en edición");
                }
            }
            catch (Exception ex)
            {
                mostrarMensaje = true;
                tipoMensaje = "danger";
                mensaje = $"Error al subir la foto: {ex.Message}";
                await JSRuntime.InvokeVoidAsync("console.log", "Error al subir foto en edición: " + ex.Message);
            }
        }

        private string ExtractFileIdFromUrl(string url)
        {
            try
            {
                // La URL tiene el formato: https://drive.google.com/uc?id=FILE_ID
                if (url.Contains("id="))
                {
                    return url.Split("id=")[1].Split('&')[0];
                }
                // O el formato: https://drive.google.com/file/d/FILE_ID/view
                else if (url.Contains("/file/d/"))
                {
                    return url.Split("/file/d/")[1].Split('/')[0];
                }
            }
            catch (Exception ex)
            {
                JSRuntime.InvokeVoidAsync("console.log", $"Error al extraer ID del archivo: {ex.Message}");
            }
            return string.Empty;
        }

        private async Task EliminarDocente()
        {
            if (!EsEdicion) return;

            var confirmado = await JSRuntime.InvokeAsync<bool>("confirm",
                $". ¿Estás seguro de que deseas eliminar al docente {docente.NombreDocente} {docente.PaternoDocente}?");

            if (confirmado)
            {
                try
                {
                    var resultado = await DocenteServicios.BorrarDocente(docente.IdDocente);
                    if (resultado.Resultado)
                    {
                        await JSRuntime.InvokeVoidAsync("alert", ". Docente eliminado exitosamente");
                        Navigation.NavigateTo("/CV/Docentes");
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

        private void Cancelar()
        {
            Navigation.NavigateTo("/CV/Docentes");
        }

        private void AutoCompletarFormulario()
        {
            var random = new Random();

            // Arrays de nombres mexicanos típicos
            var nombres = new[] {
                "Jorge", "Daniel", "Gabriel", "William", "Declan", "Aaron",
                "David", "Alejandro", "Ricardo", "Thomas", "Bukayo", "Kai"
            };

            var apellidosPaternos = new[] {
                "Raya", "Santiago", "Nolan", "Harper", "Lopez", "Bradford",
                "Chen", "Magalhaes", "Saliba", "Rice"
            };

            var apellidosMaternos = new[] {
                "Saka", "Ferez", "Gerardo", "Nazario", "Romero", "Navarro",
                "Fuentes", "Castillo", "Ortega", "Silva"
            };

            var nombre = nombres[random.Next(nombres.Length)];
            var paterno = apellidosPaternos[random.Next(apellidosPaternos.Length)];
            var materno = apellidosMaternos[random.Next(apellidosMaternos.Length)];

            // Generar número de empleado único
            var numeroEmpleado = $"EMP{random.Next(1000, 9999)}";

            // Generar RFC (formato básico)
            var iniciales = $"{paterno.Substring(0, 2)}{materno.Substring(0, 1)}{nombre.Substring(0, 1)}".ToUpper();
            var fecha = "800101"; // Fecha ejemplo
            var homoclave = $"{(char)('A' + random.Next(26))}{(char)('A' + random.Next(26))}{random.Next(10)}";
            var rfc = $"{iniciales}{fecha}{homoclave}";

            // Generar CURP (formato básico)
            var sexoLetra = random.Next(2) == 0 ? "H" : "M";
            var estado = "BC"; // Baja California
            var consonantes = "BCDFGHJKLMNPQRSTVWXYZ";
            var curp = $"{iniciales}{fecha}{sexoLetra}{estado}{consonantes[random.Next(consonantes.Length)]}{consonantes[random.Next(consonantes.Length)]}{consonantes[random.Next(consonantes.Length)]}{random.Next(10)}{random.Next(10)}";

            // Generar email institucional
            var emailNombre = nombre.Split(' ')[0].ToLower();
            var emailApellido = paterno.ToLower();
            var email = $"{emailNombre}.{emailApellido}@uabc.edu.mx";

            var semblanzas = new[]
            {
                $"El Dr. {nombre} {paterno} es un destacado académico e investigador con amplia experiencia en el campo de la ingeniería en software y tecnologías emergentes. Su compromiso con la excelencia académica y la innovación tecnológica lo posicionan como un referente en su área de especialización.",
                $"La Dra. {nombre} {paterno} cuenta con una sólida trayectoria en investigación y docencia, especializándose en desarrollo de software, inteligencia artificial y sistemas distribuidos. Su enfoque multidisciplinario ha contribuido significativamente al avance del conocimiento en su campo.",
                $"El Mtro. {nombre} {paterno} es un profesional experimentado con una pasión por la enseñanza y la investigación aplicada. Su experiencia en la industria tecnológica y su dedicación a la formación de nuevas generaciones de ingenieros lo convierten en un valioso miembro del cuerpo académico.",
                $"La Mtra. {nombre} {paterno} se distingue por su compromiso con la innovación educativa y su capacidad para integrar las últimas tendencias tecnológicas en el proceso de enseñanza-aprendizaje. Su experiencia en proyectos de desarrollo de software la posiciona como una líder en su área."
            };

            docente = new DocenteDTO
            {
                // Datos Personales
                NombreDocente = nombre,
                PaternoDocente = paterno,
                MaternoDocente = materno,
                FechaNacimiento = DateTime.Now.AddYears(-random.Next(25, 65)), // Entre 25 y 65 años
                IdSexo = random.Next(1, 3), // 1=Masculino, 2=Femenino (VALORES REALES)
                RFC = rfc,
                CURP = curp,
                TelefonoCasa = $"664{random.Next(1000000, 9999999)}",
                TelefonoCelular = $"664{random.Next(1000000, 9999999)}",
                Direccion = $"Calle {random.Next(1, 50)} #{random.Next(100, 9999)}, Colonia {(random.Next(2) == 0 ? "Centro" : "Universidad")}, Tijuana, BC",
                IdEstadoCivil = random.Next(1, 5), // 1=Soltero, 2=Casado, 3=Divorciado, 4=Viudo (VALORES REALES)
                SembalzaDocente = semblanzas[random.Next(semblanzas.Length)],

                // Datos Institucionales
                NumeroEmpleado = numeroEmpleado,
                FechaIngreso = DateTime.Now.AddYears(-random.Next(1, 15)), // Entre 1 y 15 años de antigüedad
                EstadoDocente = 1, // Activo por defecto
                IdCategoria = random.Next(1, 4), // 1=Tiempo Completo, 2=Medio Tiempo, 3=Asignatura (VALORES REALES)
                IdNombramiento = random.Next(1, 4), // 1=Base, 2=Interino, 3=Honorarios (VALORES REALES)
                IdCarrera = 1, // Solo existe 1 carrera: "Ingeniería en Software y Tecnologías Emergentes"
                IdEscolaridad = random.Next(1, 4), // 1=Licenciatura, 2=Maestría, 3=Doctorado (VALORES REALES)
                UniversidadLicenciatura = random.Next(3) switch
                {
                    0 => "UABC",
                    1 => "UNAM",
                    _ => "IPN"
                },
                CedulaProfesional = random.Next(1000000, 9999999).ToString(),
                EmailInstitucional = email,
                Extension = random.Next(1000, 9999).ToString(),
                IdNivelSNI = random.Next(1, 5), // 1=Nivel I, 2=Nivel II, 3=Nivel III, 4=No Aplica (VALORES REALES)
                IdPRODEP = random.Next(1, 3) // 1="Sí", 2="No" (VALORES REALES)
            };

            // Actualizar variables de estado del componente
            estaActivo = docente.EstadoDocente == 1;
            tieneProdep = docente.IdPRODEP == 1; // 1 = "Sí", 2 = "No"

            // Forzar actualización de la UI
            StateHasChanged();

            // Mostrar mensaje informativo
            JSRuntime.InvokeVoidAsync("alert", ". Formulario completado automáticamente con datos de ejemplo");
        }

        private void IrAContactos()
        {
            if (EsEdicion && IdDocente.HasValue)
            {
                Navigation.NavigateTo($"/CV/Docentes/{IdDocente.Value}/Contactos");
            }
        }

        private async Task GuardarYContinuar()
        {
            try
            {
                guardando = true;
                StateHasChanged();

                // Actualizar los campos de estado
                docente.EstadoDocente = estaActivo ? 1 : 0;
                docente.IdPRODEP = tieneProdep ? 1 : 2; // 1 = "Sí", 2 = "No"

                // 1. Crear el docente primero
                var resultado = await DocenteServicios.InsertarDocente(docente);
                if (!resultado.Resultado || resultado.Entidad == null)
                {
                    var mensajes = string.Join(", ", resultado.Mensajes);
                    await JSRuntime.InvokeVoidAsync("alert", $". Error al guardar: {mensajes}");
                    return;
                }

                var nuevoDocente = resultado.Entidad;

                // 2. Si hay foto seleccionada, subirla y actualizar el docente
                if (fotoSeleccionada != null)
                {
                    await JSRuntime.InvokeVoidAsync("console.log", "Subiendo foto...");
                    
                    using var stream = fotoSeleccionada.OpenReadStream(maxAllowedSize: 50 * 1024 * 1024);
                    using var ms = new MemoryStream();
                    await stream.CopyToAsync(ms);
                    var fileData = ms.ToArray();

                    var uploadRequest = new DocumentUploadRequest
                    {
                        FileData = fileData,
                        FileName = fotoSeleccionada.Name,
                        FileType = fotoSeleccionada.ContentType,
                        EmpleadoNombre = $"{docente.NombreDocente} {docente.PaternoDocente}".Trim(),
                        NumeroEmpleado = docente.NumeroEmpleado ?? "Sin Número",
                        TipoDocumento = DocumentType.General
                    };

                    var response = await DocumentRepository.SubirDocumentoAsync(uploadRequest);
                    
                    if (!string.IsNullOrEmpty(response.Url))
                    {
                        await JSRuntime.InvokeVoidAsync("console.log", "Foto subida. URL recibida: " + response.Url);
                        
                        // Actualizar el docente con la URL de la foto
                        nuevoDocente.URLFoto = response.Url;
                        
                        var resultadoUpdate = await DocenteServicios.ModificarDocente(nuevoDocente);
                        if (!resultadoUpdate.Resultado)
                        {
                            await JSRuntime.InvokeVoidAsync("console.log", "Error al guardar URL de foto: " + string.Join(", ", resultadoUpdate.Mensajes));
                            await JSRuntime.InvokeVoidAsync("alert", ". Docente creado pero hubo un error al guardar la URL de la foto. Puede editarla más tarde.");
                        }
                        else
                        {
                            await JSRuntime.InvokeVoidAsync("console.log", "URL de foto guardada exitosamente en la base de datos");
                        }
                    }
                    else
                    {
                        await JSRuntime.InvokeVoidAsync("console.log", "Error: No se recibió URL de la foto");
                        await JSRuntime.InvokeVoidAsync("alert", ". Docente creado pero no se pudo subir la foto. Puede intentar subirla más tarde.");
                    }
                }

                await JSRuntime.InvokeVoidAsync("alert", ". Docente guardado exitosamente. Ahora puede agregar contactos.");
                Navigation.NavigateTo($"/CV/Docentes/{nuevoDocente.IdDocente}/Contactos");
            }
            catch (Exception ex)
            {
                await JSRuntime.InvokeVoidAsync("alert", $". Error inesperado: {ex.Message}");
            }
            finally
            {
                guardando = false;
                StateHasChanged();
            }
        }

        private async Task SubirFotoPerfil(InputFileChangeEventArgs e)
        {
            try
            {
                fotoSeleccionada = e.File;
                await JSRuntime.InvokeVoidAsync("console.log", "Foto seleccionada: " + fotoSeleccionada.Name);
            }
            catch (Exception ex)
            {
                mostrarMensaje = true;
                tipoMensaje = "danger";
                mensaje = $"Error al seleccionar la foto: {ex.Message}";
            }
        }
    }
} 