using Entidades.Utilidades;

namespace Servicios.IRepositorios
{
    public interface IDocumentRepository
    {
        Task<GoogleDriveUploadResponse> SubirDocumentoAsync(DocumentUploadRequest request);
        Task<GoogleDriveUploadResponse> SubirDocumentoAsync(byte[] fileData, string fileName, string fileType, string empleadoNombre, string numeroEmpleado, DocumentType tipoDocumento);
        Task<bool> EliminarDocumentoAsync(string fileId);
    }
} 