namespace Entidades.DTO.CurriculumVite
{
    public class SexoDTO
    {
        public int IdSexo { get; set; }
        public string Sexo { get; set; } = null!;
    }

    public class EstadoCivilDTO
    {
        public int IdEstadoCivil { get; set; }
        public string EstadoCivil { get; set; } = null!;
    }

    public class CategoriaDTO
    {
        public int IdCategoria { get; set; }
        public string ClaveCategoria { get; set; } = null!;
        public string NombreCategoria { get; set; } = null!;
    }

    public class NombramientoDTO
    {
        public int IdNombramiento { get; set; }
        public string Nombramiento { get; set; } = null!;
    }

    public class EscolaridadDTO
    {
        public int IdEscolaridad { get; set; }
        public string ClaveEscolaridad { get; set; } = null!;
        public string NombreEscolaridad { get; set; } = null!;
    }

    public class SNIDTO
    {
        public int IdNivelSNI { get; set; }
        public string NivelSNI { get; set; } = null!;
    }

    public class PRODEPDTO
    {
        public int IdPRODEP { get; set; }
        public string TienePRODEP { get; set; } = null!;
    }

    public class CarreraDTO
    {
        public int IdCarrera { get; set; }
        public int IdCoordinador { get; set; }
        public string ClaveCarrera { get; set; } = null!;
        public string NombreCarrera { get; set; } = null!;
        public string AliasCarrera { get; set; } = null!;
        public bool EstadoCarrera { get; set; } = true;
        public string? NombreCoordinador { get; set; }
    }

    public class CuerpoAcademicoDTO
    {
        public int CuerpoAcademicoId { get; set; }
        public string Nombre { get; set; } = null!;
        public string? Descripcion { get; set; }
        public int CantidadDocentes { get; set; }
    }
} 