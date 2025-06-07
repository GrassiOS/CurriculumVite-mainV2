using Entidades.Modelos.PlanesDeEstudio.PlanEstudios;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entidades.Modelos.PlanesDeEstudio.Materias
{
  public class E_Materia
  {
    public int IdMateria { get; set; }
    public string ClaveMateria { get; set; }
    public string NombreMateria { get; set; }
    public int HC { get; set; }
    public int HL { get; set; }
    public int HT { get; set; }
    public int HPC { get; set; }
    public int HCL { get; set; }
    public int HE { get; set; }
    public int CR { get; set; }
    public string PropositoGeneral { get; set; }
    public string Competencia { get; set; }
    public string Evidencia { get; set; }
    public string Metodologia { get; set; }
    public string Criterios { get; set; }
    public string BibliografiaBasica { get; set; }
    public string BibliografiaComplementaria { get; set; }
    public string PerfilDocente { get; set; }
    public string PathPUA { get; set; }
    public bool EstadoMateria { get; set; }

    // Navigation property
    public ICollection<E_PlanEstudioMateria> PlanEstudioMaterias { get; set; }
  }
}