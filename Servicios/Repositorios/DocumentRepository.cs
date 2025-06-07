using System.Text;
using System.Text.Json;
using Entidades.Utilidades;
using Servicios.IRepositorios;

namespace Servicios.Repositorios
{
    public class DocumentRepository : IDocumentRepository
    {
        private readonly HttpClient _httpClient;
        private const string GOOGLE_SCRIPT_URL = "https://script.google.com/macros/s/AKfycbwC5CLE_OquYzR1AaObsj_8FHjTG4d-f9daRUZlf_LWcTuYfrjSbPSMloA85i-SExVW/exec";

        public DocumentRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<GoogleDriveUploadResponse> SubirDocumentoAsync(DocumentUploadRequest request)
        {
            return await SubirDocumentoAsync(
                request.FileData,
                request.FileName,
                request.FileType,
                request.EmpleadoNombre,
                request.NumeroEmpleado,
                request.TipoDocumento);
        }

        public async Task<GoogleDriveUploadResponse> SubirDocumentoAsync(byte[] fileData, string fileName, string fileType, string empleadoNombre, string numeroEmpleado, DocumentType tipoDocumento)
        {
            try
            {
                var base64Data = Convert.ToBase64String(fileData);
                var subFolder = GetSubFolderName(tipoDocumento);

                var requestData = new
                {
                    fname = "uploadFilesToGoogleDrive",
                    dataReq = new
                    {
                        data = base64Data,
                        name = fileName,
                        type = fileType,
                        employeeName = empleadoNombre,
                        employeeNumber = numeroEmpleado,
                        subFolder = subFolder
                    }
                };

                Console.WriteLine(JsonSerializer.Serialize(requestData));

                var json = JsonSerializer.Serialize(requestData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(GOOGLE_SCRIPT_URL, content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    
                    // Deserializar la respuesta JSON
                    var uploadResponse = JsonSerializer.Deserialize<GoogleDriveUploadResponse>(responseContent, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    if (uploadResponse != null && uploadResponse.Status == "success")
                    {
                        return uploadResponse;
                    }
                    else
                    {
                        throw new Exception($"Error en la respuesta del servidor: {responseContent}");
                    }
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Error HTTP {response.StatusCode}: {errorContent}");
                }
            }
            catch (JsonException ex)
            {
                throw new Exception($"Error al procesar la respuesta JSON: {ex.Message}", ex);
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Error de conexión: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al subir archivo: {ex.Message}", ex);
            }
        }

        private static string GetSubFolderName(DocumentType tipoDocumento)
        {
            return tipoDocumento switch
            {
                DocumentType.Publicacion => "Publicacion",
                DocumentType.Distincion => "Distincion",
                DocumentType.Proyecto => "Proyecto",
                DocumentType.TesisDirigida => "TesisDirigida",
                DocumentType.Educacion => "Educacion",
                DocumentType.Experiencia => "Experiencia",
                DocumentType.General => "General",
                DocumentType.FotoPerfil => "FotoPerfil",
                _ => "General"
            };
        }

        public async Task<bool> EliminarDocumentoAsync(string fileId)
        {
            try
            {
                Console.WriteLine($"Intentando eliminar archivo con ID: {fileId}");
                
                var requestData = new
                {
                    fname = "deleteFileFromGoogleDrive",
                    dataReq = new
                    {
                        fileId = fileId
                    }
                };

                var json = JsonSerializer.Serialize(requestData);
                Console.WriteLine($"Request JSON: {json}");
                
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(GOOGLE_SCRIPT_URL, content);
                var responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Response status code: {response.StatusCode}");
                Console.WriteLine($"Response content: {responseContent}");

                if (response.IsSuccessStatusCode)
                {
                    var result = JsonSerializer.Deserialize<GoogleDriveDeleteResponse>(responseContent, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    if (result?.Status == "success")
                    {
                        Console.WriteLine("Archivo eliminado exitosamente");
                        return true;
                    }
                    else
                    {
                        Console.WriteLine($"Error al eliminar archivo: {result?.Message ?? "No message"}");
                        return false;
                    }
                }

                Console.WriteLine($"Error HTTP: {response.StatusCode}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar archivo: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                return false;
            }
        }
    }
} 