using System.ComponentModel.DataAnnotations;

namespace Entidades.DTO.PlanesDeEstudio.Materias
{
  public class MateriaDTO
  {    
    public int? IdMateria { get; set; }
    
    [Required(ErrorMessage = "Debe capturar la clave de la materia.")]
    [RegularExpression(@"^\d{1,6}$", ErrorMessage = "La clave de la materia debe tener máximo 6 dígitos.")]
    public string ClaveMateria { get; set; }
    
    [Required(ErrorMessage = "Debe capturar el nombre de la materia.")]
    [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚüÜñÑ\s\-,.']+$", ErrorMessage = "El nombre de la mataria solo puede contener letras, espacios y los siguientes caracteres especiales: - , . '")]
    public string NombreMateria { get; set; }
    
    [Required(ErrorMessage = "Debe capturar las horas clase.")]
    public int HC { get; set; }
    
    [Required(ErrorMessage = "Debe capturar las horas laboratorio.")]
    public int HL { get; set; }
    
    [Required(ErrorMessage = "Debe capturar las horas taller.")]
    public int HT { get; set; }
    
    [Required(ErrorMessage = "Debe capturar las horas prácticas clínicas.")]
    public int HPC { get; set; }
    
    [Required(ErrorMessage = "Debe capturar las horas clase laboratorio.")]
    public int HCL { get; set; }
    
    [Required(ErrorMessage = "Debe capturar las horas extraclase.")]
    public int HE { get; set; }
    
    [Required(ErrorMessage = "Debe capturar los créditos.")]
    public int CR { get; set; }
    
    [Required(ErrorMessage = "Debe capturar el propocito general.")]
    public string PropositoGeneral { get; set; }
    
    [Required(ErrorMessage = "Debe capturar la competencia.")]
    public string Competencia { get; set; }
    
    [Required(ErrorMessage = "Debe capturar las evidencias.")]
    public string Evidencia { get; set; }
    
    [Required(ErrorMessage = "Debe capturar la metodología de trabajo.")]
    public string Metodologia { get; set; }
    
    [Required(ErrorMessage = "Debe capturar los crterios de avaluación.")]
    public string Criterios { get; set; }
    
    [Required(ErrorMessage = "Debe capturar la bibliografia básica.")]
    public string BibliografiaBasica { get; set; }
    
    [Required(ErrorMessage = "Debe capturar la bibliografia complemantaria.")]
    public string BibliografiaComplementaria { get; set; }
    
    [Required(ErrorMessage = "Debe capturar al perfil del docente.")]
    public string PerfilDocente { get; set; }

    [Required]
    public bool EstadoMateria { get; set; }
  }
}
