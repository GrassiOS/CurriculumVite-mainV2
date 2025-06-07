using Entidades.Modelos.PlanesDeEstudio.PlanEstudios;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entidades.Modelos.PlanesDeEstudio.Etapas
{
  public class E_Etapa
  {
    public int IdEtapa { get; set; }
    public string NombreEtapa { get; set; }

    // Navigation property
    public ICollection<E_PlanEstudioMateria> PlanEstudioMaterias { get; set; }
  }
}