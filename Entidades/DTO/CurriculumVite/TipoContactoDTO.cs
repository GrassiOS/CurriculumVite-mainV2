namespace Entidades.DTO.CurriculumVite
{
    public class TipoContactoDTO
    {
        public int TipoContactoId { get; set; }
        public string Nombre { get; set; } = null!;
        
        // Propiedades calculadas para iconos y estilos

        // FUTURA IMPLEMENTACION DE ICONOS, COLRES Y PLACEHOLDERS (Ponerlos como variables en la base de datos, para evitar que se tengan que hacer cambios en el codigo )
        public string Icono => ObtenerIcono();
        public string ColorFondo => ObtenerColorFondo();
        public string PlaceholderTexto => ObtenerPlaceholderTexto();
        
        private string ObtenerIcono()
{
    return Nombre.ToLower() switch
    {
        var n when n.Contains("email") || n.Contains("correo") => "fas fa-envelope",
        var n when n.Contains("teléfono") || n.Contains("telefono") || n.Contains("cel") => "fas fa-phone",
        var n when n.Contains("web") || n.Contains("página") || n.Contains("sitio") => "fas fa-globe",
        var n when n.Contains("blog") => "fas fa-blog",

        // Redes sociales y plataformas
        var n when n.Contains("linkedin") => "fab fa-linkedin",
        var n when n.Contains("twitter") || n.Contains("x") => "fab fa-x-twitter",
        var n when n.Contains("facebook") => "fab fa-facebook",
        var n when n.Contains("instagram") => "fab fa-instagram",
        var n when n.Contains("github") => "fab fa-github",
        var n when n.Contains("youtube") => "fab fa-youtube",
        var n when n.Contains("tiktok") => "fab fa-tiktok",
        var n when n.Contains("whatsapp") => "fab fa-whatsapp",
        var n when n.Contains("telegram") => "fab fa-telegram",
        var n when n.Contains("slack") => "fab fa-slack",
        var n when n.Contains("discord") => "fab fa-discord",
        var n when n.Contains("behance") => "fab fa-behance",
        var n when n.Contains("dribbble") => "fab fa-dribbble",
        var n when n.Contains("reddit") => "fab fa-reddit",
        var n when n.Contains("pinterest") => "fab fa-pinterest",
        var n when n.Contains("snapchat") => "fab fa-snapchat",
        var n when n.Contains("threads") => "fab fa-threads",
        var n when n.Contains("mastodon") => "fab fa-mastodon",
        var n when n.Contains("vimeo") => "fab fa-vimeo",
        var n when n.Contains("tumblr") => "fab fa-tumblr",
        var n when n.Contains("medium") => "fab fa-medium",

        // Académicos
        var n when n.Contains("scholar") => "fas fa-graduation-cap",
        var n when n.Contains("researchgate") || n.Contains("research") => "fas fa-microscope",
        var n when n.Contains("orcid") => "fab fa-orcid",
        var n when n.Contains("academia") => "fas fa-university",

        // Otros
        var n when n.Contains("skype") => "fab fa-skype",

        _ => "fas fa-link"
    };
}

        
        private string ObtenerColorFondo()
{
    return Nombre.ToLower() switch
    {
        var n when n.Contains("email") || n.Contains("correo") => "#EA4335",
        var n when n.Contains("teléfono") || n.Contains("telefono") || n.Contains("cel") => "#34A853",
        var n when n.Contains("linkedin") => "#0077B5",
        var n when n.Contains("twitter") || n.Contains("x") => "#1DA1F2",
        var n when n.Contains("facebook") => "#1877F2",
        var n when n.Contains("instagram") => "#E4405F",
        var n when n.Contains("github") => "#181717",
        var n when n.Contains("youtube") => "#FF0000",
        var n when n.Contains("tiktok") => "#000000",
        var n when n.Contains("whatsapp") => "#25D366",
        var n when n.Contains("telegram") => "#0088CC",
        var n when n.Contains("slack") => "#4A154B",
        var n when n.Contains("discord") => "#5865F2",
        var n when n.Contains("behance") => "#1769FF",
        var n when n.Contains("dribbble") => "#EA4C89",
        var n when n.Contains("reddit") => "#FF4500",
        var n when n.Contains("pinterest") => "#E60023",
        var n when n.Contains("snapchat") => "#FFFC00",
        var n when n.Contains("threads") => "#000000",
        var n when n.Contains("mastodon") => "#6364FF",
        var n when n.Contains("vimeo") => "#1AB7EA",
        var n when n.Contains("tumblr") => "#36465D",
        var n when n.Contains("medium") => "#00AB6C",
        var n when n.Contains("orcid") => "#A6CE39",
        var n when n.Contains("scholar") => "#4285F4",
        var n when n.Contains("academia") => "#2E2E2E",
        _ => "#2D6B3C" // Color UABC por defecto
    };
}

        
        private string ObtenerPlaceholderTexto()
{
    return Nombre.ToLower() switch
    {
        var n when n.Contains("email") || n.Contains("correo") => "ejemplo@correo.com",
        var n when n.Contains("teléfono") || n.Contains("telefono") || n.Contains("cel") => "+52 664 123 4567",
        var n when n.Contains("linkedin") => "https://linkedin.com/in/usuario",
        var n when n.Contains("twitter") || n.Contains("x") => "https://twitter.com/usuario",
        var n when n.Contains("facebook") => "https://facebook.com/usuario",
        var n when n.Contains("instagram") => "https://instagram.com/usuario",
        var n when n.Contains("github") => "https://github.com/usuario",
        var n when n.Contains("youtube") => "https://youtube.com/c/usuario",
        var n when n.Contains("tiktok") => "https://tiktok.com/@usuario",
        var n when n.Contains("whatsapp") => "+52 664 123 4567",
        var n when n.Contains("telegram") => "@usuario",
        var n when n.Contains("slack") => "usuario@equipo.slack.com",
        var n when n.Contains("discord") => "usuario#1234",
        var n when n.Contains("behance") => "https://behance.net/usuario",
        var n when n.Contains("dribbble") => "https://dribbble.com/usuario",
        var n when n.Contains("reddit") => "https://reddit.com/u/usuario",
        var n when n.Contains("pinterest") => "https://pinterest.com/usuario",
        var n when n.Contains("snapchat") => "@usuario",
        var n when n.Contains("threads") => "https://www.threads.net/@usuario",
        var n when n.Contains("mastodon") => "https://mastodon.social/@usuario",
        var n when n.Contains("vimeo") => "https://vimeo.com/usuario",
        var n when n.Contains("tumblr") => "https://usuario.tumblr.com",
        var n when n.Contains("medium") => "https://medium.com/@usuario",
        var n when n.Contains("skype") => "usuario.skype",
        var n when n.Contains("web") || n.Contains("página") || n.Contains("sitio") => "https://mipagina.com",
        var n when n.Contains("blog") => "https://miblog.com",
        var n when n.Contains("scholar") => "https://scholar.google.com/citations?user=XXXXXXX",
        var n when n.Contains("orcid") => "https://orcid.org/0000-0000-0000-0000",
        var n when n.Contains("academia") => "https://independent.academia.edu/usuario",
        _ => "https://enlace.com"
    };
}

    }
}
