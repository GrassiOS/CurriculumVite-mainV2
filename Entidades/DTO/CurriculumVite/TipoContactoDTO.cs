namespace Entidades.DTO.CurriculumVite
{
    public class TipoContactoDTO
    {
        public int TipoContactoId { get; set; }
        public string Nombre { get; set; } = null!;
        
        // Propiedades calculadas para iconos y estilos
        public string Icono => ObtenerIcono();
        public string ColorFondo => ObtenerColorFondo();
        public string PlaceholderTexto => ObtenerPlaceholderTexto();
        
        private string ObtenerIcono()
        {
            return Nombre.ToLower() switch
            {
                var n when n.Contains("email") || n.Contains("correo") => "fas fa-envelope",
                var n when n.Contains("teléfono") || n.Contains("telefono") || n.Contains("cel") => "fas fa-phone",
                var n when n.Contains("linkedin") => "fab fa-linkedin",
                var n when n.Contains("twitter") || n.Contains("x") => "fab fa-twitter",
                var n when n.Contains("facebook") => "fab fa-facebook",
                var n when n.Contains("instagram") => "fab fa-instagram",
                var n when n.Contains("github") => "fab fa-github",
                var n when n.Contains("youtube") => "fab fa-youtube",
                var n when n.Contains("whatsapp") => "fab fa-whatsapp",
                var n when n.Contains("telegram") => "fab fa-telegram",
                var n when n.Contains("skype") => "fab fa-skype",
                var n when n.Contains("web") || n.Contains("página") || n.Contains("sitio") => "fas fa-globe",
                var n when n.Contains("blog") => "fas fa-blog",
                var n when n.Contains("scholar") => "fas fa-graduation-cap",
                var n when n.Contains("researchgate") || n.Contains("research") => "fas fa-microscope",
                var n when n.Contains("orcid") => "fab fa-orcid",
                var n when n.Contains("academia") => "fas fa-university",
                _ => "fas fa-link"
            };
        }
        
        private string ObtenerColorFondo()
        {
            return Nombre.ToLower() switch
            {
                var n when n.Contains("linkedin") => "#0077B5",
                var n when n.Contains("twitter") || n.Contains("x") => "#1DA1F2",
                var n when n.Contains("facebook") => "#1877F2",
                var n when n.Contains("instagram") => "#E4405F",
                var n when n.Contains("github") => "#181717",
                var n when n.Contains("youtube") => "#FF0000",
                var n when n.Contains("whatsapp") => "#25D366",
                var n when n.Contains("telegram") => "#0088CC",
                var n when n.Contains("skype") => "#00AFF0",
                var n when n.Contains("email") || n.Contains("correo") => "#EA4335",
                var n when n.Contains("teléfono") || n.Contains("telefono") => "#34A853",
                var n when n.Contains("orcid") => "#A6CE39",
                _ => "#2D6B3C" // Color UABC por defecto
            };
        }
        
        private string ObtenerPlaceholderTexto()
        {
            return Nombre.ToLower() switch
            {
                var n when n.Contains("email") || n.Contains("correo") => "ejemplo@correo.com",
                var n when n.Contains("teléfono") || n.Contains("telefono") => "+52 664 123 4567",
                var n when n.Contains("linkedin") => "https://linkedin.com/in/usuario",
                var n when n.Contains("twitter") || n.Contains("x") => "https://twitter.com/usuario",
                var n when n.Contains("facebook") => "https://facebook.com/usuario",
                var n when n.Contains("instagram") => "https://instagram.com/usuario",
                var n when n.Contains("github") => "https://github.com/usuario",
                var n when n.Contains("youtube") => "https://youtube.com/c/canal",
                var n when n.Contains("whatsapp") => "+52 664 123 4567",
                var n when n.Contains("telegram") => "@usuario",
                var n when n.Contains("skype") => "usuario.skype",
                var n when n.Contains("web") || n.Contains("página") => "https://mipagina.com",
                var n when n.Contains("blog") => "https://miblog.com",
                var n when n.Contains("scholar") => "https://scholar.google.com/citations?user=ID",
                var n when n.Contains("orcid") => "https://orcid.org/0000-0000-0000-0000",
                _ => "https://enlace.com"
            };
        }
    }
}
