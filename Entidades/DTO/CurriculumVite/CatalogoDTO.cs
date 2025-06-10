using System.ComponentModel.DataAnnotations;

namespace Entidades.DTO.CurriculumVite
{
    public class SexoDTO
    {
        public int IdSexo { get; set; }
        
        [Required(ErrorMessage = "El sexo es obligatorio")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "El sexo debe tener entre 1 y 50 caracteres")]
        public string Sexo { get; set; } = null!;
    }

    public class EstadoCivilDTO
    {
        public int IdEstadoCivil { get; set; }
        
        [Required(ErrorMessage = "El estado civil es obligatorio")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "El estado civil debe tener entre 1 y 50 caracteres")]
        public string EstadoCivil { get; set; } = null!;
    }

    public class CategoriaDTO
    {
        public int IdCategoria { get; set; }
        
        [Required(ErrorMessage = "La clave de categoría es obligatoria")]
        [StringLength(10, MinimumLength = 1, ErrorMessage = "La clave debe tener entre 1 y 10 caracteres")]
        public string ClaveCategoria { get; set; } = null!;
        
        [Required(ErrorMessage = "El nombre de categoría es obligatorio")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 100 caracteres")]
        public string NombreCategoria { get; set; } = null!;
    }

    public class NombramientoDTO
    {
        public int IdNombramiento { get; set; }
        
        [Required(ErrorMessage = "El nombramiento es obligatorio")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombramiento debe tener entre 2 y 100 caracteres")]
        public string Nombramiento { get; set; } = null!;
    }

    public class EscolaridadDTO
    {
        public int IdEscolaridad { get; set; }
        
        [Required(ErrorMessage = "La clave de escolaridad es obligatoria")]
        [StringLength(10, MinimumLength = 1, ErrorMessage = "La clave debe tener entre 1 y 10 caracteres")]
        public string ClaveEscolaridad { get; set; } = null!;
        
        [Required(ErrorMessage = "El nombre de escolaridad es obligatorio")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 100 caracteres")]
        public string NombreEscolaridad { get; set; } = null!;
    }

    public class SNIDTO
    {
        public int IdNivelSNI { get; set; }
        
        [Required(ErrorMessage = "El nivel SNI es obligatorio")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "El nivel SNI debe tener entre 1 y 50 caracteres")]
        public string NivelSNI { get; set; } = null!;
    }

    public class PRODEPDTO
    {
        public int IdPRODEP { get; set; }
        
        [Required(ErrorMessage = "El estado PRODEP es obligatorio")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "El estado PRODEP debe tener entre 1 y 50 caracteres")]
        public string TienePRODEP { get; set; } = null!;
    }

    public class CarreraDTO
    {
        public int IdCarrera { get; set; }
        
        [Required(ErrorMessage = "Debe seleccionar un coordinador")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un coordinador válido")]
        public int IdCoordinador { get; set; }
        
        [Required(ErrorMessage = "La clave de carrera es obligatoria")]
        [RegularExpression(@"^\d{3}$", ErrorMessage = "La clave de la carrera debe contener exactamente 3 dígitos")]
        public string ClaveCarrera { get; set; } = null!;
        
        [Required(ErrorMessage = "El nombre de carrera es obligatorio")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 200 caracteres")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚüÜñÑ\s\-,.']+$", ErrorMessage = "El nombre solo puede contener letras, espacios y los caracteres: - , . '")]
        public string NombreCarrera { get; set; } = null!;
        
        [Required(ErrorMessage = "El alias de carrera es obligatorio")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "El alias debe tener entre 2 y 100 caracteres")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚüÜñÑ\s\-,.']+$", ErrorMessage = "El alias solo puede contener letras, espacios y los caracteres: - , . '")]
        public string AliasCarrera { get; set; } = null!;
        
        public bool EstadoCarrera { get; set; } = true;
        public string? NombreCoordinador { get; set; }
    }

    public class CuerpoAcademicoDTO
    {
        public int CuerpoAcademicoId { get; set; }
        
        [Required(ErrorMessage = "El nombre del cuerpo académico es obligatorio")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 200 caracteres")]
        public string Nombre { get; set; } = null!;
        
        [StringLength(1000, ErrorMessage = "La descripción no puede exceder 1000 caracteres")]
        public string? Descripcion { get; set; }
        
        public int CantidadDocentes { get; set; }
    }
} 