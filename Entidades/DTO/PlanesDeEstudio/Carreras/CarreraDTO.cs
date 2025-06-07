using System.ComponentModel.DataAnnotations;

namespace Entidades.DTO.PlanesDeEstudio.Carreras
{
  public class CarreraDTO
  {
    // Solo requerido para modificaciones
    public int? IdCarrera { get; set; }


    [Required(ErrorMessage = "Debe capturar la clave de la carrera.")]
    [RegularExpression(@"^\d{3}$", ErrorMessage = "La clave de la carrera debe contener exactamente 3 dígitos.")]
    public string ClaveCarrera { get; set; }

    [Required(ErrorMessage = "Debe capturar el nombre de la carrera.")]
    [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚüÜñÑ\s\-,.']+$",
        ErrorMessage = "El nombre de la carrera solo puede contener letras, espacios y los siguientes caracteres especiales: - , . '")]
    public string NombreCarrera { get; set; }

    [Required(ErrorMessage = "Debe capturar el Alias de la carrera.")]
    [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚüÜñÑ\s\-,.']+$",
        ErrorMessage = "El alias de la carrera solo puede contener letras, espacios y los siguientes caracteres especiales: - , . '")]
    public string AliasCarrera { get; set; }

    public bool EstadoCarrera { get; set; }
  }
}
