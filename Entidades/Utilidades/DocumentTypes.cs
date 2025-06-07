namespace Entidades.Utilidades
{
    public enum DocumentType
    {
        Publicacion = 1,
        Distincion = 2,
        Proyecto = 3,
        TesisDirigida = 4,
        Educacion = 5,
        Experiencia = 6,
        General = 7,
        FotoPerfil = 8
    }

    public class GoogleDriveUploadResponse
    {
        public string Status { get; set; } = "";
        public string Url { get; set; } = "";
        public string Id { get; set; } = "";
        public string Path { get; set; } = "";
    }

    public class GoogleDriveDeleteResponse
    {
        public string Status { get; set; } = "";
        public string Message { get; set; } = "";
    }

    public class DocumentUploadRequest
    {
        public byte[] FileData { get; set; } = Array.Empty<byte>();
        public string FileName { get; set; } = "";
        public string FileType { get; set; } = "";
        public string EmpleadoNombre { get; set; } = "";
        public string NumeroEmpleado { get; set; } = "";
        public DocumentType TipoDocumento { get; set; }
    }
} 