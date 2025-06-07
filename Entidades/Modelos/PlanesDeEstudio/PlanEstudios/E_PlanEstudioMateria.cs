using Entidades.Modelos.PlanesDeEstudio.AreasDeConocimiento;
using Entidades.Modelos.PlanesDeEstudio.Etapas;
using Entidades.Modelos.PlanesDeEstudio.Materias;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entidades.Modelos.PlanesDeEstudio.PlanEstudios
{
  public class E_PlanEstudioMateria
  {
    public int IdPlanEstudioMateria { get; set; }
   // Foreign keys
    public int IdPlanEstudio { get; set; }
    public int IdMateria { get; set; }
    public int IdEtapa { get; set; }
    public int IdAreaConocimiento { get; set; }
    public int Semestre { get; set; }
    // Navigation properties
    public E_PlanEstudio PlanEstudio { get; set; }
    public E_Materia Materia { get; set; }
    public E_Etapa Etapa { get; set; }
    public E_AreaConocimiento AreaConocimiento { get; set; }
  }
}
