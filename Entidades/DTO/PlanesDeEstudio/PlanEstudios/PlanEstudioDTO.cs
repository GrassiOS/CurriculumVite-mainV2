using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Entidades.DTO.PlanesDeEstudio.PlanEstudios
{
  public class PlanEstudioDTO
  {
    public int? IdPlanEstudio { get; set; }

    [Required(ErrorMessage = "Debe capturar el plan de estudios.")]
    [RegularExpression(@"^\d{4}-[124]$", ErrorMessage = "El formato debe ser AAAA-D, donde AAAA es un año y D es 1, 2 o 4.")]
    public string PlanEstudio { get; set; }

    [Required]
    public DateTime FechaCreacion { get; set; } = DateTime.Now;

    [Required(ErrorMessage = "Debe capturar el total de créditos.")]
    public int TotalCreditos { get; set; }
    [Required(ErrorMessage = "Debe capturar los créditos obligatorios.")]
    public int CreditosOptativos { get; set; }
    [Required(ErrorMessage = "Debe capturar los créditos optativos.")]
    public int CreditosObligatorios { get; set; }
    public string PerfilDeIngreso { get; set; }

    [Required(ErrorMessage = "Debe capturar el perfil de egreso.")]
    public string PerfilDeEgreso { get; set; }

    [Required(ErrorMessage = "Debe capturar el campo ocupacional.")]
    public string CampoOcupacional { get; set; }

    public string Comentarios { get; set; }
    
    [Required]
    public bool EstadoPlanEstudio { get; set; } = true;

    // Propiedad de navegación
    // Llave Foranea Muchos PlanEstudios pertenecen a una carrera    
    [ForeignKey("IdCarrera")]
    public int IdCarrera { get; set; }
  }
}
