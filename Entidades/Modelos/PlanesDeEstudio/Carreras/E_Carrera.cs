using Entidades.Modelos.PlanesDeEstudio.PlanEstudios;
using System.ComponentModel.DataAnnotations;

namespace Entidades.Modelos.PlanesDeEstudio.Carreras
{
  public class E_Carrera
  {
    public int IdCarrera { get; set; }
    public string ClaveCarrera { get; set; }
    public string NombreCarrera { get; set; }
    public string AliasCarrera { get; set; }
    public bool EstadoCarrera { get; set; } = true;

    // Navigation property
    public ICollection<E_PlanEstudio> PlanesEstudio { get; set; }    
  }
}
