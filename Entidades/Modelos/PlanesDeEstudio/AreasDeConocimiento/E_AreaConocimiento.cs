using Entidades.Modelos.PlanesDeEstudio.PlanEstudios;
using System.ComponentModel.DataAnnotations;

namespace Entidades.Modelos.PlanesDeEstudio.AreasDeConocimiento
{
  public class E_AreaConocimiento
  {
    public int IdAreaConocimiento { get; set; }
    public string ClaveAreaConocimiento { get; set; }
    public string DescripcionAreaConocimiento { get; set; }

    // Navigation property
    public ICollection<E_PlanEstudioMateria> PlanEstudioMaterias { get; set; }
  }
}